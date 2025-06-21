import { CommonModule } from '@angular/common';
import {
   ChangeDetectionStrategy,
   Component,
   computed,
   inject,
   OnInit,
   Signal,
   signal,
   WritableSignal,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
   ButtonComponent,
   ButtonToggleComponent,
   ButtonToggleItemComponent,
   ColorScheme,
   ColorSchemeToggleComponent,
   IconComponent,
   Theme,
   ThemeModel,
   ThemeService,
} from '@twa-core';
import { Subject, takeUntil } from 'rxjs';

@Component({
   selector: 'twa-theme-page',
   templateUrl: 'theme-page.component.html',
   styleUrl: 'theme-page.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [
      CommonModule,
      FormsModule,
      ReactiveFormsModule,
      ButtonComponent,
      IconComponent,
      ButtonToggleComponent,
      ButtonToggleItemComponent,
      ColorSchemeToggleComponent,
   ],
})
export class ThemePageComponent implements OnInit {
   protected readonly themes: Array<Theme>;

   protected readonly oppositeColorScheme: Signal<ColorScheme>;

   protected readonly selectedTheme: WritableSignal<Theme> = signal('default');

   protected readonly selectedColorScheme: WritableSignal<ColorScheme> = signal('light');

   private readonly themeService: ThemeService;

   private readonly unsubscribe = new Subject<void>();

   constructor(private readonly snackbar: MatSnackBar) {
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

   onIconClicked(): void {
      this.snackbar.open('Icon clicked', 'Close', { duration: 3_000 });
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
