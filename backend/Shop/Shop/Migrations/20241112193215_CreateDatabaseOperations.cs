using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabaseOperations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" CREATE OR REPLACE FUNCTION update_timestamp()
            RETURNS TRIGGER AS $$
            BEGIN
                NEW.""UpdatedAt"" = NOW();
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;");
            
            // Tworzenie wyzwalacza dla aktualizacji znacznika czasu w tabeli Products
            migrationBuilder.Sql(@"
            CREATE TRIGGER update_product_timestamp
            BEFORE UPDATE ON ""Products""
            FOR EACH ROW
            EXECUTE FUNCTION update_timestamp();
        ");
            // Tworzenie procedury do przypisywania roli użytkownikowi
            migrationBuilder.Sql(@"
            CREATE OR REPLACE PROCEDURE assign_user_role(user_id text, role_name text)
            LANGUAGE plpgsql
            AS $$
            BEGIN
                INSERT INTO public.""AspNetUserRoles"" (""UserId"", ""RoleId"")
                SELECT user_id, r.""Id""
                FROM public.""AspNetRoles"" r
                WHERE r.""Name"" = role_name;
            END;
            $$;
        ");
            migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION trigger_assign_user_role() 
            RETURNS TRIGGER AS $$ 
            BEGIN 
            CALL assign_user_role(NEW.""Id"", 'User'); 
            RETURN NEW; 
            END; 
            $$ LANGUAGE plpgsql; 
");
            
            // Tworzenie wyzwalacza na prawidłowy sposób przypisywania roli użytkownikowi
            migrationBuilder.Sql(@"
            CREATE TRIGGER set_user_role 
            AFTER INSERT ON public.""AspNetUsers"" 
            FOR EACH ROW 
            EXECUTE FUNCTION trigger_assign_user_role(); 
        ");
              migrationBuilder.Sql(@"
            CREATE OR REPLACE FUNCTION calculate_total_price(cart_id UUID)
            RETURNS DECIMAL AS $$
            DECLARE
                total_price DECIMAL := 0;
            BEGIN
                SELECT SUM(COALESCE(p.""Price"", 0) * COALESCE(ci.""Quantity"", 0)) INTO total_price
                FROM public.""ShoppingCartItems"" ci
                JOIN public.""Products"" p ON ci.""ProductId"" = p.""Id""
                WHERE ci.""ShoppingCartId"" = cart_id;

                RETURN COALESCE(total_price, 0);
            END;
            $$ LANGUAGE plpgsql;
        ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {      
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS set_user_role  ON public.""AspNetUsers"";");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS assign_user_role;");
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS update_product_timestamp ON ""Products"";");
            migrationBuilder.Sql(@"DROP FUNCTION IF EXISTS update_timestamp;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS calculate_total_price;");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS trigger_assign_user_role;");
        }
    }
}
