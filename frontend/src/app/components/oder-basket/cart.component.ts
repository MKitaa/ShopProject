import {Component, OnInit} from '@angular/core';
import {
  MatTableModule
} from '@angular/material/table';
import {Cart} from '../../Interfaces/cart';
import {CurrencyPipe} from '@angular/common';
import {CartService} from '../../services/cart.service';
import {MatInput} from '@angular/material/input';
import {MatButton} from '@angular/material/button';
import {MatIcon} from '@angular/material/icon';
import {MatMenuItem} from '@angular/material/menu';

@Component({
  selector: 'app-oder-basket',
  standalone: true,
  imports: [MatTableModule, CurrencyPipe, MatInput, MatButton, MatIcon, MatMenuItem],

  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  displayedColumns: string[] = ['name', 'price', 'quantity', 'actions'];
  cartItems!: Cart[];
  totalCost!: number;

  constructor(private cartService: CartService) {
  }

  ngOnInit() {
    this.loadCart()
    this.loadTotalCost();
  }

  loadCart(): void {
    this.cartService.getCart().subscribe((response) => {
      if (response && response.$values) {
        this.cartItems = response.$values;
      }
    });
  }

  loadTotalCost() {
    return this.cartService.getTotalCost(6).subscribe((response) => {
      this.totalCost = response;
    });
  }

  getTotalCost() {
    return this.totalCost;
  }

  updateQuantity(cartItem: Cart, event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const newQuantity = Number(inputElement.value);
    this.cartService.updateQuantity(cartItem, <number>newQuantity).subscribe(() => {
    });
  }

  deleteFromCart(cartItem: Cart): void {
    this.cartService.deleteFromCart(cartItem).subscribe(() => {
      this.loadCart();
    });
  }
}
