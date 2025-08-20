import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleViewComponent } from './sale-view.component';

describe('SaleViewComponent', () => {
  let component: SaleViewComponent;
  let fixture: ComponentFixture<SaleViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SaleViewComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SaleViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
