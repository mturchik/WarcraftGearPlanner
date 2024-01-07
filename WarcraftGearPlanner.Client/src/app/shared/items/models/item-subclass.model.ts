import { BaseModel } from '../../base.model';
import { InventoryType } from './inventory-type.model';

export interface ItemSubclass extends BaseModel {
  itemClassId: string;
  name: string;
  verboseName?: string;
  subclassId: number;
  hideTooltip: boolean;
  displayOrder?: number;
  inventoryTypes: InventoryType[];
}
