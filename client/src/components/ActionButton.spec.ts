import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import ActionButton from '@/components/ActionButton.vue'

describe('ActionButton', () => {
  it('forwards its type and visual level to the button', () => {
    const wrapper = mount(ActionButton, {
      props: { type: 'submit', level: 'primary' },
      slots: { default: 'Save changes' },
    })

    expect(wrapper.get('button').attributes('type')).toBe('submit')
    expect(wrapper.get('button').classes()).toContain('primary')
    expect(wrapper.text()).toBe('Save changes')
  })

  it('forwards its disabled state', () => {
    const wrapper = mount(ActionButton, {
      props: { type: 'button', level: 'danger', disabled: true },
    })

    expect((wrapper.get('button').element as HTMLButtonElement).disabled).toBe(true)
  })
})
