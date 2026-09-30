<script setup lang="ts">
import type { Range } from '@/utils/Range'

const { min } = defineProps<{
  min: number
  max: number
  id: string
  unit: string
  label?: string
  required?: boolean
  disabled?: boolean
  error?: string
}>()

const model = defineModel<Partial<Range>>({ required: true })
</script>

<template>
  <div class="form-group" :class="{ 'has-error': error }">
    <label v-if="label" :for="id" class="form-label">
      {{ label }}
      <span v-if="required" class="required">*</span>
    </label>

    <div class="range-container">
      <div class="range-field">
        <span class="range-prefix">Min</span>
        <input
          :id="`${id}-min`"
          type="number"
          step="0.01"
          :min
          :max
          :value="model.from"
          :disabled
          class="form-input"
          @input="model.from = $event.target.value"
        />
        <span class="range-unit">{{ unit }}</span>
      </div>

      <span class="range-separator">to</span>

      <div class="range-field">
        <span class="range-prefix">Max</span>
        <input
          :id="`${id}-max`"
          type="number"
          step="0.01"
          :min
          :max
          :value="model.to"
          :disabled
          class="form-input"
          @input="model.to = $event.target.value"
        />
        <span class="range-unit">{{ unit }}</span>
      </div>
    </div>

    <span v-if="error" class="error-message">{{ error }}</span>
  </div>
</template>

<style scoped>
.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.375rem;
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

.range-container {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  width: 100%;
}

.range-field {
  position: relative;
  display: flex;
  align-items: center;
  flex: 1;
}

.range-prefix {
  position: absolute;
  left: 0.75rem;
  font-size: 0.75rem;
  font-weight: 600;
  color: #64748b;
  text-transform: uppercase;
  pointer-events: none;
}

.range-unit {
  position: absolute;
  right: 0.75rem;
  font-size: 0.875rem;
  color: #64748b;
  pointer-events: none;
}

.form-input {
  width: 100%;
  padding: 0.625rem 2.25rem 0.625rem 3rem;
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

.form-input:disabled {
  background-color: #f8fafc;
  border-color: #e2e8f0;
  color: #94a3b8;
  cursor: not-allowed;
}

.range-separator {
  font-size: 0.875rem;
  font-weight: 500;
  color: #64748b;
}

/* Validation & Erreurs */
.form-group.has-error .form-input {
  border-color: #dc2626;
}

.form-group.has-error .form-input:focus {
  box-shadow: 0 0 0 3px rgba(220, 38, 38, 0.15);
}

.error-message {
  font-size: 0.75rem;
  color: #dc2626;
  margin-top: 0.25rem;
}
</style>
