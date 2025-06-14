import { CommonModule } from '@angular/common';
import { Component, signal, WritableSignal } from '@angular/core';
import { MatTooltip } from '@angular/material/tooltip';
import { IconComponent, OverlayComponent } from '@twa-core';

@Component({
   selector: 'twa-quick-settings-menu',
   templateUrl: 'quick-settings-menu.component.html',
   styleUrl: 'quick-settings-menu.component.scss',
   imports: [CommonModule, IconComponent, OverlayComponent, MatTooltip],
})
export class QuickSettingsMenuComponent {
   protected readonly isOpen: WritableSignal<boolean> = signal(false);

   toggleSettings(): void {
      this.isOpen.update(isOpen => !isOpen);
   }
}
