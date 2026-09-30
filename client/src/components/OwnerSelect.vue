<script setup lang="ts">
import { computed, onBeforeMount } from 'vue'
import AppSelect from './AppSelect.vue'
import { useOwnerStore } from '@/stores/owners'
import { useOwnerRequests } from '@/requests/owners'

defineProps<{
  required?: boolean
  disabled?: boolean
}>()

const ownerId = defineModel<number>('owner')

const ownerStore = useOwnerStore()
const ownerRequests = useOwnerRequests()

const owner = computed(() => ownerStore.owners?.find((o) => o.id === ownerId.value))

async function createOwner(name: string) {
  const { id } = await ownerRequests.createOwner({ name })
  ownerStore.addCreatedOwner({ id, name })
  ownerId.value = id
}

onBeforeMount(ownerStore.loadOwners)
</script>

<template>
  <AppSelect
    :modelValue="owner"
    id="ownerSelect"
    :required
    :disabled
    :getLabel="(owner) => owner.name"
    :getValue="(owner) => owner.id"
    :options="ownerStore.owners ?? []"
    @createOption="createOwner"
    @update:modelValue="ownerId = $event?.id"
    allowCreate
  />
</template>
