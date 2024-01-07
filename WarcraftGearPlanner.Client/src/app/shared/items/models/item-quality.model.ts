import { BaseModel } from '../../base.model';

export interface ItemQuality extends BaseModel {
  name: string;
  type: string;
  color?: string;
  displayOrder?: number;
}
