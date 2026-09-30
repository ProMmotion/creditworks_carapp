<script setup lang="ts">
import { ref, shallowRef } from 'vue'
import ActionButton from './ActionButton.vue'
import BrandSelect from './BrandSelect.vue'
import ModelSelect from './ModelSelect.vue'
import { useCarRequests } from '@/requests/cars'
import AppInput from './AppInput.vue'
import type { Car } from '@/models/car'
import OwnerSelect from './OwnerSelect.vue'
import { useOwnershipRequests } from '@/requests/ownership'
import { useCarStore } from '@/stores/cars'

const {
  car: baseCar = {
    id: 0,
    brandId: 0,
    modelId: 0,
    year: 0,
    weight: 0,
  },
  ownerId: baseOwnerId = 0,
} = defineProps<{
  car?: Car
  ownerId?: number
}>()

const emit = defineEmits<{
  submitted: []
}>()

const car = ref<Car>(baseCar)
const ownerId = ref<number>(baseOwnerId)

const carStore = useCarStore()
const carRequests = useCarRequests()
const ownershipRequests = useOwnershipRequests()

const loading = shallowRef(false)

async function submit() {
  if (car.value.id === 0) {
    loading.value = true
    const { id } = await carRequests.createCar(car.value)
    await ownershipRequests.createOwnership({ carId: id, ownerId: ownerId.value })
    loading.value = false
    carStore.addCreatedCar({ ...car.value, id })
    emit('submitted')
  } else return
}
</script>

<template>
  <form class="car-form" @submit.prevent="submit">
    <BrandSelect v-model="car.brandId" label="Brand" required />
    <ModelSelect v-model="car.modelId" :brandId="car.brandId" label="Model" required />
    <OwnerSelect v-model="ownerId" label="Owner" required />
    <AppInput v-model="car.year" label="Year" />
    <AppInput v-model="car.weight" label="Weight" />
    <ActionButton type="submit" level="primary" :disabled="loading">Create</ActionButton>
  </form>
</template>

<style scoped lang="scss">
.car-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>
