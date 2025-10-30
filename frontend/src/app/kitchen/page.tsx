'use client';

import { useKitchenOrders, useUpdateKitchenOrderStatus } from '@/hooks/useKitchen';
import { KitchenDashboard } from '@/components/KitchenDashboard';
import { KitchenOrderStatus } from '@/types';

export default function KitchenPage() {
  const { data: orders, isLoading } = useKitchenOrders();
  const updateStatusMutation = useUpdateKitchenOrderStatus();

  const handleUpdateStatus = async (orderId: string, status: KitchenOrderStatus, notes?: string) => {
    try {
      await updateStatusMutation.mutateAsync({ orderId, status, notes });
    } catch (error) {
      console.error('Failed to update order status:', error);
      alert('Failed to update order status. Please try again.');
    }
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Loading kitchen orders...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-6xl mx-auto px-4 py-8">
        <KitchenDashboard
          orders={orders || []}
          onUpdateStatus={handleUpdateStatus}
          isUpdating={updateStatusMutation.isPending}
        />
      </div>
    </div>
  );
}