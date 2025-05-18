import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { NgClassInput } from '../../types/angular.types';

export type ButtonFocusChangeEvent = { active: boolean; event: Event };

@Component({
   selector: 'twa-button',
   templateUrl: 'button.component.html',
   styleUrl: 'button.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class ButtonComponent {
   readonly cssClass = input<NgClassInput>('primary');

   readonly type = input<'button' | 'save'>('button');

   readonly clicked = output<Event>();

   readonly focusChange = output<ButtonFocusChangeEvent>();

   onClick(event: MouseEvent | Event): void {
      this.clicked.emit(event);
   }

   onFocus(event: Event): void {
      this.focusChange.emit({ active: true, event: event });
   }

   onBlur(event: Event): void {
      this.focusChange.emit({ active: false, event: event });
   }
}
