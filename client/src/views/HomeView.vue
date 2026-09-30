<script setup lang="ts">
import CarTable from '@/components/CarTable.vue'
</script>

<template>
  <div class="main-view">
    <h1>Vehicle Management</h1>
    <CarTable />
  </div>
</template>

<style scoped lang="scss">
.main-view {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
}
</style>
