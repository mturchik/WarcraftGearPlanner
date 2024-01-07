import { Injectable } from '@angular/core';
import { CacheItem } from './cache-item.model';

@Injectable({ providedIn: 'root' })
export class CacheService {
  private _storage: Storage = localStorage;

  constructor() {}

  /**
   * Retrieve the item if it exists and is not expired, otherwise returns null.
   * @param key Key of the item to retrieve.
   * @returns The item if it exists and is not expired, otherwise returns null.
   */
  public getItem<T>(key: string): T | null {
    const item = this._storage.getItem(key);
    if (!item) return null;

    const cacheItem: CacheItem<T> = JSON.parse(item);
    if (cacheItem.expires < Date.now()) {
      this.removeItem(key);
      return null;
    }

    return cacheItem.value;
  }

  /**
   * Set the item in the cache.
   * @param key Key of the item to set.
   * @param value Value of the item to set.
   * @param expires Time in milliseconds until the item expires. Defaults to 1 day.
   */
  public setItem<T>(key: string, value: T, expires: number = 86400000) {
    if (!value) return;

    const cacheItem: CacheItem<T> = {
      expires: Date.now() + expires,
      value,
    };

    this._storage.setItem(key, JSON.stringify(cacheItem));
  }

  /**
   * Remove the item from the cache.
   * @param key Key of the item to remove.
   */
  public removeItem(key: string) {
    this._storage.removeItem(key);
  }

  /**
   * Clear the cache.
   */
  public reset() {
    this._storage.clear();
  }
}
