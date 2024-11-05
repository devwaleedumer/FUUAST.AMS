import { TestBed } from '@angular/core/testing';

import { ProgramtypeService } from './programtype.service';

describe('ProgramtypeService', () => {
  let service: ProgramtypeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ProgramtypeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
