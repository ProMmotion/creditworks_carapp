import type { Ownership } from '@/models/ownership'
import { useApiRequests } from './api'

export const useOwnershipRequests = () => {
  const apiRequests = useApiRequests('ownerships')
  return {
    createOwnership: (ownership: Omit<Ownership, 'id'>) => {
      return apiRequests.post<{ id: number }>('/', ownership)
    },
    getOwnerships: (ids: number[]) => {
      return apiRequests.get<Ownership[]>(`/`, { carIds: ids })
    },
  }
}
