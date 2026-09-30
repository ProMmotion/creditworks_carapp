import { useApiRequests } from './api'
import type { Model } from '@/models/model'

export const useModelRequests = () => {
  const apiRequests = useApiRequests('models')
  return {
    createModel: (model: { name: string; brandId: number }) => {
      return apiRequests.post<{ id: number }>('/', model)
    },
    getModels: (brandId: number) => {
      return apiRequests.get<Model[]>(`/${brandId}`)
    },
  }
}
