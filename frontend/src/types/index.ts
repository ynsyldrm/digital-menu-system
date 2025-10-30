export interface MenuItem {
  id: string;
  name: string;
  description: string;
  price: number;
  categoryId: string;
  categoryName: string;
  isAvailable: boolean;
}

export interface OrderItem {
  menuItemId: string;
  menuItemName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface Order {
  id: string;
  customerId: string;
  status: OrderStatus;
  totalAmount: number;
  createdAt: string;
  orderItems: OrderItem[];
}

export interface KitchenOrder {
  id: string;
  orderId: string;
  status: KitchenOrderStatus;
  receivedAt: string;
  startedAt?: string;
  completedAt?: string;
  notes?: string;
  orderItems: KitchenOrderItem[];
}

export interface KitchenOrderItem {
  id: string;
  menuItemId: string;
  menuItemName: string;
  quantity: number;
  specialInstructions?: string;
}

export enum OrderStatus {
  Pending = 'Pending',
  Confirmed = 'Confirmed',
  Preparing = 'Preparing',
  Ready = 'Ready',
  Delivered = 'Delivered',
  Cancelled = 'Cancelled'
}

export enum KitchenOrderStatus {
  Received = 'Received',
  Preparing = 'Preparing',
  Ready = 'Ready',
  Completed = 'Completed'
}