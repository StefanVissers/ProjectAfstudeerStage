import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProjectKaliLinuxComponent } from './project-kali-linux.component';

describe('ProjectKaliLinuxComponent', () => {
  let component: ProjectKaliLinuxComponent;
  let fixture: ComponentFixture<ProjectKaliLinuxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProjectKaliLinuxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProjectKaliLinuxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
