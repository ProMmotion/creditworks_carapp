import { mount, flushPromises } from '@vue/test-utils'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import CategoryManagement from '@/components/CategoryManagement.vue'

const { deleteCategory } = vi.hoisted(() => ({ deleteCategory: vi.fn() }))

vi.mock('@/requests/category', () => ({
  useCategoryRequests: () => ({ deleteCategory }),
}))

const categories = [
  { id: 1, name: 'Lightweight', icon: '1k', filters: { weight: { from: 0, to: 1000 } } },
  { id: 2, name: 'Heavy', icon: '3k', filters: { weight: { from: 2000 } } },
]

const categoryFormStub = {
  props: ['category'],
  emits: ['submitted'],
  template: '<div data-testid="category-form">{{ category?.name }}</div>',
}

describe('CategoryManagement', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    vi.stubGlobal(
      'confirm',
      vi.fn(() => true),
    )
  })

  it('lists categories and formats weight ranges', () => {
    const wrapper = mount(CategoryManagement, {
      props: { categories },
      global: { stubs: { CategoryForm: categoryFormStub } },
    })

    expect(wrapper.text()).toContain('Lightweight')
    expect(wrapper.text()).toContain('0 kg to 1000 kg')
    expect(wrapper.text()).toContain('2000 kg to Infinity')
  })

  it('opens the form with the selected category for editing', async () => {
    const wrapper = mount(CategoryManagement, {
      props: { categories },
      global: { stubs: { CategoryForm: categoryFormStub } },
    })

    await wrapper.get('.category-card').get('button.secondary').trigger('click')

    expect(wrapper.get('[data-testid="category-form"]').text()).toBe('Lightweight')
  })

  it('deletes a category after confirmation', async () => {
    const wrapper = mount(CategoryManagement, {
      props: { categories },
      global: { stubs: { CategoryForm: categoryFormStub } },
    })

    await wrapper.get('.category-card').get('button.danger').trigger('click')
    await flushPromises()

    expect(confirm).toHaveBeenCalledWith('Are you sure you want to delete this category?')
    expect(deleteCategory).toHaveBeenCalledWith(1)
  })

  it('does not delete a category when confirmation is declined', async () => {
    vi.stubGlobal(
      'confirm',
      vi.fn(() => false),
    )
    const wrapper = mount(CategoryManagement, {
      props: { categories },
      global: { stubs: { CategoryForm: categoryFormStub } },
    })

    await wrapper.get('.category-card').get('button.danger').trigger('click')

    expect(deleteCategory).not.toHaveBeenCalled()
  })
})
