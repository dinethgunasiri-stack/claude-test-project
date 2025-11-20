import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  Cart,
  AddToCartCommand,
  RemoveFromCartCommand,
  CartSummary
} from '../models/cart.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartSubject: BehaviorSubject<Cart | null>;
  public cart$: Observable<Cart | null>;

  constructor(private apiService: ApiService) {
    this.cartSubject = new BehaviorSubject<Cart | null>(null);
    this.cart$ = this.cartSubject.asObservable();
  }

  public get cartValue(): Cart | null {
    return this.cartSubject.value;
  }

  public get itemCount(): number {
    return this.cartValue?.items.reduce((sum, item) => sum + item.quantity, 0) || 0;
  }

  loadCart(): Observable<ApiResponse<Cart>> {
    return this.apiService.get<ApiResponse<Cart>>('Carts')
      .pipe(
        tap(response => {
          if (response.isSuccess && response.value) {
            this.cartSubject.next(response.value);
          }
        })
      );
  }

  addToCart(command: AddToCartCommand): Observable<ApiResponse<Cart>> {
    return this.apiService.post<ApiResponse<Cart>>('Carts/add', command)
      .pipe(
        tap(response => {
          if (response.isSuccess && response.value) {
            this.cartSubject.next(response.value);
          }
        })
      );
  }

  removeFromCart(itemId: number): Observable<ApiResponse<Cart>> {
    return this.apiService.delete<ApiResponse<Cart>>(`Carts/remove/${itemId}`)
      .pipe(
        tap(response => {
          if (response.isSuccess && response.value) {
            this.cartSubject.next(response.value);
          }
        })
      );
  }

  getCartSummary(): CartSummary {
    const cart = this.cartValue;
    if (!cart || !cart.items.length) {
      return {
        itemCount: 0,
        subtotal: 0,
        shipping: 0,
        tax: 0,
        total: 0
      };
    }

    const subtotal = cart.items.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    const shipping = subtotal > 0 ? 10.00 : 0; // Fixed shipping
    const tax = subtotal * 0.08; // 8% tax
    const total = subtotal + shipping + tax;

    return {
      itemCount: cart.items.length,
      subtotal,
      shipping,
      tax,
      total
    };
  }

  clearCart(): void {
    this.cartSubject.next(null);
  }
}
