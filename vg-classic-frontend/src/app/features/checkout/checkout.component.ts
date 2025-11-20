import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../core/services/api.service';
import { CartService } from '../../core/services/cart.service';
import { CreateOrderCommand } from '../../core/models/order.model';
import { ApiResponse } from '../../core/models/api-response.model';
import { CartSummary } from '../../core/models/cart.model';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  checkoutForm: FormGroup;
  loading = false;
  submitted = false;
  error = '';

  constructor(
    private formBuilder: FormBuilder,
    private apiService: ApiService,
    public cartService: CartService,
    private router: Router
  ) {
    this.checkoutForm = this.formBuilder.group({
      shippingFirstName: ['', Validators.required],
      shippingLastName: ['', Validators.required],
      shippingAddressLine1: ['', Validators.required],
      shippingAddressLine2: [''],
      shippingCity: ['', Validators.required],
      shippingState: ['', Validators.required],
      shippingZipCode: ['', [Validators.required, Validators.pattern(/^\d{5}$/)]],
      shippingCountry: ['USA', Validators.required],
      shippingPhone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      customerNotes: ['']
    });
  }

  ngOnInit(): void {
    // Check if cart is empty
    if (!this.cartService.cartValue || this.cartService.cartValue.items.length === 0) {
      this.router.navigate(['/cart']);
    }
  }

  get f() { return this.checkoutForm.controls; }

  get summary(): CartSummary {
    return this.cartService.getCartSummary();
  }

  onSubmit(): void {
    this.submitted = true;
    this.error = '';

    if (this.checkoutForm.invalid) {
      return;
    }

    this.loading = true;
    const orderCommand: CreateOrderCommand = this.checkoutForm.value;

    this.apiService.post<ApiResponse<number>>('Orders', orderCommand)
      .subscribe({
        next: (response) => {
          if (response.isSuccess) {
            this.cartService.clearCart();
            alert('Order placed successfully!');
            this.router.navigate(['/products']);
          } else {
            this.error = response.errors?.join(', ') || 'Failed to place order';
            this.loading = false;
          }
        },
        error: (err) => {
          this.error = 'Failed to place order. Please try again.';
          this.loading = false;
        }
      });
  }
}
