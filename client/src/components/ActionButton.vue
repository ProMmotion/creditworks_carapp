<script setup lang="ts">
defineProps<{
  type: 'button' | 'submit'
  level: 'primary' | 'secondary' | 'tertiary' | 'danger' | 'warning' | 'info'
  disabled?: boolean
}>()
</script>

<template>
  <button :disabled :type :class="['btn', level]"><slot /></button>
</template>

<style scoped lang="scss">
.btn {
  display: inline-flex;
  flex-wrap: nowrap;
  white-space: nowrap;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  font-family: inherit;
  font-size: 0.875rem;
  font-weight: 500;
  line-height: 1.25rem;
  padding: 0.625rem 1.25rem;
  border-radius: 0.375rem;
  border: 1px solid transparent;
  cursor: pointer;
  transition: all 0.2s ease-in-out;
  outline: none;
  text-decoration: none;
}

.btn:focus-visible {
  ring: 2px;
  outline: 2px solid var(--color-primary);
  outline-offset: 2px;
}

.btn.primary {
  background-color: var(--color-primary);
  color: var(--color-white);
}
.btn.primary:hover {
  background-color: var(--color-primary-hover);
}

.btn.secondary {
  background-color: var(--color-secondary);
  color: var(--color-white);
}
.btn.secondary:hover {
  background-color: var(--color-secondary-hover);
}

.btn.tertiary {
  background-color: var(--color-tertiary);
  color: var(--color-text-dark);
  border-color: #cbd5e1;
}
.btn.tertiary:hover {
  background-color: var(--color-tertiary-hover);
}

.btn.danger {
  background-color: var(--color-danger);
  color: var(--color-white);
}
.btn.danger:hover {
  background-color: var(--color-danger-hover);
}

.btn.warning {
  background-color: var(--color-warning);
  color: var(--color-white);
}
.btn.warning:hover {
  background-color: var(--color-warning-hover);
}

.btn.info {
  background-color: transparent;
  color: var(--color-info);
  border-color: var(--color-info);
}
.btn.info:hover {
  background-color: #f0f9ff;
  color: var(--color-info-hover);
}

.btn:disabled,
.btn.disabled {
  opacity: 0.6;
  cursor: not-allowed;
  pointer-events: none;
}
</style>
