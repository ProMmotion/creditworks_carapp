import { defineStore } from 'pinia'
import { useRequestWrapper } from '@/scripts/request'
import { useBrandRequests } from '@/requests/brands'
import type { Brand } from '@/models/brand'
import { computed, ref } from 'vue'

export const useBrandStore = defineStore('brands', () => {
  const brandRequests = useBrandRequests()

  const created = ref<Brand[]>([])
  const [brands, loadBrands, isLoading] = useRequestWrapper(brandRequests.getBrands, {
    preventReload: (data) => Boolean(data),
  })

  return {
    brands: computed(() => [...(brands.value ?? []), ...created.value]),
    isLoading,
    loadBrands,
    addCreatedBrand: (brand: Brand) => {
      created.value.push(brand)
    },
  }
})
