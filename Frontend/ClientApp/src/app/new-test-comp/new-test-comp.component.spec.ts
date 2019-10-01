import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewTestCompComponent } from './new-test-comp.component';

describe('NewTestCompComponent', () => {
  let component: NewTestCompComponent;
  let fixture: ComponentFixture<NewTestCompComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewTestCompComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewTestCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
