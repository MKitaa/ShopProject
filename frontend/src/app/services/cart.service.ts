import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AddToCart} from '../Interfaces/add-to-cart';
import {Cart} from '../Interfaces/cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private apiUrl = 'http://localhost:5223/api/cart';

  constructor(private http: HttpClient) {
  }

  addProductToCart(product: AddToCart): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, product, {withCredentials: true});
  }

  getCart(): Observable<any> {
    return this.http.get(`${this.apiUrl}`, {withCredentials: true});
  }

  updateQuantity(cartItem: Cart, newQuantity: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/update/${cartItem.productId}`, newQuantity, {withCredentials: true});
  }

  deleteFromCart(cartItem: Cart): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete/${cartItem.productId}`, {withCredentials: true});
  }

  getTotalCost(cartId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/totalPrice/${cartId}`, {withCredentials: true});
  }
}
