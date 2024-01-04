import { BaseModel } from '../../base.model';
import { ItemSubclass } from './item-subclass.model';

export interface ItemClass extends BaseModel {
  name: string;
  classId: number;
  subclasses: ItemSubclass[];
}
