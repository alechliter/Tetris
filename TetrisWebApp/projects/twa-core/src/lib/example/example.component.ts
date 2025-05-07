import { CommonModule } from '@angular/common';
import {
   ChangeDetectionStrategy,
   Component,
   computed,
   HostBinding,
   inject,
   OnInit,
   Signal,
   signal,
   WritableSignal,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ThemeService } from '../theme/theme.service';
import { Subject, takeUntil } from 'rxjs';
import { ColorScheme, Theme, ThemeModel } from '../../public-api';

@Component({
   selector: 'twa-lib-example',
   templateUrl: 'example.component.html',
   styleUrl: 'example.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class ExampleComponent implements OnInit {
   protected readonly themes: Array<Theme>;

   protected readonly oppositeColorScheme: Signal<ColorScheme>;

   protected readonly selectedTheme: WritableSignal<Theme> = signal('default');

   protected readonly selectedColorScheme: WritableSignal<ColorScheme> = signal('light');

   private readonly themeService: ThemeService;

   private readonly unsubscribe = new Subject<void>();

   constructor() {
      this.themeService = inject(ThemeService);

      this.themes = this.themeService.getThemes();

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

   onThemeChange(theme: Theme): void {
      this.themeService.changeTheme({ theme: theme });
   }

   private computeOppositeColorScheme(): ColorScheme {
      if (this.selectedColorScheme() === 'light') {
         return 'dark';
      } else {
         return 'light';
      }
   }

   private updateThemeSelection(theme: ThemeModel): void {
      this.selectedTheme.set(theme.theme);
      this.selectedColorScheme.set(theme.scheme);
   }
}
