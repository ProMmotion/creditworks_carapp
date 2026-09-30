import { defineStore } from 'pinia'
import { useRequestWrapper } from '@/scripts/request'
import { useCategoryRequests } from '@/requests/category'
import type { Category } from '@/models/category'
import { computed, ref } from 'vue'

export const useCategoryStore = defineStore('categories', () => {
  const categoryRequests = useCategoryRequests()

  const created = ref<Category[]>([])
  const [categories, loadCategories, isLoading] = useRequestWrapper(
    categoryRequests.getCategories,
    {
      preventReload: (data) => Boolean(data),
    },
  )

  return {
    categories: computed(() => [...(categories.value ?? []), ...created.value]),
    loadCategories,
    isLoading,
    addCreatedCategory: (category: Category) => created.value.push(category),
  }
})
