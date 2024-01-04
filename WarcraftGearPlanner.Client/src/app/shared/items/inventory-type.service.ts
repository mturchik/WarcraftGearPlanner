import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, tap } from 'rxjs';
import { BaseService } from '../base.service';
import { CacheService } from '../cache/cache.service';
import { ConfigService } from '../config/config.service';
import { InventoryType } from './models/inventory-type.model';

@Injectable({ providedIn: 'root' })
export class InventoryTypeService extends BaseService {
  protected _appName = 'wgp-api';

  constructor(
    http: HttpClient,
    configService: ConfigService,
    cacheService: CacheService
  ) {
    super(http, configService, cacheService);
  }

  getInventoryTypes(): Observable<InventoryType[]> {
    const cachedValue = this.cacheService.getItem<any[]>('inventory-types');
    if (cachedValue) return of(cachedValue);
    return this.get<InventoryType[]>('inventory-types').pipe(
      map((inventoryTypes) =>
        inventoryTypes.sort((a, b) => a.name.localeCompare(b.name))
      ),
      tap((inventoryTypes) =>
        this.cacheService.setItem('inventory-types', inventoryTypes)
      )
    );
  }
}
