import { CommonModule } from '@angular/common';
import { booleanAttribute, ChangeDetectionStrategy, Component, input, model, output } from '@angular/core';
import { NgClassInput } from '../../types/angular.types';

@Component({
   selector: 'twa-icon',
   templateUrl: 'icon.component.html',
   styleUrl: 'icon.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class IconComponent {
   readonly icon = model<string>();

   readonly iconClass = model<NgClassInput>();

   readonly clickable = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly clicked = output<Event>();

   onClick(event: Event): void {
      event.preventDefault();
      event.stopPropagation();

      this.clicked.emit(event);
   }
}
