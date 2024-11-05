import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramtypeComponent } from './programtype.component';

describe('ProgramtypeComponent', () => {
  let component: ProgramtypeComponent;
  let fixture: ComponentFixture<ProgramtypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ProgramtypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProgramtypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
