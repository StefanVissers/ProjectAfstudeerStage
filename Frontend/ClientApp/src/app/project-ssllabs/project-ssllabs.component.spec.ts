import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectSsllabsComponent } from './project-ssllabs.component';

describe('ProjectSsllabsComponent', () => {
  let component: ProjectSsllabsComponent;
  let fixture: ComponentFixture<ProjectSsllabsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectSsllabsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectSsllabsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
