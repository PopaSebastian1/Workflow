import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildIssuesComponent } from './child-issues.component';

describe('ChildIssuesComponent', () => {
  let component: ChildIssuesComponent;
  let fixture: ComponentFixture<ChildIssuesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ChildIssuesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ChildIssuesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
