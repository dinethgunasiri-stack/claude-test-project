import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartService } from '../../core/services/cart.service';
import { Cart, CartItem, CartSummary } from '../../core/models/cart.model';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cart: Cart | null = null;
  loading = false;
  removing: { [key: number]: boolean } = {};

  constructor(
    public cartService: CartService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.loading = true;
    this.cartService.loadCart().subscribe({
      next: () => {
        this.cart = this.cartService.cartValue;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  removeItem(itemId: number): void {
    if (!confirm('Remove this item from cart?')) return;

    this.removing[itemId] = true;
    this.cartService.removeFromCart(itemId).subscribe({
      next: () => {
        this.cart = this.cartService.cartValue;
        this.removing[itemId] = false;
      },
      error: () => {
        this.removing[itemId] = false;
        alert('Failed to remove item');
      }
    });
  }

  get summary(): CartSummary {
    return this.cartService.getCartSummary();
  }

  proceedToCheckout(): void {
    this.router.navigate(['/checkout']);
  }

  continueShopping(): void {
    this.router.navigate(['/products']);
  }
}
