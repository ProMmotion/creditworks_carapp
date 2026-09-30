<script setup lang="ts">
defineProps<{
  level: 'primary' | 'secondary' | 'tertiary' | 'danger' | 'warning' | 'info'
}>()
</script>

<template>
  <div :class="['d-badge', level]">
    <slot />
  </div>
</template>

<style lang="scss" scoped>
@use 'sass:color';

.d-badge {
  width: fit-content;
  display: flex;
  flex-direction: row;
  gap: 4px;
  padding: 8px;
  border-radius: 8px;
  font-weight: bold;
  align-items: center;

  &.primary {
    background-color: var(--color-primary);
  }

  &.secondary {
    background-color: var(--color-secondary);
    color: var(--color-white);
  }

  &.tertiary {
    background-color: var(--color-tertiary);
  }

  &.danger {
    background-color: var(--color-danger);
  }

  &.warning {
    background-color: var(--color-warning);
  }

  &.info {
    background-color: var(--color-info);
    color: var(--color-white);
  }
}
</style>
