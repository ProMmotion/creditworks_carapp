import { useApiRequests } from './api'
import type { Owner } from '@/models/owner'

export const useOwnerRequests = () => {
  const apiRequests = useApiRequests('owners')
  return {
    createOwner: (owner: { name: string }) => {
      return apiRequests.post<{ id: number }>('/', owner)
    },
    getOwners: () => {
      return apiRequests.get<Owner[]>(`/`)
    },
  }
}
