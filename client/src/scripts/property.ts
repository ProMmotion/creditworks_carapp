export function getProperty(obj: Record<string, unknown>, path: string): unknown {
  const [key, ...rest] = path.split('.')

  if (key) {
    const current = obj[key]
    if (rest) return getProperty(current as Record<string, unknown>, rest.join('.'))
    return current
  }

  return obj
}
