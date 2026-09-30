import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import ListTable from '@/components/ListTable.vue'

const columns = [
  { field: 'name', label: 'Name', sortBy: 'name' },
  { field: 'year', label: 'Year' },
]

describe('ListTable', () => {
  it('renders column headings and an empty state', () => {
    const wrapper = mount(ListTable, { props: { columns, items: [] } })

    expect(wrapper.findAll('thead th').map((header) => header.text())).toEqual(['Name↕', 'Year'])
    expect(wrapper.get('.state-cell').text()).toBe('No items in list.')
  })

  it('renders a loading message in place of rows', () => {
    const wrapper = mount(ListTable, {
      props: { columns, items: [{ name: 'Coupe', year: 2020 }], loading: true },
    })

    expect(wrapper.get('.state-cell').text()).toBe('Loading...')
    expect(wrapper.find('.table-row').exists()).toBe(false)
  })

  it('renders row cells through its scoped slot', () => {
    const wrapper = mount(ListTable, {
      props: { columns, items: [{ name: 'Coupe', year: 2020 }] },
      slots: {
        cell: '<template #cell="{ item, column }">{{ item[column.field] }}</template>',
      },
    })

    expect(wrapper.findAll('tbody tr td').map((cell) => cell.text())).toEqual(['Coupe', '2020'])
  })

  it('emits a sort event only for sortable columns', async () => {
    const wrapper = mount(ListTable, { props: { columns, items: [] } })

    await wrapper.get('th.sortable').trigger('click')
    await wrapper.get('th:nth-child(2)').trigger('click')

    expect(wrapper.emitted('sort')).toEqual([['name']])
  })
})
