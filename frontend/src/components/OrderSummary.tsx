import { OrderItem } from '@/types';

interface OrderSummaryProps {
  items: OrderItem[];
  onRemoveItem: (menuItemId: string) => void;
  onUpdateQuantity: (menuItemId: string, quantity: number) => void;
  onPlaceOrder: () => void;
  isPlacingOrder: boolean;
}

export function OrderSummary({
  items,
  onRemoveItem,
  onUpdateQuantity,
  onPlaceOrder,
  isPlacingOrder
}: OrderSummaryProps) {
  const totalAmount = items.reduce((sum, item) => sum + item.totalPrice, 0);

  if (items.length === 0) {
    return (
      <div className="bg-gray-50 rounded-lg p-6 text-center">
        <p className="text-gray-500">Your order is empty</p>
        <p className="text-sm text-gray-400 mt-1">Add some items from the menu</p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h2 className="text-xl font-semibold mb-4">Your Order</h2>

      <div className="space-y-3 mb-4">
        {items.map((item) => (
          <div key={item.menuItemId} className="flex justify-between items-center border-b pb-2">
            <div className="flex-1">
              <h4 className="font-medium">{item.menuItemName}</h4>
              <p className="text-sm text-gray-600">${item.unitPrice.toFixed(2)} each</p>
            </div>

            <div className="flex items-center space-x-2">
              <button
                onClick={() => onUpdateQuantity(item.menuItemId, item.quantity - 1)}
                className="w-8 h-8 rounded-full bg-gray-200 hover:bg-gray-300 flex items-center justify-center"
              >
                -
              </button>
              <span className="w-8 text-center">{item.quantity}</span>
              <button
                onClick={() => onUpdateQuantity(item.menuItemId, item.quantity + 1)}
                className="w-8 h-8 rounded-full bg-gray-200 hover:bg-gray-300 flex items-center justify-center"
              >
                +
              </button>
              <button
                onClick={() => onRemoveItem(item.menuItemId)}
                className="ml-2 text-red-600 hover:text-red-800"
              >
                ✕
              </button>
            </div>
          </div>
        ))}
      </div>

      <div className="border-t pt-4">
        <div className="flex justify-between items-center mb-4">
          <span className="text-lg font-semibold">Total:</span>
          <span className="text-xl font-bold text-blue-600">
            ${totalAmount.toFixed(2)}
          </span>
        </div>

        <button
          onClick={onPlaceOrder}
          disabled={isPlacingOrder}
          className="w-full bg-green-600 text-white py-3 px-4 rounded-md font-medium hover:bg-green-700 disabled:bg-gray-400 transition-colors"
        >
          {isPlacingOrder ? 'Placing Order...' : 'Place Order'}
        </button>
      </div>
    </div>
  );
}