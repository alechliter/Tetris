import { CommonModule } from '@angular/common';
import { Component, computed, inject, input, signal, Signal, WritableSignal } from '@angular/core';
import { MatTooltip, TooltipPosition } from '@angular/material/tooltip';
import { Subject, takeUntil } from 'rxjs';
import { IconComponent } from '../../../icon/icon.component';
import { ThemeModel } from '../../models/theme.model';
import { ThemeService } from '../../theme.service';
import { ColorScheme, Theme } from '../../types/theme.types';

@Component({
   selector: 'twa-color-scheme-toggle',
   templateUrl: 'color-scheme-toggle.component.html',
   styleUrl: 'color-scheme-toggle.component.scss',
   imports: [CommonModule, MatTooltip, IconComponent],
})
export class ColorSchemeToggleComponent {
   readonly tooltipPosition = input<TooltipPosition>('right');

   protected readonly oppositeColorScheme: Signal<ColorScheme>;

   protected readonly selectedTheme: WritableSignal<Theme> = signal('default');

   protected readonly selectedColorScheme: WritableSignal<ColorScheme> = signal('light');

   private readonly themeService: ThemeService;

   private readonly unsubscribe = new Subject<void>();

   constructor() {
      this.themeService = inject(ThemeService);

      this.oppositeColorScheme = computed(this.computeOppositeColorScheme.bind(this));
   }

   ngOnInit(): void {
      this.updateThemeSelection(this.themeService.current);
      this.themeService.theme.pipe(takeUntil(this.unsubscribe)).subscribe(theme => {
         this.updateThemeSelection(theme);
      });
   }

   onToggleColorScheme(): void {
      this.themeService.changeTheme({ scheme: this.oppositeColorScheme() });
   }

   private computeOppositeColorScheme(): ColorScheme {
      if (this.selectedColorScheme() === 'light') {
         return 'dark';
      } else {
         return 'light';
      }
   }

   private updateThemeSelection(theme: ThemeModel): void {
      this.selectedColorScheme.set(theme.scheme);
   }
}
