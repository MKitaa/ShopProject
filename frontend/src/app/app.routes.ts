import {Routes} from '@angular/router';
import {RegisterComponent} from './components/authentication/register/register.component';
import {LoginComponent} from './components/authentication/login/login.component';
import {CreateProductComponent} from './components/create-product/create-product.component';
import {ProductListComponent} from './components/product-list/product-list.component';
import {AuthGuardService} from './services/auth-guard.service';
import {CartComponent} from './components/oder-basket/cart.component';
export const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'add-product', component: CreateProductComponent, canActivate: [AuthGuardService]},
  {path: 'products', component: ProductListComponent, canActivate: [AuthGuardService]},
  {path: 'order-basket', component: CartComponent, canActivate: [AuthGuardService]},
  {path: '', redirectTo: '/login', pathMatch: 'full'},
  {path: '**', redirectTo: '/login'},
];
