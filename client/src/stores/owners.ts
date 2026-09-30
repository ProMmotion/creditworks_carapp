import { defineStore } from 'pinia'
import { useRequestWrapper } from '@/scripts/request'
import { useOwnerRequests } from '@/requests/owners'
import type { Owner } from '@/models/owner'
import { computed, ref } from 'vue'

export const useOwnerStore = defineStore('owners', () => {
  const ownerRequests = useOwnerRequests()

  const created = ref<Owner[]>([])
  const [owners, loadOwners, isLoading] = useRequestWrapper(ownerRequests.getOwners, {
    preventReload: (data) => Boolean(data),
  })

  return {
    owners: computed(() => [...(owners.value ?? []), ...created.value]),
    isLoading,
    loadOwners,
    addCreatedOwner: (owner: Owner) => {
      created.value.push(owner)
    },
  }
})
