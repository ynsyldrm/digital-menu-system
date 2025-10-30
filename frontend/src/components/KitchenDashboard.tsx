import { KitchenOrder, KitchenOrderStatus } from '@/types';

interface KitchenDashboardProps {
  orders: KitchenOrder[];
  onUpdateStatus: (orderId: string, status: KitchenOrderStatus, notes?: string) => void;
  isUpdating: boolean;
}

const statusColors = {
  [KitchenOrderStatus.Received]: 'bg-gray-100 text-gray-800',
  [KitchenOrderStatus.Preparing]: 'bg-yellow-100 text-yellow-800',
  [KitchenOrderStatus.Ready]: 'bg-orange-100 text-orange-800',
  [KitchenOrderStatus.Completed]: 'bg-green-100 text-green-800',
};

const nextStatus = {
  [KitchenOrderStatus.Received]: KitchenOrderStatus.Preparing,
  [KitchenOrderStatus.Preparing]: KitchenOrderStatus.Ready,
  [KitchenOrderStatus.Ready]: KitchenOrderStatus.Completed,
  [KitchenOrderStatus.Completed]: KitchenOrderStatus.Completed,
};

export function KitchenDashboard({ orders, onUpdateStatus, isUpdating }: KitchenDashboardProps) {
  const pendingOrders = orders.filter(o => o.status !== KitchenOrderStatus.Completed);
  const completedOrders = orders.filter(o => o.status === KitchenOrderStatus.Completed);

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold text-gray-900">Kitchen Dashboard</h1>
        <div className="text-sm text-gray-600">
          Active Orders: {pendingOrders.length}
        </div>
      </div>

      {/* Active Orders */}
      <div className="grid gap-4">
        <h2 className="text-lg font-semibold text-gray-800">Active Orders</h2>
        {pendingOrders.length === 0 ? (
          <div className="bg-gray-50 rounded-lg p-8 text-center">
            <p className="text-gray-500">No active orders</p>
          </div>
        ) : (
          pendingOrders.map((order) => (
            <OrderCard
              key={order.id}
              order={order}
              onUpdateStatus={onUpdateStatus}
              isUpdating={isUpdating}
            />
          ))
        )}
      </div>

      {/* Completed Orders */}
      {completedOrders.length > 0 && (
        <div className="grid gap-4">
          <h2 className="text-lg font-semibold text-gray-800">Completed Orders</h2>
          {completedOrders.slice(0, 5).map((order) => (
            <OrderCard
              key={order.id}
              order={order}
              onUpdateStatus={onUpdateStatus}
              isUpdating={isUpdating}
              isCompleted
            />
          ))}
        </div>
      )}
    </div>
  );
}

interface OrderCardProps {
  order: KitchenOrder;
  onUpdateStatus: (orderId: string, status: KitchenOrderStatus, notes?: string) => void;
  isUpdating: boolean;
  isCompleted?: boolean;
}

function OrderCard({ order, onUpdateStatus, isUpdating, isCompleted }: OrderCardProps) {
  const canUpdateStatus = !isCompleted && order.status !== KitchenOrderStatus.Completed;

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <div className="flex justify-between items-start mb-4">
        <div>
          <h3 className="text-lg font-semibold">Order #{order.orderId.slice(-8)}</h3>
          <p className="text-sm text-gray-600">
            Received: {new Date(order.receivedAt).toLocaleString()}
          </p>
          {order.startedAt && (
            <p className="text-sm text-gray-600">
              Started: {new Date(order.startedAt).toLocaleString()}
            </p>
          )}
        </div>
        <span className={`px-3 py-1 rounded-full text-sm font-medium ${statusColors[order.status]}`}>
          {order.status}
        </span>
      </div>

      <div className="space-y-2 mb-4">
        {order.orderItems.map((item) => (
          <div key={item.id} className="flex justify-between items-center">
            <span className="font-medium">{item.menuItemName}</span>
            <span className="text-gray-600">x{item.quantity}</span>
          </div>
        ))}
      </div>

      {order.notes && (
        <div className="mb-4 p-3 bg-yellow-50 border border-yellow-200 rounded">
          <p className="text-sm text-yellow-800">{order.notes}</p>
        </div>
      )}

      {canUpdateStatus && (
        <button
          onClick={() => onUpdateStatus(order.orderId, nextStatus[order.status])}
          disabled={isUpdating}
          className="w-full bg-blue-600 text-white py-2 px-4 rounded-md font-medium hover:bg-blue-700 disabled:bg-gray-400 transition-colors"
        >
          {isUpdating ? 'Updating...' : `Mark as ${nextStatus[order.status]}`}
        </button>
      )}
    </div>
  );
}