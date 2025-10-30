'use client';

import { useParams } from 'next/navigation';
import { useOrder } from '@/hooks/useOrders';
import { OrderTracker } from '@/components/OrderTracker';

export default function OrderTrackingPage() {
  const params = useParams();
  const orderId = params.orderId as string;

  const { data: order, isLoading, error } = useOrder(orderId);

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
          <p className="mt-4 text-gray-600">Loading order details...</p>
        </div>
      </div>
    );
  }

  if (error || !order) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="text-center">
          <div className="text-red-600 text-xl mb-4">❌</div>
          <h2 className="text-xl font-semibold text-gray-900 mb-2">Order Not Found</h2>
          <p className="text-gray-600">
            We couldn't find the order you're looking for. Please check the order ID and try again.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <div className="max-w-4xl mx-auto px-4 py-8">
        <header className="text-center mb-8">
          <h1 className="text-3xl font-bold text-gray-900 mb-2">
            Order Tracking
          </h1>
          <p className="text-lg text-gray-600">
            Track your order in real-time
          </p>
        </header>

        <div className="max-w-2xl mx-auto">
          <OrderTracker order={order} />
        </div>
      </div>
    </div>
  );
}