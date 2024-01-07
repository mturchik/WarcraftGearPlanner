import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { InventoryTypeService } from '../shared/items/inventory-type.service';
import { ItemClassService } from '../shared/items/item-class.service';
import { ItemQualityService } from '../shared/items/item-quality.service';
import { InventoryType } from '../shared/items/models/inventory-type.model';
import { ItemClass } from '../shared/items/models/item-class.model';
import { ItemQuality } from '../shared/items/models/item-quality.model';

export interface ItemSearchResolverData {
  itemClasses: ItemClass[];
  inventoryTypes: InventoryType[];
  itemQualities: ItemQuality[];
}

export const itemSearchResolver: ResolveFn<ItemSearchResolverData> = async (
  route,
  state
) => {
  const itemClassService = inject(ItemClassService);
  const itemQualityService = inject(ItemQualityService);
  const inventoryTypeService = inject(InventoryTypeService);

  const [itemClasses, inventoryTypes, itemQualities] = await Promise.all([
    itemClassService.getItemClasses(),
    inventoryTypeService.getInventoryTypes(),
    itemQualityService.getItemQualities(),
  ]);
  return {
    itemClasses,
    inventoryTypes,
    itemQualities,
  };
};
