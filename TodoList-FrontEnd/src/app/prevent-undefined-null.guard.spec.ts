import { TestBed } from '@angular/core/testing';

import { PreventUndefinedNullGuard } from './prevent-undefined-null.guard';

describe('PreventUndefinedNullGuard', () => {
  let guard: PreventUndefinedNullGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(PreventUndefinedNullGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
