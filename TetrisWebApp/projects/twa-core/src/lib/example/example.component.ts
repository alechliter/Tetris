import { CommonModule } from '@angular/common';
import {
   ChangeDetectionStrategy,
   Component,
   HostBinding,
   OnInit,
   signal,
   WritableSignal,
} from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
   selector: 'twa-lib-example',
   templateUrl: 'example.component.html',
   styleUrl: 'example.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class ExampleComponent {
   @HostBinding('style.color-scheme') get colorScheme(): string {
      return this.currentColorScheme();
   }

   protected readonly selectedTheme: WritableSignal<string> = signal('');

   protected readonly currentColorScheme: WritableSignal<string> = signal('light');

   constructor() {}

   onToggleColorScheme(): void {
      this.currentColorScheme.update(colorScheme => {
         if (colorScheme === 'light') {
            return 'dark';
         } else {
            return 'light';
         }
      });
   }

   onThemeChange(theme: string): void {
      this.selectedTheme.set(`${theme}-theme`);
   }
}
