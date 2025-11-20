export interface Cart {
  id: number;
  items: CartItem[];
}

export interface CartItem {
  id: number;
  productId: number;
  productName: string;
  imageUrl?: string;
  quantity: number;
  price: number;
  variantSize?: string;
  variantColor?: string;
}

export interface AddToCartCommand {
  productId: number;
  variantId?: number;
  quantity: number;
}

export interface RemoveFromCartCommand {
  itemId: number;
}

export interface CartSummary {
  itemCount: number;
  subtotal: number;
  shipping: number;
  tax: number;
  total: number;
}
