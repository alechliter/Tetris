import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ColorSchemeToggleComponent } from '@twa-core';
import { routes } from '../../app/app.routes';
import { QuickSettingsMenuComponent } from '../quick-settings-menu/quick-settings-menu.component';
import { NavigationLinkComponent } from './navigation-link/navigation-link.component';

@Component({
   selector: 'twa-navigation-menu',
   templateUrl: 'navigation-menu.component.html',
   styleUrl: 'navigation-menu.component.scss',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, NavigationLinkComponent, ColorSchemeToggleComponent, QuickSettingsMenuComponent],
})
export class NavigationMenuComponent {
   protected readonly availableRoutes;

   constructor(private router: Router) {
      this.availableRoutes = routes.filter(route => route.title);
   }

   onRouteTo(route: Route): void {
      this.router.navigate([route.path]);
   }
}
