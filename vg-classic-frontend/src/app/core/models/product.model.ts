export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  compareAtPrice?: number;
  categoryName: string;
  brand: string;
  averageRating: number;
  reviewCount: number;
  isFeatured: boolean;
  images: string[];
  variants: ProductVariant[];
}

export interface ProductDetail extends Product {
  detailedDescription: string;
  sku: string;
  stockQuantity: number;
  viewCount: number;
}

export interface ProductVariant {
  id: number;
  size?: string;
  color?: string;
  colorHex?: string;
  isInStock: boolean;
  additionalPrice: number;
}

export interface ProductFilters {
  categoryId?: number;
  minPrice?: number;
  maxPrice?: number;
  searchTerm?: string;
  isFeatured?: boolean;
  sortBy?: string;
  sortOrder?: string;
  pageNumber: number;
  pageSize: number;
}

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface CreateProductCommand {
  name: string;
  description: string;
  detailedDescription: string;
  price: number;
  compareAtPrice?: number;
  categoryId: number;
  brand: string;
  sku: string;
  stockQuantity: number;
  isFeatured: boolean;
  variants: string[];
  imageUrls: string[];
}
