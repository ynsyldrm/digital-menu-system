import axios from 'axios';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || 'http://localhost:5001';

export const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Menu Service API
export const menuApi = {
  getMenuItems: (categoryId?: string, isAvailable?: boolean) =>
    api.get('/api/menu-items', {
      params: { categoryId, isAvailable }
    }),

  createMenuItem: (data: {
    name: string;
    description: string;
    price: number;
    categoryId: string;
  }) => api.post('/api/menu-items', data),
};

// Order Service API
export const orderApi = {
  createOrder: (data: {
    customerId: string;
    orderItems: Array<{
      menuItemId: string;
      menuItemName: string;
      quantity: number;
      unitPrice: number;
    }>;
  }) => api.post('/api/orders', data, { baseURL: 'http://localhost:5002' }),

  getOrder: (orderId: string) =>
    api.get(`/api/orders/${orderId}`, { baseURL: 'http://localhost:5002' }),

  updateOrderStatus: (orderId: string, status: string, notes?: string) =>
    api.put(`/api/orders/${orderId}/status`, { status, notes }, { baseURL: 'http://localhost:5002' }),
};

// Kitchen Service API
export const kitchenApi = {
  getKitchenOrders: (status?: string) =>
    api.get('/api/kitchen-orders', {
      params: { status },
      baseURL: 'http://localhost:5003'
    }),

  updateOrderStatus: (orderId: string, status: string, notes?: string) =>
    api.put(`/api/kitchen-orders/${orderId}/status`, { status, notes }, {
      baseURL: 'http://localhost:5003'
    }),
};