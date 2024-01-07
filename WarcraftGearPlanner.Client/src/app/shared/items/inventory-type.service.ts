import { Injectable } from '@angular/core';
import { sort } from 'fast-sort';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { InventoryType } from './models/inventory-type.model';

@Injectable({ providedIn: 'root' })
export class InventoryTypeService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(configService: ConfigService, cacheService: CacheService) {
    super(configService, cacheService);
  }

  async getInventoryTypes() {
    const cachedValue = this.cacheService.getItem<any[]>('inventory-types');
    if (cachedValue) return cachedValue;

    let inventoryTypes = await this.get<InventoryType[]>(
      'inventory-types'
    ).catch((err) => {
      console.error('Error loading inventory types: ', err);
      return [];
    });

    inventoryTypes = inventoryTypes.filter((x) => (x.displayOrder ?? -1) >= 0);
    inventoryTypes = sort(inventoryTypes).asc([
      (x) => x.displayOrder ?? 0,
      (x) => x.name,
    ]);

    this.cacheService.setItem('inventory-types', inventoryTypes);
    return inventoryTypes;
  }
}
