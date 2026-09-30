import { useStorage } from '@vueuse/core'
import { readonly, ref, shallowRef, type Ref, type ShallowRef } from 'vue'

export function useRequestWrapper<
  FnType extends (...args: Parameters<FnType>) => ReturnType<FnType>,
>(
  cb: FnType,
  options?: {
    useCache?: string
    preventReload?: (data?: Awaited<ReturnType<FnType>>, ...args: Parameters<FnType>) => boolean
    merge?: (
      newValue: Awaited<ReturnType<FnType>>,
      oldValue?: Awaited<ReturnType<FnType>>,
    ) => Awaited<ReturnType<FnType>>
  },
): [
  Ref<Awaited<ReturnType<FnType>> | undefined>,
  (...args: Parameters<FnType>) => Promise<void>,
  Readonly<ShallowRef<boolean>>,
  Ref<unknown>,
] {
  const response = options?.useCache
    ? useStorage(options.useCache, undefined, sessionStorage, {
        serializer: {
          read: (raw) => JSON.parse(raw) as Awaited<ReturnType<FnType>>,
          write: (data) => JSON.stringify(data),
        },
      })
    : ref<Awaited<ReturnType<FnType>>>()

  const error = ref<unknown>()
  const loading = shallowRef(false)

  const queue: Parameters<FnType>[] = []
  function completeQueue() {
    const qArgs = queue.shift()
    if (!qArgs) return
    // @ts-expect-error type is ok
    load(qArgs)
  }

  async function load(...args: Parameters<FnType>): Promise<void> {
    if (loading.value) {
      queue.push(args)
      return
    }
    if (options?.preventReload?.(response.value, ...args)) return
    loading.value = true
    try {
      const res = (await cb(...args)) as Awaited<ReturnType<FnType>>
      if (options?.merge) {
        response.value = options.merge(res, response.value)
      } else {
        response.value = res
      }
    } catch (e) {
      error.value = e
    } finally {
      loading.value = false
      completeQueue()
    }
  }

  return [response, load, readonly(loading), error]
}
