import { defineStore } from 'pinia'
import { useRequestWrapper } from '@/scripts/request'
import { useModelRequests } from '@/requests/models'
import type { Model } from '@/models/model'
import { computed, ref } from 'vue'
import { isUnique } from '@/scripts/array'

export const useModelStore = defineStore('models', () => {
  const modelRequests = useModelRequests()

  const created = ref<Model[]>([])
  const [models, loadModels, isLoading] = useRequestWrapper(modelRequests.getModels, {
    preventReload: (data: Model[] | undefined, brandId) => {
      return data?.some((x) => x.brandId === brandId) ?? false
    },
    merge: (newValue, oldValue) => [...(oldValue ?? []), ...newValue],
  })

  const modelsByBrands = computed(() =>
    [...(models.value ?? []), ...created.value].reduce<Record<number, Model[]>>((acc, model) => {
      if (!acc[model.brandId]) acc[model.brandId] = []
      acc[model.brandId]?.push(model)
      acc[model.brandId]?.filter(isUnique)
      return acc
    }, {}),
  )

  return {
    loadModels,
    modelsByBrands,
    isLoading,
    addCreatedModel: (model: Model) => created.value.push(model),
  }
})
