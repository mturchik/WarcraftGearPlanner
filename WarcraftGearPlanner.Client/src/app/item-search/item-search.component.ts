import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import {
  BehaviorSubject,
  ReplaySubject,
  filter,
  map,
  switchMap,
  tap,
} from 'rxjs';
import { ItemService } from '../shared/items/item.service';
import { InventoryType } from '../shared/items/models/inventory-type.model';
import { ItemClass } from '../shared/items/models/item-class.model';
import { ItemQuality } from '../shared/items/models/item-quality.model';
import { ItemSearchParameters } from '../shared/items/models/item-search-parameters.model';
import { ItemSubclass } from '../shared/items/models/item-subclass.model';
import { SearchRequest } from '../shared/search.request';
import { ItemSearchResolverData } from './item-search.resolver';

@Component({
  selector: 'warcraft-gear-planner-item-search',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './item-search.component.html',
  styleUrl: './item-search.component.scss',
})
export class ItemSearchComponent implements OnInit {
  searchItems$ = new ReplaySubject<SearchRequest<ItemSearchParameters>>();
  searchResults$ = new BehaviorSubject([]);

  form = new FormGroup({
    name: new FormControl(''),
    itemClass: new FormControl(''),
    itemSubclass: new FormControl(''),
    itemQuality: new FormControl(''),
    inventoryType: new FormControl(''),
  });

  itemClasses: ItemClass[] = [];
  itemSubclasses: ItemSubclass[] = [];
  inventoryTypes: InventoryType[] = [];
  filteredInventoryTypes: InventoryType[] = [];
  itemQualities: ItemQuality[] = [];

  constructor(
    private _itemService: ItemService,
    private _activatedRoute: ActivatedRoute
  ) {}

  ngOnInit(): void {
    // Load data from resolver
    this._activatedRoute.data
      .pipe(
        map((data) => data['itemSearch'] as ItemSearchResolverData),
        tap((data) => {
          console.log('🚀 ~ ItemSearchResolverData:', data);
          this.itemClasses = data.itemClasses;
          this.inventoryTypes = data.inventoryTypes;
          this.itemQualities = data.itemQualities;
        })
      )
      .subscribe();
    // Handle item class changes to filter sub-classes and set default values
    this.form.controls.itemClass.valueChanges
      .pipe(
        tap((itemClassId) => {
          const itemClass = this.itemClasses.find(
            (ic) => ic.id === itemClassId
          );
          this.itemSubclasses = itemClass?.subclasses ?? [];
          if (!this.itemSubclasses?.length) return;

          if (this.itemSubclasses.length === 1) {
            this.form.controls.itemSubclass.setValue(this.itemSubclasses[0].id);
            this.form.controls.itemSubclass.disable();
          } else {
            this.form.controls.itemSubclass.setValue('');
            this.form.controls.itemSubclass.enable();
          }
        })
      )
      .subscribe();
    // Handle item subclass changes to filter inventory types and set default values
    this.form.controls.itemSubclass.valueChanges
      .pipe(
        tap((itemSubclassId) => {
          const itemSubclass = this.itemSubclasses.find(
            (isc) => isc.id === itemSubclassId
          );
          this.filteredInventoryTypes = this.inventoryTypes.filter((it) =>
            itemSubclass?.inventoryTypes.some((ist) => ist.id === it.id)
          );
          if (!this.filteredInventoryTypes?.length) return;

          if (this.filteredInventoryTypes.length === 1) {
            this.form.controls.inventoryType.setValue(
              this.filteredInventoryTypes[0].id
            );
            this.form.controls.inventoryType.disable();
          } else {
            this.form.controls.inventoryType.setValue('');
            this.form.controls.inventoryType.enable();
          }
        })
      )
      .subscribe();
    // Handle search requests
    this.searchItems$
      .pipe(
        filter((searchRequest) => !!searchRequest),
        tap((searchRequest) => console.log('searchRequest:', searchRequest)),
        switchMap((searchRequest) =>
          this._itemService.searchItems(searchRequest)
        ),
        tap((items) => this.searchResults$.next(items))
      )
      .subscribe();
    // Log search results
    this.searchResults$.subscribe((items) => console.log('items:', items));
  }

  search() {
    const searchRequest: SearchRequest<ItemSearchParameters> = {
      page: 1,
      pageSize: 10,
      parameters: this._createSearchParameters(),
    };
    this.searchItems$.next(searchRequest);
  }

  private _createSearchParameters(): ItemSearchParameters {
    const parameters: ItemSearchParameters = {
      itemClassIds: [],
      itemSubclassIds: [],
      itemQualityIds: [],
      inventoryIds: [],
    };
    const name = this.form.controls.name.value;
    if (name) parameters.name = name;

    const itemClass = this.form.controls.itemClass.value;
    if (itemClass) parameters.itemClassIds?.push(itemClass);

    const itemSubclass = this.form.controls.itemSubclass.value;
    if (itemSubclass) parameters.itemSubclassIds?.push(itemSubclass);

    const itemQuality = this.form.controls.itemQuality.value;
    if (itemQuality) parameters.itemQualityIds?.push(itemQuality);

    const inventoryType = this.form.controls.inventoryType.value;
    if (inventoryType) parameters.inventoryIds?.push(inventoryType);

    return parameters;
  }
}
