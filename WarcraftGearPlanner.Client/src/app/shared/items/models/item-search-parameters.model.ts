export interface ItemSearchParameters {
  name?: string;
  itemLevelMin?: number;
  itemLevelMax?: number;
  requiredLevelMin?: number;
  requiredLevelMax?: number;
  itemClassIds?: string[];
  itemSubclassIds?: string[];
  itemQualityIds?: string[];
  inventoryIds?: string[];
}
