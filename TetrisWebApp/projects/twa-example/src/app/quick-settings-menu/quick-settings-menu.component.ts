import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit, signal, WritableSignal } from '@angular/core';
import { MatTooltip } from '@angular/material/tooltip';
import {
   ButtonToggleComponent,
   ButtonToggleItemComponent,
   IconComponent,
   OverlayComponent,
   Theme,
   ThemeModel,
   ThemeService,
} from '@twa-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
   selector: 'twa-quick-settings-menu',
   templateUrl: 'quick-settings-menu.component.html',
   styleUrl: 'quick-settings-menu.component.scss',
   imports: [
      CommonModule,
      IconComponent,
      OverlayComponent,
      MatTooltip,
      ButtonToggleComponent,
      ButtonToggleItemComponent,
   ],
})
export class QuickSettingsMenuComponent implements OnInit, OnDestroy {
   protected readonly isOpen: WritableSignal<boolean> = signal(false);

   protected readonly selectedTheme: WritableSignal<Theme> = signal('default');

   protected readonly themes: Array<Theme>;

   private readonly themeService = inject(ThemeService);

   private readonly unsubscribe = new Subject<void>();

   constructor() {
      this.themes = this.themeService.getThemes();
   }

   ngOnInit(): void {
      this.updateThemeSelection(this.themeService.current);
      this.themeService.theme.pipe(takeUntil(this.unsubscribe)).subscribe(theme => {
         this.updateThemeSelection(theme);
      });
   }

   ngOnDestroy(): void {
      this.unsubscribe.next();
   }

   toggleSettings(): void {
      this.isOpen.update(isOpen => !isOpen);
   }

   onThemeChange(theme: Theme): void {
      this.themeService.changeTheme({ theme: theme });
   }

   private updateThemeSelection(theme: ThemeModel): void {
      this.selectedTheme.set(theme.theme);
   }
}
