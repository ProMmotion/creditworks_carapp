<script setup lang="ts" generic="T">
import { onClickOutside } from '@vueuse/core'
import { ref, computed, useTemplateRef } from 'vue'

const { options, allowCreate, getLabel, getValue } = defineProps<{
  id: string
  options: T[]
  getLabel: (option: T) => string
  getValue: (option: T) => string | number
  label?: string
  allowCreate?: boolean
  required?: boolean
  disabled?: boolean
  error?: string
}>()

const model = defineModel<T>()

const emit = defineEmits<{
  createOption: [string]
}>()

const isOpen = ref(false)
const searchQuery = ref(String(model.value ? getLabel(model.value) : ''))
const highlightedIndex = ref(-1)
const dropdownRef = useTemplateRef('dropdownRef')

const filteredOptions = computed(() => {
  if (!searchQuery.value) return options
  const query = searchQuery.value.toLowerCase()
  return options.filter((opt) => getLabel(opt).toLowerCase().includes(query))
})

const canCreateNew = computed(() => {
  if (!allowCreate) return false
  const trimmed = searchQuery.value.trim()
  if (!trimmed) return false

  const exists = options.some((opt) => getLabel(opt).toLowerCase() === trimmed.toLowerCase())
  return !exists
})

const onInput = (event) => {
  searchQuery.value = event.target.value
  isOpen.value = true
  highlightedIndex.value = 0
}

const navigateOptions = (step: number) => {
  if (!isOpen.value) {
    isOpen.value = true
    return
  }
  const total = filteredOptions.value.length + (canCreateNew.value ? 1 : 0)
  if (total === 0) return

  highlightedIndex.value = (highlightedIndex.value + step + total) % total
}

const selectHighlighted = () => {
  if (!isOpen.value) return

  if (canCreateNew.value && highlightedIndex.value === filteredOptions.value.length) {
    createOption()
  } else if (highlightedIndex.value >= 0 && highlightedIndex.value < filteredOptions.value.length) {
    selectOption(filteredOptions.value[highlightedIndex.value])
  } else if (canCreateNew.value) {
    createOption()
  }
}

const selectOption = (option: T) => {
  searchQuery.value = getLabel(option)
  model.value = option
  isOpen.value = false
}

const createOption = () => {
  const newVal = searchQuery.value.trim()
  if (!newVal) return

  emit('createOption', newVal)
  isOpen.value = false
}

onClickOutside(dropdownRef, () => {
  isOpen.value = false
})
</script>

<template>
  <div class="form-group" :class="{ 'has-error': error }" ref="dropdownRef">
    <label v-if="label" :for="id" class="form-label">
      {{ label }}
      <span v-if="required" class="required">*</span>
    </label>

    <div class="combobox-wrapper">
      <input
        :id="id"
        type="text"
        class="form-input"
        :value="searchQuery"
        :disabled="disabled"
        @input="onInput"
        @focus="isOpen = true"
        @keydown.down.prevent="navigateOptions(1)"
        @keydown.up.prevent="navigateOptions(-1)"
        @keydown.enter.prevent="selectHighlighted"
        @keydown.escape="isOpen = false"
      />

      <button
        type="button"
        class="toggle-btn"
        :disabled="disabled"
        @click="isOpen = !isOpen"
        tabindex="-1"
      >
        <svg
          class="chevron-icon"
          :class="{ open: isOpen }"
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M19 9l-7 7-7-7"
          />
        </svg>
      </button>

      <ul v-if="isOpen && !disabled" class="dropdown-menu">
        <li
          v-for="(option, index) in filteredOptions"
          :key="getValue(option)"
          class="dropdown-item"
          :class="{
            selected: getValue(option) === model,
            highlighted: index === highlightedIndex,
          }"
          @click="selectOption(option)"
          @mouseenter="highlightedIndex = index"
        >
          {{ getLabel(option) }}
        </li>

        <li
          v-if="canCreateNew"
          class="dropdown-item create-item"
          :class="{ highlighted: highlightedIndex === filteredOptions.length }"
          @click="createOption"
          @mouseenter="highlightedIndex = filteredOptions.length"
        >
          <span class="plus-icon">+</span> Add "{{ searchQuery.trim() }}"
        </li>

        <li v-if="filteredOptions.length === 0 && !canCreateNew" class="dropdown-item no-result">
          No option found
        </li>
      </ul>
    </div>

    <span v-if="error" class="error-message">{{ error }}</span>
  </div>
</template>

<style scoped>
.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.375rem;
  position: relative;
}

.form-label {
  font-size: 0.875rem;
  font-weight: 500;
  color: #1e293b;
}

.form-label .required {
  color: #dc2626;
  margin-left: 0.25rem;
}

.combobox-wrapper {
  position: relative;
  width: 100%;
}

.form-input {
  width: 100%;
  padding: 0.625rem 2.5rem 0.625rem 0.875rem;
  font-family: inherit;
  font-size: 0.875rem;
  line-height: 1.25rem;
  color: #1e293b;
  background-color: #ffffff;
  border: 1px solid #cbd5e1;
  border-radius: 0.375rem;
  box-shadow: 0 1px 2px 0 rgba(0, 0, 0, 0.05);
  transition:
    border-color 0.15s ease-in-out,
    box-shadow 0.15s ease-in-out;
  outline: none;
  box-sizing: border-box;
}

.form-input:hover {
  border-color: #94a3b8;
}

.form-input:focus {
  border-color: #2563eb;
  box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.15);
}

.toggle-btn {
  position: absolute;
  right: 0.5rem;
  top: 50%;
  transform: translateY(-50%);
  background: transparent;
  border: none;
  padding: 0.25rem;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #64748b;
}

.chevron-icon {
  width: 1rem;
  height: 1rem;
  transition: transform 0.2s ease;
}

.chevron-icon.open {
  transform: rotate(180deg);
}

/* Liste déroulante Dropdown */
.dropdown-menu {
  position: absolute;
  top: calc(100% + 0.25rem);
  left: 0;
  right: 0;
  max-height: 15rem;
  overflow-y: auto;
  background-color: #ffffff;
  border: 1px solid #e2e8f0;
  border-radius: 0.375rem;
  box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  z-index: 50;
  list-style: none;
  padding: 0.25rem 0;
  margin: 0;
}

.dropdown-item {
  padding: 0.625rem 0.875rem;
  font-size: 0.875rem;
  color: #1e293b;
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  transition: background-color 0.1s ease;
}

.dropdown-item.highlighted {
  background-color: #f1f5f9;
}

.dropdown-item.selected {
  font-weight: 600;
  color: #2563eb;
}

/* Item de création personnalisée */
.dropdown-item.create-item {
  color: #2563eb;
  font-weight: 500;
  border-top: 1px solid #f1f5f9;
  background-color: #f0f9ff;
}

.dropdown-item.create-item.highlighted {
  background-color: #e0f2fe;
}

.plus-icon {
  font-weight: 700;
  font-size: 1rem;
}

.no-result {
  color: #94a3b8;
  cursor: default;
}

/* Erreurs */
.form-group.has-error .form-input {
  border-color: #dc2626;
}

.error-message {
  font-size: 0.75rem;
  color: #dc2626;
  margin-top: 0.25rem;
}
</style>
