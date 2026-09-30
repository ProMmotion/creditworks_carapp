<script setup lang="ts">
import { ref, shallowRef } from 'vue'
import type { Category } from '@/models/category'
import ActionButton from './ActionButton.vue'
import CategoryForm from './CategoryForm.vue'
import type { Range } from '@/utils/Range'
import { useCategoryRequests } from '@/requests/category'
import AppBadge from './AppBadge.vue'

const { categories } = defineProps<{
  categories: Category[]
}>()

const isFormOpen = shallowRef(false)
const editingCategory = ref<Category>()

const formatRange = (range: Partial<Range>) => {
  const minText = range.from !== null && range.from !== undefined ? `${range.from} kg` : '0 kg'
  const maxText = range.to !== null && range.to !== undefined ? `${range.to} kg` : 'Infinity'

  return `${minText} to ${maxText}`
}

const categoryRequests = useCategoryRequests()

async function deleteCategory(id: number) {
  if (confirm('Are you sure you want to delete this category?')) {
    categoryRequests.deleteCategory(id)
  }
}
</script>

<template>
  <div class="category-manager">
    <div class="header-actions">
      <div>
        <h2 class="title">Category management</h2>
      </div>
      <ActionButton v-if="!isFormOpen" type="button" level="primary">Add category</ActionButton>
      <ActionButton v-else type="button" level="tertiary" @click="isFormOpen = false">
        Cancel
      </ActionButton>
    </div>

    <div v-if="!isFormOpen" class="categories-grid">
      <div v-for="(cat, index) in categories" :key="`${index}-${cat.id}`" class="category-card">
        <div class="card-header">
          <AppBadge :key="cat.id" level="info">
            <i :class="`icon-${cat.icon}`" />
            {{ cat.name }}
          </AppBadge>
          <div class="actions">
            <ActionButton
              type="button"
              level="secondary"
              @click="((editingCategory = cat), (isFormOpen = true))"
            >
              Edit
            </ActionButton>

            <ActionButton type="button" level="danger" @click="deleteCategory(cat.id)">
              Delete
            </ActionButton>
          </div>
        </div>

        <div class="card-body">
          <div v-if="cat.filters?.weight" class="range-display">
            <span class="range-label">Weight range:</span>
            <span class="range-value font-mono">
              {{ formatRange(cat.filters.weight) }}
            </span>
          </div>
        </div>
      </div>
    </div>

    <CategoryForm
      v-if="isFormOpen"
      :category="Object.assign({}, editingCategory)"
      @submitted="((isFormOpen = false), (editingCategory = undefined))"
    />
  </div>
</template>

<style scoped lang="scss">
.category-manager {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.header-actions {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  flex-wrap: wrap;
  gap: 1rem;
}

.title {
  font-size: 1.25rem;
  font-weight: 700;
  color: #0f172a;
  margin: 0;
}

.subtitle {
  font-size: 0.875rem;
  color: #64748b;
  margin-top: 0.25rem;
}

.alert {
  padding: 0.75rem 1rem;
  border-radius: 0.375rem;
  font-size: 0.875rem;
  font-weight: 500;
}

.alert-warning {
  background-color: #fffbeb;
  border: 1px solid #fde68a;
  color: #b45309;
}

.categories-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 1.25rem;
}

.category-card {
  background-color: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 0.5rem;
  padding: 1.25rem;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.05);
  transition:
    transform 0.15s ease,
    box-shadow 0.15s ease;
}

.category-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);
}

.card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 1rem;
  .actions {
    display: flex;
    gap: 0.5rem;
  }
}

.category-icon-large {
  font-size: 2rem;
  background-color: #f1f5f9;
  padding: 0.5rem;
  border-radius: 0.5rem;
}

.category-title {
  font-size: 1.125rem;
  font-weight: 600;
  color: #1e293b;
  margin: 0;
}

.category-badge {
  font-size: 0.75rem;
  color: #64748b;
  background-color: #f8fafc;
  padding: 0.125rem 0.375rem;
  border-radius: 0.25rem;
}

.card-body {
  border-top: 1px solid #f1f5f9;
  padding-top: 0.875rem;
}

.range-label {
  display: block;
  font-size: 0.75rem;
  color: #64748b;
  margin-bottom: 0.25rem;
}

.range-value {
  font-size: 0.875rem;
  font-weight: 600;
  color: #2563eb;
}

.btn-icon {
  background: none;
  border: 1px solid #e2e8f0;
  border-radius: 0.375rem;
  padding: 0.375rem 0.625rem;
  cursor: pointer;
  transition: background-color 0.15s ease;
}

.btn-icon:hover {
  background-color: #f1f5f9;
}

/* Modale */
.modal-backdrop {
  position: fixed;
  top: 0;
  left: 0;
  width: 100vw;
  height: 100vh;
  background-color: rgba(15, 23, 42, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 100;
}

.modal-content {
  background-color: #ffffff;
  border-radius: 0.5rem;
  width: 100%;
  max-width: 500px;
  box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.modal-header {
  padding: 1rem 1.25rem;
  background-color: #f8fafc;
  border-bottom: 1px solid #e2e8f0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.modal-header h3 {
  margin: 0;
  font-size: 1.125rem;
  color: #0f172a;
}

.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  color: #64748b;
  cursor: pointer;
}

.modal-body {
  padding: 1.25rem;
}

/* Sélection d'icônes */
.icon-selector {
  display: flex;
  gap: 0.5rem;
  margin-top: 0.375rem;
}

.icon-option {
  font-size: 1.25rem;
  padding: 0.5rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.375rem;
  background: #ffffff;
  cursor: pointer;
  transition: all 0.15s ease;
}

.icon-option.selected {
  border-color: #2563eb;
  background-color: #eff6ff;
}

.modal-footer {
  display: flex;
  justify-content: flex-end;
  gap: 0.75rem;
  margin-top: 1.5rem;
}

.font-mono {
  font-family: monospace;
}
</style>
