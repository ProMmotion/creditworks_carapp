import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import HomeView from '@/views/HomeView.vue'

describe('HomeView', () => {
  it('renders the vehicle management heading and car inventory component', () => {
    const wrapper = mount(HomeView, {
      global: {
        stubs: {
          CarTable: { template: '<div data-testid="car-table">Car inventory</div>' },
        },
      },
    })

    expect(wrapper.get('h1').text()).toBe('Vehicle Management')
    expect(wrapper.get('[data-testid="car-table"]').text()).toBe('Car inventory')
  })
})
