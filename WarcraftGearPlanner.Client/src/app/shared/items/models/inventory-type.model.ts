import { BaseModel } from '../../base.model';

export interface InventoryType extends BaseModel {
  name: string;
  type: string;
  displayOrder?: number;
}
