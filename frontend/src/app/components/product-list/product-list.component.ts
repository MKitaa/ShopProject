import {Component, OnInit} from '@angular/core';
import {Product} from '../../Interfaces/product';
import {ProductService} from '../../services/product.service';
import {MatTableModule} from '@angular/material/table';
import {MatButtonModule} from '@angular/material/button';
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import {CommonModule} from '@angular/common';
import {MatRadioButton} from '@angular/material/radio';
import {MatDialog} from '@angular/material/dialog';
import {EditProductComponent} from '../edit-product/edit-product.component';
import {MatIcon} from '@angular/material/icon';
import {MatMenuItem} from '@angular/material/menu';
import {CartService} from '../../services/cart.service';
import {AddToCart} from '../../Interfaces/add-to-cart';
import {FormControl, ReactiveFormsModule, Validators} from '@angular/forms';
import {MatFormField, MatFormFieldModule} from '@angular/material/form-field';
import {MatInput} from '@angular/material/input';
import {AuthService} from '../../services/auth.service';
import {CheckRoleDirective} from '../../Directives/check-role.directive';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    MatTableModule,
    MatButtonModule,
    MatPaginatorModule,
    MatSortModule,
    CommonModule,
    MatRadioButton,
    MatIcon,
    MatMenuItem,
    ReactiveFormsModule,
    MatFormField,
    MatInput,
    MatFormFieldModule,
    ReactiveFormsModule,
    CheckRoleDirective,
  ],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  displayedColumns: string[] = ['name', 'description', 'price', 'stockQuantity', 'actions'];
  minPrice = new FormControl(0, [Validators.required]);
  maxPrice = new FormControl(0, [Validators.required]);

  constructor(private productService: ProductService,
              public dialog: MatDialog,
              private cartService: CartService,
              private authService: AuthService
  ) {
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAllProducts().subscribe((data: any) => {
      if (data && data.$values) {
        this.products = data.$values;
      }
    });
  }

  editProduct(product: any): void {
    const dialogRef = this.dialog.open(EditProductComponent, {
      width: '400px',
      data: {product}
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadProducts();
      }
    });
  }

  deleteProduct(product: any): void {
    const confirmDelete = window.confirm('Are you sure you want to delete this product?');
    if (confirmDelete) {
      this.productService.deleteProduct(product.id).subscribe(() => {
        this.products = this.products.filter(p => p.id !== product.id);
      });
    }
  }

  addProductToCard(product: Product) {
    console.log('Product to add to cart', product);
    const productToAdd: AddToCart = {
      productId: product.id,
      quantity: 1
    };
    this.cartService.addProductToCart(productToAdd).subscribe(value => {
      console.log('Product added to cart', value);
    });
  }

  filterProducts() {
    this.productService.getFilteredProducts(<number>this.minPrice.value, <number>this.maxPrice.value).subscribe((data: any) => {
      if (data && data.$values) {
        this.products = data.$values;
      }
    });
  }
}
