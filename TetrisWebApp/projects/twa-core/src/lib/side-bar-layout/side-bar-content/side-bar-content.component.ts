import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
   selector: 'twa-side-bar-content',
   templateUrl: 'side-bar-content.component.html',
   styleUrl: 'side-bar-content.component.scss',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideBarContentComponent {
   constructor() {}
}
