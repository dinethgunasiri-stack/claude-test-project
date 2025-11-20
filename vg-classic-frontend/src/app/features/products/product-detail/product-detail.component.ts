import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../../../core/services/api.service';
import { CartService } from '../../../core/services/cart.service';
import { AuthService } from '../../../core/services/auth.service';
import { ProductDetail } from '../../../core/models/product.model';
import { ApiResponse } from '../../../core/models/api-response.model';

@Component({
  selector: 'app-product-detail',
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.scss']
})
export class ProductDetailComponent implements OnInit {
  product: ProductDetail | null = null;
  loading = false;
  error = '';
  selectedImage = 0;
  selectedVariantId: number | null = null;
  quantity = 1;
  addingToCart = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiService: ApiService,
    private cartService: CartService,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadProduct(+id);
    }
  }

  loadProduct(id: number): void {
    this.loading = true;
    this.error = '';

    this.apiService.get<ApiResponse<ProductDetail>>(`Products/${id}`)
      .subscribe({
        next: (response) => {
          if (response.isSuccess && response.value) {
            this.product = response.value;
          } else {
            this.error = 'Product not found';
          }
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load product';
          this.loading = false;
        }
      });
  }

  selectImage(index: number): void {
    this.selectedImage = index;
  }

  selectVariant(variantId: number): void {
    this.selectedVariantId = variantId;
  }

  addToCart(): void {
    if (!this.authService.isAuthenticated) {
      this.router.navigate(['/auth/login'], {
        queryParams: { returnUrl: this.router.url }
      });
      return;
    }

    if (!this.product) return;

    this.addingToCart = true;
    this.cartService.addToCart({
      productId: this.product.id,
      variantId: this.selectedVariantId || undefined,
      quantity: this.quantity
    }).subscribe({
      next: (response) => {
        if (response.isSuccess) {
          // Show success message (you could add a toast notification here)
          alert('Product added to cart!');
        }
        this.addingToCart = false;
      },
      error: (err) => {
        alert('Failed to add product to cart');
        this.addingToCart = false;
      }
    });
  }

  increaseQuantity(): void {
    if (this.quantity < 10) {
      this.quantity++;
    }
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }
}
