import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectWrapperComponent } from './project-wrapper.component';

describe('ProjectWrapperComponent', () => {
  let component: ProjectWrapperComponent;
  let fixture: ComponentFixture<ProjectWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
