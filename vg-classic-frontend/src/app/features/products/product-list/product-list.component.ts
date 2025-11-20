import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../core/services/api.service';
import { Product, ProductFilters, PaginatedList } from '../../../core/models/product.model';
import { ApiResponse } from '../../../core/models/api-response.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  loading = false;
  error = '';

  filters: ProductFilters = {
    pageNumber: 1,
    pageSize: 12,
    sortBy: 'name',
    sortOrder: 'asc'
  };

  totalPages = 0;
  totalCount = 0;

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.error = '';

    this.apiService.get<ApiResponse<PaginatedList<Product>>>('Products', this.filters)
      .subscribe({
        next: (response) => {
          if (response.isSuccess && response.value) {
            this.products = response.value.items;
            this.totalPages = response.value.totalPages;
            this.totalCount = response.value.totalCount;
          }
          this.loading = false;
        },
        error: (err) => {
          this.error = 'Failed to load products';
          this.loading = false;
        }
      });
  }

  onPageChange(page: number): void {
    this.filters.pageNumber = page;
    this.loadProducts();
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onSortChange(sortBy: string): void {
    if (this.filters.sortBy === sortBy) {
      this.filters.sortOrder = this.filters.sortOrder === 'asc' ? 'desc' : 'asc';
    } else {
      this.filters.sortBy = sortBy;
      this.filters.sortOrder = 'asc';
    }
    this.filters.pageNumber = 1;
    this.loadProducts();
  }

  onSearch(searchTerm: string): void {
    this.filters.searchTerm = searchTerm || undefined;
    this.filters.pageNumber = 1;
    this.loadProducts();
  }

  clearFilters(): void {
    this.filters = {
      pageNumber: 1,
      pageSize: 12,
      sortBy: 'name',
      sortOrder: 'asc'
    };
    this.loadProducts();
  }
}
