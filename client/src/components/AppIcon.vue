<script setup lang="ts">
const { width = 32, height = 32 } = defineProps<{
  icon: string
  width?: number
  height?: number
}>()
</script>

<template>
  <i :class="`icon-${icon}`" :style="{ width: `${width}px`, height: `${height}px` }" />
</template>
