<script setup lang="ts">
import { computed, watch } from 'vue'
import AppSelect from './AppSelect.vue'
import { useModelStore } from '@/stores/models'
import { useModelRequests } from '@/requests/models'

const { brandId } = defineProps<{ brandId: number; required?: boolean; disabled?: boolean }>()

const modelId = defineModel<number>()

const modelStore = useModelStore()
const modelRequests = useModelRequests()

const model = computed(() =>
  modelStore.modelsByBrands?.[brandId]?.find((m) => m.id === modelId.value),
)

async function createModel(name: string) {
  if (brandId === 0) return
  const { id } = await modelRequests.createModel({ name, brandId })
  modelStore.addCreatedModel({ id, brandId, name })
  modelId.value = id
}

watch(
  () => brandId,
  () => {
    if (brandId === 0) return
    modelStore.loadModels(brandId)
  },
  { immediate: true },
)
</script>

<template>
  <AppSelect
    :modelValue="model"
    id="modelSelect"
    :required
    :getLabel="(model) => model.name"
    :getValue="(model) => model.id"
    :disabled="disabled || brandId === 0"
    :options="modelStore.modelsByBrands?.[brandId] ?? []"
    :allowCreate="brandId !== 0"
    @createOption="createModel"
    @update:modelValue="modelId = $event?.id"
  />
</template>
