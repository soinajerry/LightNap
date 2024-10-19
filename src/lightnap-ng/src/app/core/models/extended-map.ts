export class ExtendedMap<K, V> extends Map<K, V> {
  getOrSetDefault(key: K, defaultFactory: () => V) {
    if (!this.has(key)) {
      this.set(key, defaultFactory());
    }
    return this.get(key)!;
  }

  filter(predicate: (v: V, k: K) => boolean) {
    const newMap = new ExtendedMap<K, V>();
    const entries = Array.from(this.entries());
    for (const [key, value] of entries) {
      if (predicate(value, key)) {
        newMap.set(key, value);
      }
    }
    return newMap;
  }
}
