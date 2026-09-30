<script setup lang="ts">
import { onClickOutside } from '@vueuse/core'
import { ref, useTemplateRef } from 'vue'

const modal = useTemplateRef('modal')

const isOpen = ref(false)

function openModal() {
  isOpen.value = true
}

function closeModal() {
  isOpen.value = false
}

onClickOutside(modal, () => (isOpen.value = false))
</script>

<template>
  <div class="modal-wrapper">
    <div v-if="isOpen" class="modal" ref="modal">
      <div class="modal-content">
        <slot name="content" :closeModal="closeModal" />
      </div>
    </div>
    <slot name="opener" :openModal="openModal" />
  </div>
</template>

<style>
.modal-wrapper {
  width: 100%;
  height: 100%;
}
.modal {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  z-index: 100;
  background-color: var(--color-white);
  border-radius: 14px;
  box-shadow: 2px 2px 5px black;
  max-width: 100%;
  max-height: 100%;
  min-width: min-content;
  min-height: min-content;

  .modal-content {
    padding: 24px 32px;
  }
}
</style>
