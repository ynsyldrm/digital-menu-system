import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { kitchenApi } from '@/lib/api';
import { KitchenOrder, KitchenOrderStatus } from '@/types';

export const useKitchenOrders = (status?: KitchenOrderStatus) => {
  return useQuery({
    queryKey: ['kitchenOrders', status],
    queryFn: async () => {
      const response = await kitchenApi.getKitchenOrders(status);
      return response.data as KitchenOrder[];
    },
    refetchInterval: 5000, // Refetch every 5 seconds for real-time updates
  });
};

export const useUpdateKitchenOrderStatus = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: ({ orderId, status, notes }: {
      orderId: string;
      status: KitchenOrderStatus;
      notes?: string;
    }) => kitchenApi.updateOrderStatus(orderId, status, notes),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['kitchenOrders'] });
    },
  });
};