<script setup lang="ts" generic="T">
import { computed } from 'vue'

defineProps<{
  label: string
  id?: string
  placeholder?: string
  required?: boolean
  disabled?: boolean
  error?: string
}>()

const model = defineModel<T>()

const inputType = computed(() => typeof model.value)
</script>

<template>
  <div class="form-group" :class="{ 'has-error': error }">
    <label v-if="label" :for="id" class="form-label">
      {{ label }}
      <span v-if="required" class="required">*</span>
    </label>

    <input
      v-model="model"
      :id
      :type="inputType"
      step="0.01"
      :placeholder
      :disabled
      :required
      class="form-input"
    />

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

.form-input {
  width: 100%;
  padding: 0.625rem 0.875rem;
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

.form-input::placeholder {
  color: #64748b;
  opacity: 0.8;
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
