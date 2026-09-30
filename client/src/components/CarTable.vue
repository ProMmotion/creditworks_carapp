<script setup lang="ts">
import { useCarStore } from '@/stores/cars'
import ActionButton from './ActionButton.vue'
import CarForm from './CarForm.vue'
import ListTable, { type TableColumn } from './ListTable.vue'
import PaginationActions from './PaginationActions.vue'
import ViewModal from './ViewModal.vue'
import { computed, onBeforeMount, shallowRef, watch } from 'vue'
import { getProperty } from '@/scripts/property'
import { useBrandStore } from '@/stores/brands'
import { useCategoryStore } from '@/stores/categories'
import { useModelStore } from '@/stores/models'
import { isUnique } from '@/scripts/array'
import { isInRange } from '@/utils/Range'
import AppBadge from './AppBadge.vue'
import { useOwnershipStore } from '@/stores/ownerships'
import { useOwnerStore } from '@/stores/owners'
import CategoryForm from './CategoryForm.vue'
import CategoryManagement from './CategoryManagement.vue'

const categoryStore = useCategoryStore()
const carStore = useCarStore()
const brandStore = useBrandStore()
const ownerStore = useOwnerStore()
const modelStore = useModelStore()
const ownershipStore = useOwnershipStore()

const tableColumns = computed<TableColumn[]>(() => [
  { field: 'owner', label: 'Owner' },
  { field: 'brand', label: 'Brand', sortBy: 'brand' },
  { field: 'model', label: 'Model' },
  { field: 'year', label: 'Year', sortBy: 'year' },
  { field: 'weight', label: 'Weight (kg)', sortBy: 'weight' },
  { field: 'category', label: 'category' },
])

const page = shallowRef(1)
const pageSize = shallowRef<10 | 25 | 50>(10)

const cars = computed(
  () =>
    carStore.cars.map((car) => ({
      ...car,
      weight: car.weight.toFixed(2),
      owner: ownerStore.owners?.find(
        (o) => o.id === ownershipStore.ownerships?.find((os) => os.carId === car.id)?.ownerId,
      )?.name,
      brand: brandStore.brands?.find((b) => b.id === car.brandId)?.name,
      model: modelStore.modelsByBrands?.[car.brandId]?.find((m) => m.id === car.modelId)?.name,
      categories: categoryStore.categories?.filter(({ filters }) =>
        isInRange(car.weight, {
          from: filters.weight?.from ?? 0,
          to: filters.weight?.to ?? Infinity,
        }),
      ),
    })) ?? [],
)

async function loadPage(p: number) {
  page.value = p
  await carStore.loadCars({ page: p, pageSize: pageSize.value })
  await ownershipStore.loadOwnerships(carStore.cars.map((x) => x.id))
  const brands = carStore.cars.map((x) => x.brandId).filter(isUnique) ?? []
  Promise.all(brands.map((b) => modelStore.loadModels(b)))
}

onBeforeMount(async () => {
  categoryStore.loadCategories()
  brandStore.loadBrands()
  ownerStore.loadOwners()
  loadPage(1)
})
watch(pageSize, () => {
  loadPage(1)
})
watch(page, (newPage) => {
  loadPage(newPage)
})
</script>

<template>
  <div class="table">
    <div class="table-header">
      <h3>Car inventory</h3>

      <div class="actions">
        <ViewModal>
          <template #opener="{ openModal }">
            <ActionButton type="button" level="primary" @click.stop="openModal">
              Add a car
            </ActionButton>
          </template>

          <template #content="{ closeModal }"> <CarForm @submitted="closeModal" /> </template>
        </ViewModal>

        <ViewModal>
          <template #opener="{ openModal }">
            <ActionButton type="button" level="secondary" @click.stop="openModal">
              Category management
            </ActionButton>
          </template>
          <template #content>
            <CategoryManagement :categories="categoryStore.categories" />
          </template>
        </ViewModal>
      </div>
    </div>

    <ListTable
      class="car-table"
      :columns="tableColumns"
      :items="cars"
      :loading="carStore.isLoading"
    >
      <template #cell="{ item, column }">
        <template v-if="column.field === 'category'">
          <AppBadge v-for="category in item.categories" :key="category.id" level="info">
            <i :class="`icon-${category.icon}`" />
            {{ category.name }}
          </AppBadge>
        </template>
        <span v-else>{{ getProperty(item, column.field) }}</span>
      </template>
    </ListTable>

    <div class="table-footer">
      <PaginationActions
        v-model:page="page"
        v-model:pageSize="pageSize"
        :total="carStore.totalCars ?? 0"
      />
    </div>
  </div>
</template>

<style scoped lang="scss">
.table {
  width: 100%;
  max-height: 100%;
  display: flex;
  flex-direction: column;
  gap: 12px;
  .table-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }
}
.actions {
  display: inline-flex;
  gap: 8px;
}
</style>
