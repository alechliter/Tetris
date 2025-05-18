import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { Route, Router } from '@angular/router';
import { routes } from '../app.routes';
import { NavigationLinkComponent } from './navigation-link/navigation-link.component';

@Component({
   selector: 'twa-navigation-menu',
   templateUrl: 'navigation-menu.component.html',
   styleUrl: 'navigation-menu.component.scss',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, NavigationLinkComponent],
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
