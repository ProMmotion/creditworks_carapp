<script setup lang="ts">
import { ref, shallowRef } from 'vue'
import ActionButton from './ActionButton.vue'
import AppInput from './AppInput.vue'
import type { Category } from '@/models/category'
import IconSelect from './IconSelect.vue'
import RangeInput from './RangeInput.vue'
import { useCategoryStore } from '@/stores/categories'
import { useCategoryRequests } from '@/requests/category'

const {
  category: baseCategory = {
    id: 0,
    icon: '',
    name: '',
    filters: {
      weight: { from: 0, to: 0 },
    },
  },
} = defineProps<{
  category?: Category
}>()

const emit = defineEmits<{
  submitted: []
}>()

const category = ref<Category>(baseCategory)

const categoryStore = useCategoryStore()
const categoryRequests = useCategoryRequests()

const loading = shallowRef(false)

async function submit() {
  if (category.value.id === 0) {
    loading.value = true
    const { id } = await categoryRequests.createCategory(category.value)
    loading.value = false
    categoryStore.addCreatedCategory({ ...category.value, id })
    emit('submitted')
  } else {
    loading.value = true
    await categoryRequests.patchCategory(category.value.id, category.value)
    loading.value = false
  }
  emit('submitted')
}
</script>

<template>
  <form class="car-form" @submit.prevent="submit">
    <AppInput v-model="category.name" label="Name" required />
    <IconSelect v-model="category.icon" label="Icon" required />
    <RangeInput
      v-if="category.filters.weight"
      v-model="category.filters.weight"
      id="weightRange"
      label="Weight Range"
      unit="kg"
      :min="0"
      :max="4000"
      required
    />
    <ActionButton type="submit" level="primary" :disabled="loading">
      {{ category.id === 0 ? 'Create' : 'Update' }}
    </ActionButton>
  </form>
</template>

<style scoped lang="scss">
.car-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>
