import { defineStore } from 'pinia'
import { useCarRequests } from '@/requests/cars'
import { useRequestWrapper } from '@/scripts/request'
import type { Car } from '@/models/car'
import { computed, ref } from 'vue'

export const useCarStore = defineStore('cars', () => {
  const carRequests = useCarRequests()

  const created = ref<Car[]>([])
  const [cars, loadCars, isLoading] = useRequestWrapper(carRequests.getCars)

  return {
    cars: computed(() => [...(cars.value?.items ?? []), ...created.value]),
    // not really but will see later
    totalCars: computed(() => (cars.value?.total ?? 0) + created.value.length),
    loadCars,
    isLoading,
    addCreatedCar: (car: Car) => {
      created.value.push(car)
    },
  }
})
