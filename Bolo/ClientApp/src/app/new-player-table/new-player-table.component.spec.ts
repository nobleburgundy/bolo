import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewPlayerTableComponent } from './new-player-table.component';

describe('NewPlayerTableComponent', () => {
  let component: NewPlayerTableComponent;
  let fixture: ComponentFixture<NewPlayerTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewPlayerTableComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NewPlayerTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
