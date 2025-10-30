import { MenuItem } from '@/types';

interface MenuCardProps {
  item: MenuItem;
  onAddToOrder: (item: MenuItem) => void;
}

export function MenuCard({ item, onAddToOrder }: MenuCardProps) {
  return (
    <div className="bg-white rounded-lg shadow-md p-6 hover:shadow-lg transition-shadow">
      <div className="flex justify-between items-start mb-2">
        <h3 className="text-lg font-semibold text-gray-900">{item.name}</h3>
        <span className={`px-2 py-1 text-xs rounded-full ${
          item.isAvailable
            ? 'bg-green-100 text-green-800'
            : 'bg-red-100 text-red-800'
        }`}>
          {item.isAvailable ? 'Available' : 'Unavailable'}
        </span>
      </div>

      <p className="text-gray-600 text-sm mb-3">{item.description}</p>

      <div className="flex justify-between items-center">
        <span className="text-xl font-bold text-blue-600">
          ${item.price.toFixed(2)}
        </span>
        <button
          onClick={() => onAddToOrder(item)}
          disabled={!item.isAvailable}
          className={`px-4 py-2 rounded-md font-medium transition-colors ${
            item.isAvailable
              ? 'bg-blue-600 text-white hover:bg-blue-700 disabled:bg-gray-400'
              : 'bg-gray-300 text-gray-500 cursor-not-allowed'
          }`}
        >
          Add to Order
        </button>
      </div>

      <div className="mt-2 text-xs text-gray-500">
        Category: {item.categoryName}
      </div>
    </div>
  );
}