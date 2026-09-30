import { defineStore } from 'pinia'
import { useRequestWrapper } from '@/scripts/request'
import { useOwnershipRequests } from '@/requests/ownership'

export const useOwnershipStore = defineStore('ownerships', () => {
  const ownershipRequests = useOwnershipRequests()
  const [ownerships, loadOwnerships, isLoading] = useRequestWrapper(ownershipRequests.getOwnerships)

  return { ownerships, loadOwnerships, isLoading }
})
