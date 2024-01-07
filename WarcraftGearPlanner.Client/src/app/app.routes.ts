import { Route } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ItemSearchComponent } from './item-search/item-search.component';
import { itemSearchResolver } from './item-search/item-search.resolver';

export const appRoutes: Route[] = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'item-search',
    component: ItemSearchComponent,
    resolve: { resolverData: itemSearchResolver },
  },
  {
    path: '**',
    component: HomeComponent,
  },
];
