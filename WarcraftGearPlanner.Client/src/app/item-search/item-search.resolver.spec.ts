import { TestBed } from '@angular/core/testing';
import { ResolveFn } from '@angular/router';

import { itemSearchResolver } from './item-search.resolver';

describe('itemSearchResolver', () => {
  const executeResolver: ResolveFn<boolean> = (...resolverParameters) => 
      TestBed.runInInjectionContext(() => itemSearchResolver(...resolverParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeResolver).toBeTruthy();
  });
});
