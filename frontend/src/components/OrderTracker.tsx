import { Order, OrderStatus } from '@/types';

interface OrderTrackerProps {
  order: Order;
}

const statusSteps = [
  { status: OrderStatus.Pending, label: 'Order Placed', color: 'bg-gray-400' },
  { status: OrderStatus.Confirmed, label: 'Confirmed', color: 'bg-blue-500' },
  { status: OrderStatus.Preparing, label: 'Preparing', color: 'bg-yellow-500' },
  { status: OrderStatus.Ready, label: 'Ready for Pickup', color: 'bg-orange-500' },
  { status: OrderStatus.Delivered, label: 'Delivered', color: 'bg-green-500' },
];

export function OrderTracker({ order }: OrderTrackerProps) {
  const currentStepIndex = statusSteps.findIndex(step => step.status === order.status);

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h2 className="text-xl font-semibold mb-4">Order Status</h2>
      <p className="text-sm text-gray-600 mb-6">Order #{order.id.slice(-8)}</p>

      <div className="relative">
        {/* Progress Line */}
        <div className="absolute top-5 left-0 right-0 h-0.5 bg-gray-200">
          <div
            className="h-full bg-blue-500 transition-all duration-500"
            style={{ width: `${(currentStepIndex / (statusSteps.length - 1)) * 100}%` }}
          />
        </div>

        {/* Status Steps */}
        <div className="relative flex justify-between">
          {statusSteps.map((step, index) => {
            const isCompleted = index <= currentStepIndex;
            const isCurrent = index === currentStepIndex;

            return (
              <div key={step.status} className="flex flex-col items-center">
                <div
                  className={`w-10 h-10 rounded-full flex items-center justify-center text-white font-bold transition-colors ${
                    isCompleted ? step.color : 'bg-gray-300'
                  } ${isCurrent ? 'ring-4 ring-blue-200' : ''}`}
                >
                  {index + 1}
                </div>
                <span className={`mt-2 text-xs text-center ${
                  isCompleted ? 'text-gray-900 font-medium' : 'text-gray-500'
                }`}>
                  {step.label}
                </span>
              </div>
            );
          })}
        </div>
      </div>

      <div className="mt-6 p-4 bg-gray-50 rounded-lg">
        <div className="grid grid-cols-2 gap-4 text-sm">
          <div>
            <span className="font-medium text-gray-700">Order Time:</span>
            <p className="text-gray-600">{new Date(order.createdAt).toLocaleString()}</p>
          </div>
          <div>
            <span className="font-medium text-gray-700">Total Amount:</span>
            <p className="text-gray-600">${order.totalAmount.toFixed(2)}</p>
          </div>
        </div>
      </div>

      {order.status === OrderStatus.Delivered && (
        <div className="mt-4 p-4 bg-green-50 border border-green-200 rounded-lg">
          <p className="text-green-800 font-medium">🎉 Your order has been delivered!</p>
          <p className="text-green-600 text-sm mt-1">Thank you for choosing our restaurant!</p>
        </div>
      )}
    </div>
  );
}