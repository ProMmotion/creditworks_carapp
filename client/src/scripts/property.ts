export function getProperty(obj: Record<string, unknown>, path: string): unknown {
  const [key, rest] = path.split('.', 2)

  if (key) {
    const current = obj[key]
    if (rest) return getProperty(current as Record<string, unknown>, rest)
    return current
  }

  return obj
}
