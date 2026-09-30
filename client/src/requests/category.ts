import { useApiRequests } from './api'
import type { Category } from '@/models/category'

export const useCategoryRequests = () => {
  const apiRequests = useApiRequests('categories')
  return {
    createCategory: (cat: {
      name: string
      icon: string
      filters: { weight: { from?: number; to?: number } }
    }) => {
      return apiRequests.post<{ id: number }>('/', cat)
    },
    getCategories: () => {
      return apiRequests.get<Category[]>('/')
    },
    deleteCategory: (id: number) => {
      return apiRequests.delete(`/${id}`)
    },
    patchCategory: (
      id: number,
      cat: {
        name?: string
        icon?: string
        filters?: { weight?: { from?: number; to?: number } }
      },
    ) => {
      return apiRequests.patch(`/${id}`, cat)
    },
  }
}
