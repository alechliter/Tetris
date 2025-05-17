import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
   selector: 'twa-home-page',
   templateUrl: 'home-page.component.html',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class HomePageComponent {
   constructor() {}
}
