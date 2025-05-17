import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
   selector: 'twa-side-bar',
   templateUrl: 'side-bar.component.html',
   styleUrl: 'side-bar.component.scss',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class SideBarComponent {
   readonly title = input.required<string>();

   constructor() {}
}
