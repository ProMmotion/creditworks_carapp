import type { Car } from '@/models/car'
import { useApiRequests } from './api'
import type { Range } from '@/utils/Range'
import type { Paginated } from '@/utils/Paginated'

export type GetCarsQuery = {
  filters?: {
    brandId?: number
    modelId?: number
    weight?: Range
    year?: Range
  }
  page: number
  pageSize?: number
  sortBy?: string
  sortOrder?: 'Asc' | 'Desc'
}

export const useCarRequests = () => {
  const apiRequests = useApiRequests('cars')
  return {
    createCar: (car: Omit<Car, 'id'>) => {
      return apiRequests.post<{ id: number }>('/', car)
    },
    updateCar: (car: Partial<Car>) => {
      return apiRequests.patch<{ id: number }>('/', car)
    },
    getCars: (query: GetCarsQuery) => {
      return apiRequests.get<Paginated<Car>>(`/`, query)
    },
  }
}
