import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ButtonComponent } from '@twa-core';

@Component({
   selector: 'twa-navigation-link',
   templateUrl: 'navigation-link.component.html',
   styleUrl: 'navigation-link.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, ButtonComponent],
})
export class NavigationLinkComponent {
   readonly route = input.required<Route>();

   constructor(private readonly router: Router) {}

   onRouteTo(): void {
      this.router.navigate([this.route().path]);
   }
}
