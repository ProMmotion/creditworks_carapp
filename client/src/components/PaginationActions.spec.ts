import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import PaginationActions from '@/components/PaginationActions.vue'

describe('PaginationActions', () => {
  it('shows the total and disables backward navigation on the first page', () => {
    const wrapper = mount(PaginationActions, {
      props: { total: 120, page: 1, pageSize: 10 },
    })

    expect(wrapper.find('.pagination-info').text()).toContain('120')
    expect(wrapper.get('button[title="Première page"]').element.disabled).toBe(true)
    expect(wrapper.get('button[title="Page précédente"]').element.disabled).toBe(true)
    expect(wrapper.get('button[title="Page suivante"]').element.disabled).toBe(false)
  })

  it('emits a page update when a page number is clicked', async () => {
    const wrapper = mount(PaginationActions, {
      props: { total: 120, page: 1, pageSize: 10 },
    })

    await wrapper.findAll('button.page-number')[2].trigger('click')

    expect(wrapper.emitted('update:page')).toEqual([[3]])
  })

  it('emits the selected page size', async () => {
    const wrapper = mount(PaginationActions, {
      props: { total: 120, page: 1, pageSize: 10 },
    })

    await wrapper.get('#pageSizeSelect').setValue('25')

    expect(wrapper.emitted('update:pageSize')).toEqual([[25]])
  })

  it('does not navigate beyond the last page', async () => {
    const wrapper = mount(PaginationActions, {
      props: { total: 20, page: 2, pageSize: 10 },
    })

    expect(wrapper.get('button[title="Page suivante"]').element.disabled).toBe(true)
    await wrapper.get('button[title="Page suivante"]').trigger('click')

    expect(wrapper.emitted('update:page')).toBeUndefined()
  })
})
