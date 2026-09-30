import axios, { AxiosHeaders, type AxiosResponse } from 'axios'
import { computed } from 'vue'

export function useApiRequests(prefix?: string) {
  const apiURL = import.meta.env.VUE_APP_API_URL
  const path = computed(() => {
    const c = apiURL
    return prefix ? `${c}/${prefix}` : c
  })

  function isOk(response: AxiosResponse<unknown>) {
    return response.status >= 200 && response.status < 300
  }

  async function get<T>(route: string, qs?: Record<string, unknown>) {
    const res = await axios.get<T>(`${path.value}${route}`, {
      params: qs,
      paramsSerializer: { indexes: null },
    })
    if (isOk(res)) return res.data
    throw res.data
  }

  async function post<T>(route: string, body: unknown) {
    const res = await axios.post<T>(`${path.value}${route}`, body, {})
    if (isOk(res)) return res.data
    throw res.data
  }

  async function patch<T>(route: string, body: unknown) {
    const res = await axios.patch<T>(`${path.value}${route}`, body, {})
    if (isOk(res)) return res.data
    throw res.data
  }

  async function del(route: string) {
    const res = await axios.delete(`${path.value}${route}`, {})
    if (isOk(res)) return res.data
    throw res.data
  }

  return {
    get,
    post,
    patch,
    delete: del,
  }
}
