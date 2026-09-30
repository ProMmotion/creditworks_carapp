<script setup lang="ts">
import { computed } from 'vue'

export type PageSize = 10 | 25 | 50

const { total } = defineProps<{
  total: number
}>()

const page = defineModel<number>('page', { required: true })
const pageSize = defineModel<PageSize>('pageSize', { default: 10 })

const totalPages = computed(() => {
  return Math.ceil(total / pageSize.value) || 1
})

const visiblePages = computed(() => {
  const pages = []
  const maxPagesToShow = 5
  let startPage = Math.max(1, page.value - Math.floor(maxPagesToShow / 2))
  let endPage = startPage + maxPagesToShow - 1

  if (endPage > totalPages.value) {
    endPage = totalPages.value
    startPage = Math.max(1, endPage - maxPagesToShow + 1)
  }

  for (let i = startPage; i <= endPage; i++) {
    pages.push(i)
  }
  return pages
})

const goToPage = (p: number) => {
  if (p >= 1 && p <= totalPages.value && p !== page.value) {
    page.value = p
  }
}
</script>

<template>
  <div class="pagination-container">
    <div class="pagination-info">
      <span class="font-medium">{{ total }}</span> Total
    </div>

    <div class="pagination-controls">
      <div class="items-per-page">
        <label for="pageSizeSelect" class="select-label">Par page :</label>
        <select
          id="pageSizeSelect"
          :value="pageSize"
          class="page-size-select"
          @change="pageSize = Number($event.target.value)"
        >
          <option v-for="size in [10, 25, 50]" :key="size" :value="size">
            {{ size }}
          </option>
        </select>
      </div>

      <nav class="pagination-buttons" aria-label="Pagination">
        <button class="page-btn" :disabled="page === 1" @click="goToPage(1)" title="Première page">
          &laquo;
        </button>

        <button
          class="page-btn"
          :disabled="page === 1"
          @click="goToPage(page - 1)"
          title="Page précédente"
        >
          &lsaquo;
        </button>

        <button
          v-for="p in visiblePages"
          :key="p"
          class="page-btn page-number"
          :class="{ active: p === page }"
          @click="goToPage(p)"
        >
          {{ p }}
        </button>

        <button
          class="page-btn"
          :disabled="page === totalPages"
          @click="goToPage(page + 1)"
          title="Page suivante"
        >
          &rsaquo;
        </button>

        <button
          class="page-btn"
          :disabled="page === totalPages"
          @click="goToPage(totalPages)"
          title="Dernière page"
        >
          &raquo;
        </button>
      </nav>
    </div>
  </div>
</template>

<style scoped>
.pagination-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0.875rem 1rem;
  background-color: #ffffff;
  border-top: 1px solid #e2e8f0;
  font-size: 0.875rem;
  color: #64748b;
  flex-wrap: wrap;
  gap: 1rem;
}

.font-medium {
  font-weight: 600;
  color: #1e293b;
}

.pagination-controls {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

/* Sélecteur de taille de page */
.items-per-page {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.select-label {
  font-size: 0.875rem;
}

.page-size-select {
  padding: 0.25rem 0.5rem;
  border: 1px solid #cbd5e1;
  border-radius: 0.375rem;
  background-color: #ffffff;
  color: #1e293b;
  font-size: 0.875rem;
  outline: none;
  cursor: pointer;
}

.page-size-select:focus {
  border-color: #2563eb;
}

/* Navigation par boutons */
.pagination-buttons {
  display: flex;
  align-items: center;
  gap: 0.25rem;
}

.page-btn {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 2rem;
  height: 2rem;
  padding: 0 0.375rem;
  border: 1px solid #cbd5e1;
  background-color: #ffffff;
  color: #334155;
  border-radius: 0.375rem;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.15s ease-in-out;
  user-select: none;
}

.page-btn:hover:not(:disabled) {
  background-color: #f1f5f9;
  border-color: #94a3b8;
  color: #1e293b;
}

.page-btn.active {
  background-color: #2563eb;
  color: #ffffff;
  border-color: #2563eb;
}

.page-btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
  background-color: #f8fafc;
}
</style>
