import { Injectable } from '@angular/core';
import { sort } from 'fast-sort';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { ItemQuality } from './models/item-quality.model';

@Injectable({ providedIn: 'root' })
export class ItemQualityService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(configService: ConfigService, cacheService: CacheService) {
    super(configService, cacheService);
  }

  async getItemQualities() {
    const cachedValue =
      this.cacheService.getItem<ItemQuality[]>('item-qualities');
    if (cachedValue) return cachedValue;

    let itemQualities = await this.get<ItemQuality[]>('item-qualities').catch(
      (err) => {
        console.error('Error loading item qualities: ', err);
        return [];
      }
    );

    itemQualities = itemQualities.filter((x) => (x.displayOrder ?? -1) >= 0);
    itemQualities = sort(itemQualities).asc([
      (x) => x.displayOrder ?? 0,
      (x) => x.name,
    ]);

    this.cacheService.setItem('item-qualities', itemQualities);
    return itemQualities;
  }
}
