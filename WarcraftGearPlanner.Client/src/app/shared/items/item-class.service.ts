import { Injectable } from '@angular/core';
import { sort } from 'fast-sort';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { ItemClass } from './models/item-class.model';

@Injectable({ providedIn: 'root' })
export class ItemClassService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(configService: ConfigService, cacheService: CacheService) {
    super(configService, cacheService);
  }

  async getItemClasses() {
    const cachedValue = this.cacheService.getItem<any[]>('item-classes');
    if (cachedValue) return cachedValue;

    let itemClasses = await this.get<ItemClass[]>('item-classes').catch(
      (err) => {
        console.error('Error loading item classes: ', err);
        return [];
      }
    );

    itemClasses = itemClasses.filter((x) => (x.displayOrder ?? -1) >= 0);
    itemClasses = sort(itemClasses).asc([
      (x) => x.displayOrder ?? 0,
      (x) => x.name,
    ]);

    itemClasses.forEach((itemClass) => {
      itemClass.subclasses = itemClass.subclasses.filter(
        (x) => (x.displayOrder ?? -1) >= 0
      );
      itemClass.subclasses = sort(itemClass.subclasses).asc([
        (x) => x.displayOrder ?? 0,
        (x) => x.verboseName ?? x.name,
      ]);
    });

    this.cacheService.setItem('item-classes', itemClasses);
    return itemClasses;
  }
}
