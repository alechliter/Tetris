import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, input, signal, WritableSignal } from '@angular/core';
import { Route, Router } from '@angular/router';
import { ButtonComponent, ButtonFocusChangeEvent } from '@twa-core';

@Component({
   selector: 'twa-navigation-link',
   templateUrl: 'navigation-link.component.html',
   styleUrl: 'navigation-link.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, ButtonComponent],
})
export class NavigationLinkComponent {
   readonly route = input.required<Route>();

   protected readonly isFocused: WritableSignal<boolean> = signal(false);

   constructor(private readonly router: Router) {}

   onRouteTo(): void {
      this.router.navigate([this.route().path]);
   }

   onFocusChange(event: ButtonFocusChangeEvent): void {
      this.isFocused.set(event.active);
   }
}
