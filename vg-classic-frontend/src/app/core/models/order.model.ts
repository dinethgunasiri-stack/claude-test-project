export interface Order {
  id: number;
  orderNumber: string;
  orderDate: Date;
  status: string;
  paymentStatus: string;
  totalAmount: number;
  shippingAddressLine1: string;
  shippingCity: string;
  shippingState: string;
  shippingZipCode: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: number;
  productName: string;
  variantSize?: string;
  variantColor?: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface CreateOrderCommand {
  shippingFirstName: string;
  shippingLastName: string;
  shippingAddressLine1: string;
  shippingAddressLine2?: string;
  shippingCity: string;
  shippingState: string;
  shippingZipCode: string;
  shippingCountry: string;
  shippingPhone: string;
  customerNotes?: string;
}
