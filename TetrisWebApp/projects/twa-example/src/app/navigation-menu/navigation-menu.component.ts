import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
   selector: 'twa-navigation-menu',
   templateUrl: 'navigation-menu.component.html',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, RouterLink, RouterLinkActive],
})
export class NavigationMenuComponent {
   constructor() {}
}
