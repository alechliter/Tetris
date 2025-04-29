import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TwaTetrisCoreComponent } from './twa-tetris-core.component';

describe('TwaTetrisCoreComponent', () => {
   let component: TwaTetrisCoreComponent;
   let fixture: ComponentFixture<TwaTetrisCoreComponent>;

   beforeEach(async () => {
      await TestBed.configureTestingModule({
         imports: [TwaTetrisCoreComponent],
      }).compileComponents();

      fixture = TestBed.createComponent(TwaTetrisCoreComponent);
      component = fixture.componentInstance;
      fixture.detectChanges();
   });

   it('should create', () => {
      expect(component).toBeTruthy();
   });
});
