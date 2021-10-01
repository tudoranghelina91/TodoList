import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterActivate } from './register-activate.component';

describe('RegisterActivateComponent', () => {
  let component: RegisterActivate;
  let fixture: ComponentFixture<RegisterActivate>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterActivate ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterActivate);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
