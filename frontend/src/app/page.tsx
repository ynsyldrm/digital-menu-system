'use client';

import { useState } from 'react';
import { useMenuItems } from '@/hooks/useMenu';
import { useCreateOrder } from '@/hooks/useOrders';
import { MenuCard } from '@/components/MenuCard';
import { OrderSummary } from '@/components/OrderSummary';
import { AddMenuItemForm } from '@/components/AddMenuItemForm';
import { MenuItem, OrderItem } from '@/types';

export default function HomePage() {
  const [orderItems, setOrderItems] = useState<OrderItem[]>([]);
  const [customerId] = useState(() => `customer-${Date.now()}`); // Simple customer ID for demo

  const { data: menuItems, isLoading: menuLoading } = useMenuItems();
  const createOrderMutation = useCreateOrder();

  const addToOrder = (item: MenuItem) => {
    const existingItem = orderItems.find(oi => oi.menuItemId === item.id);

    if (existingItem) {
      updateQuantity(item.id, existingItem.quantity + 1);
    } else {
      const newOrderItem: OrderItem = {
        menuItemId: item.id,
        menuItemName: item.name,
        quantity: 1,
        unitPrice: item.price,
        totalPrice: item.price
      };
      setOrderItems([...orderItems, newOrderItem]);
    }
  };

  const removeFromOrder = (menuItemId: string) => {
    setOrderItems(orderItems.filter(item => item.menuItemId !== menuItemId));
  };

  const updateQuantity = (menuItemId: string, quantity: number) => {
    if (quantity <= 0) {
      removeFromOrder(menuItemId);
      return;
    }

    setOrderItems(orderItems.map(item =>
      item.menuItemId === menuItemId
        ? { ...item, quantity, totalPrice: item.unitPrice * quantity }
        : item
    ));
  };

  const placeOrder = async () => {
    if (orderItems.length === 0) return;

    try {
      await createOrderMutation.mutateAsync({
        customerId,
        orderItems
      });

      // Clear the order after successful placement
      setOrderItems([]);
      alert('Order placed successfully!');
    } catch (error) {
      console.error('Failed to place order:', error);
      alert('Failed to place order. Please try again.');
    }
  };

  if (menuLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Loading menu...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-7xl mx-auto px-4 py-8">
        <header className="text-center mb-8 bg-yellow-100">
          <h1 className="text-4xl font-bold text-gray-900 mb-2">
            🍽️ Digital Menu & Order System
          </h1>
          <p className="text-lg text-gray-600">
            Welcome to our restaurant! Browse our menu and place your order.
          </p>
        </header>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Menu Section */}
          <div className="lg:col-span-2">
            <div className="flex justify-between items-center mb-6">
              <h2 className="text-2xl font-semibold text-gray-900">Our Menu</h2>
              <AddMenuItemForm />
            </div>

            {menuItems && menuItems.length > 0 ? (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {menuItems.map((item: MenuItem) => (
                  <MenuCard
                    key={item.id}
                    item={item}
                    onAddToOrder={addToOrder}
                  />
                ))}
              </div>
            ) : (
              <div className="text-center py-12">
                <p className="text-gray-500">No menu items available at the moment.</p>
              </div>
            )}
          </div>

          {/* Order Summary Section */}
          <div className="lg:col-span-1">
            <OrderSummary
              items={orderItems}
              onRemoveItem={removeFromOrder}
              onUpdateQuantity={updateQuantity}
              onPlaceOrder={placeOrder}
              isPlacingOrder={createOrderMutation.isPending}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
