<script setup lang="ts">
import { useBrandStore } from '@/stores/brands'
import { computed, onBeforeMount } from 'vue'
import AppSelect from './AppSelect.vue'
import { useBrandRequests } from '@/requests/brands'
import type { Brand } from '@/models/brand'

defineProps<{
  required?: boolean
  disabled?: boolean
}>()

const brandId = defineModel<number>('brandId')

const brandStore = useBrandStore()
const brandRequests = useBrandRequests()

const brand = computed(() => brandStore.brands?.find((b) => b.id === brandId.value))

async function createBrand(name: string) {
  const { id } = await brandRequests.createBrand({ name })
  brandStore.addCreatedBrand({ id, name })
  brandId.value = id
}

onBeforeMount(brandStore.loadBrands)
</script>

<template>
  <AppSelect
    :modelValue="brand"
    id="brandSelect"
    :required
    :disabled
    :getLabel="(brand) => brand.name"
    :getValue="(brand) => brand.id"
    :options="brandStore.brands ?? []"
    @createOption="createBrand"
    @update:modelValue="brandId = $event?.id"
    allowCreate
  />
</template>
