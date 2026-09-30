<script setup lang="ts">
import AppSelect from './AppSelect.vue'

const iconFiles = import.meta.glob('@/assets/icons/*.svg', { eager: true })
// @ts-expect-error shouldn't happen because of the glob pattern
const icons = Object.keys(iconFiles).map((path) => path.match(/\/([^/]+)\.svg$/)[1])

defineProps<{
  required?: boolean
  disabled?: boolean
}>()

const icon = defineModel<string>('icon')
</script>

<template>
  <AppSelect
    v-model="icon"
    id="brandSelect"
    :required
    :disabled
    :getLabel="(i) => i"
    :getValue="(i) => i"
    :options="icons"
  />
</template>
