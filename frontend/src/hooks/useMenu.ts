import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { menuApi } from '@/lib/api';
import { MenuItem } from '@/types';

export const useMenuItems = (categoryId?: string, isAvailable?: boolean) => {
  return useQuery({
    queryKey: ['menuItems', categoryId, isAvailable],
    queryFn: async () => {
      const response = await menuApi.getMenuItems(categoryId, isAvailable);
      return response.data as MenuItem[];
    },
  });
};

export const useCreateMenuItem = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: {
      name: string;
      description: string;
      price: number;
      categoryId: string;
    }) => menuApi.createMenuItem(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['menuItems'] });
    },
  });
};