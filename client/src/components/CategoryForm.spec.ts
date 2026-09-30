import { mount, flushPromises } from '@vue/test-utils'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import CategoryForm from '@/components/CategoryForm.vue'

const { createCategory, patchCategory, addCreatedCategory } = vi.hoisted(() => ({
  createCategory: vi.fn(),
  patchCategory: vi.fn(),
  addCreatedCategory: vi.fn(),
}))

vi.mock('@/requests/category', () => ({
  useCategoryRequests: () => ({ createCategory, patchCategory }),
}))

vi.mock('@/stores/categories', () => ({
  useCategoryStore: () => ({ addCreatedCategory }),
}))

const testStubs = {
  AppInput: {
    props: ['label', 'modelValue'],
    emits: ['update:modelValue'],
    template:
      '<label>{{ label }}<input :aria-label="label" :value="modelValue" @input="$emit(\'update:modelValue\', $event.target.value)" /></label>',
  },
  IconSelect: {
    props: ['label', 'modelValue'],
    emits: ['update:modelValue'],
    template:
      '<label>{{ label }}<input :aria-label="label" :value="modelValue" @input="$emit(\'update:modelValue\', $event.target.value)" /></label>',
  },
  RangeInput: {
    props: ['modelValue'],
    template: '<div data-testid="weight-range">{{ modelValue.from }}–{{ modelValue.to }}</div>',
  },
}

describe('CategoryForm', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    createCategory.mockResolvedValue({ id: 12 })
    patchCategory.mockResolvedValue(undefined)
  })

  it('creates a category, updates the store, and emits submitted', async () => {
    const wrapper = mount(CategoryForm, { global: { stubs: testStubs } })

    await wrapper.get('input[aria-label="Name"]').setValue('Lightweight')
    await wrapper.get('input[aria-label="Icon"]').setValue('1k')
    await wrapper.get('form').trigger('submit')
    await flushPromises()

    expect(createCategory).toHaveBeenCalledWith({
      id: 0,
      name: 'Lightweight',
      icon: '1k',
      filters: { weight: { from: 0, to: 0 } },
    })
    expect(addCreatedCategory).toHaveBeenCalledWith({
      id: 12,
      name: 'Lightweight',
      icon: '1k',
      filters: { weight: { from: 0, to: 0 } },
    })
    expect(wrapper.emitted('submitted')).toHaveLength(2)
  })

  it('updates an existing category without adding a new store item', async () => {
    const category = {
      id: 4,
      name: 'Current',
      icon: '2k',
      filters: { weight: { from: 500, to: 1500 } },
    }
    const wrapper = mount(CategoryForm, {
      props: { category },
      global: { stubs: testStubs },
    })

    await wrapper.get('input[aria-label="Name"]').setValue('Updated')
    await wrapper.get('form').trigger('submit')
    await flushPromises()

    expect(patchCategory).toHaveBeenCalledWith(4, expect.objectContaining({ name: 'Updated' }))
    expect(addCreatedCategory).not.toHaveBeenCalled()
    expect(wrapper.emitted('submitted')).toHaveLength(1)
  })
})
