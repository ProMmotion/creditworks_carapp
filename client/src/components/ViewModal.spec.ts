import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import ViewModal from '@/components/ViewModal.vue'

describe('ViewModal', () => {
  it('reveals slot content after the opener action and hides it after closing', async () => {
    const wrapper = mount(ViewModal, {
      slots: {
        opener:
          '<template #opener="{ openModal }"><button @click="openModal">Open</button></template>',
        content:
          '<template #content="{ closeModal }"><section data-testid="dialog"><button @click="closeModal">Close</button></section></template>',
      },
    })

    expect(wrapper.find('[data-testid="dialog"]').exists()).toBe(false)

    await wrapper.get('button').trigger('click')
    expect(wrapper.find('[data-testid="dialog"]').exists()).toBe(true)

    await wrapper.get('[data-testid="dialog"] button').trigger('click')
    expect(wrapper.find('[data-testid="dialog"]').exists()).toBe(false)
  })
})
