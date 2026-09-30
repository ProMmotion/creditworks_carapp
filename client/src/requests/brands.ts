import { useApiRequests } from './api'
import type { Brand } from '@/models/brand'

export const useBrandRequests = () => {
  const apiRequests = useApiRequests('brands')
  return {
    createBrand: (brand: { name: string; imgUrl?: string }) => {
      return apiRequests.post<{ id: number }>('/', brand)
    },
    getBrands: () => {
      return apiRequests.get<Brand[]>(`/`)
    },
  }
}
