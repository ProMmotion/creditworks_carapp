<script setup lang="ts" generic="T">
export type TableColumn = {
  field: string
  label: string
  sortBy?: string
  lineExtraClass?: string
}

const { sortBy, sortOrder } = defineProps<{
  items: T[]
  columns: TableColumn[]
  sortBy?: string
  sortOrder?: 'Asc' | 'Desc'
  loading?: boolean
}>()

const emit = defineEmits<{
  sort: [string]
}>()

const getSortIcon = (field: string) => {
  if (sortBy !== field) return '↕'
  return sortOrder === 'Asc' ? '↑' : '↓'
}

// Formatage du poids à 2 décimales
const formatWeight = (weight) => {
  if (weight === null || weight === undefined) return '-'
  return (
    Number(weight).toLocaleString('fr-FR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }) + ' kg'
  )
}

// Classe CSS personnalisée selon la catégorie
const getCategoryClass = (categoryName) => {
  if (!categoryName) return 'badge-neutral'
  const name = categoryName.toLowerCase()
  if (name.includes('light')) return 'badge-light'
  if (name.includes('medium')) return 'badge-medium'
  if (name.includes('heavy')) return 'badge-heavy'
  return 'badge-neutral'
}
</script>

<template>
  <div class="table-container">
    <table class="vehicle-table">
      <thead>
        <tr>
          <th
            v-for="(column, index) in columns"
            :key="`table-column-${index}`"
            :class="{ sortable: column.sortBy }"
            @click.stop="column.sortBy && emit('sort', column.sortBy)"
          >
            <span>{{ column.label }}</span>
            <span v-if="column.sortBy" class="sort-icon">{{ getSortIcon(column.field) }}</span>
          </th>
        </tr>
      </thead>

      <tbody>
        <tr v-if="loading">
          <td colspan="5" class="state-cell">Loading...</td>
        </tr>

        <tr v-else-if="items.length === 0">
          <td colspan="5" class="state-cell">No items in list.</td>
        </tr>

        <tr v-else v-for="(item, index) in items" :key="`table-line-${index}`" class="table-row">
          <td
            v-for="(column, colIndex) in columns"
            :key="`table-cell-${colIndex}`"
            :class="column.lineExtraClass"
          >
            <slot name="cell" :item :column />
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-container {
  width: 100%;
  overflow-x: auto;
  border-radius: 0.5rem;
  border: 1px solid #e2e8f0;
  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.05);
  background-color: #ffffff;
}

.vehicle-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
  font-size: 0.875rem;
  color: #1e293b;
}

/* En-têtes */
.vehicle-table th {
  background-color: #f8fafc;
  padding: 0.75rem 1rem;
  font-weight: 600;
  color: #475569;
  border-bottom: 1px solid #e2e8f0;
  user-select: none;
}

.vehicle-table th.sortable {
  cursor: pointer;
  transition: background-color 0.15s ease;
}

.vehicle-table th.sortable:hover {
  background-color: #f1f5f9;
}

.sort-icon {
  margin-left: 0.375rem;
  font-size: 0.75rem;
  color: #64748b;
}

/* Alignement */
.text-right {
  text-align: right;
}

.font-medium {
  font-weight: 500;
}

.font-mono {
  font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, monospace;
}

/* Lignes et cellules */
.vehicle-table td {
  padding: 0.875rem 1rem;
  border-bottom: 1px solid #f1f5f9;
  vertical-align: middle;
}

.table-row {
  transition: background-color 0.15s ease;
}

.table-row:hover {
  background-color: #f8fafc;
}

.table-row:last-child td {
  border-bottom: none;
}

/* État vide / chargement */
.state-cell {
  text-align: center;
  padding: 2rem;
  color: #64748b;
}

/* Badges de Catégories */
.category-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.375rem;
  padding: 0.25rem 0.625rem;
  border-radius: 9999px;
  font-size: 0.75rem;
  font-weight: 600;
  line-height: 1;
}

.category-icon {
  font-size: 0.875rem;
}

/* Variantes de badges */
.badge-light {
  background-color: #dcfce7;
  color: #15803d;
}

.badge-medium {
  background-color: #fef3c7;
  color: #b45309;
}

.badge-heavy {
  background-color: #fee2e2;
  color: #b91c1c;
}

.badge-neutral {
  background-color: #f1f5f9;
  color: #475569;
}
</style>
