import { mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import AppInput from '@/components/AppInput.vue'

describe('AppInput', () => {
  it('renders a labeled input with its value and required state', () => {
    const wrapper = mount(AppInput, {
      props: { label: 'Year', modelValue: 2024, required: true, id: 'year' },
    })

    const input = wrapper.get('input')
    expect(wrapper.get('label').text()).toContain('Year')
    expect(input.attributes('type')).toBe('number')
    expect(input.element.value).toBe('2024')
    expect(input.attributes('required')).toBeDefined()
    expect(wrapper.get('label').attributes('for')).toBe('year')
  })

  it('emits updated values from user input and displays validation errors', async () => {
    const wrapper = mount(AppInput, {
      props: { label: 'Name', modelValue: '', error: 'Name is required' },
    })

    await wrapper.get('input').setValue('Coupe')

    expect(wrapper.emitted('update:modelValue')).toEqual([['Coupe']])
    expect(wrapper.get('.error-message').text()).toBe('Name is required')
    expect(wrapper.get('.form-group').classes()).toContain('has-error')
  })
})
