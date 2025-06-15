import { ChangeDetectionStrategy, Component, computed, ElementRef, input, Signal } from '@angular/core';
import { IconComponent } from '../../icon/icon.component';
import { ButtonToggleComponent, ButtonToggleItemRect } from '../button-toggle.component';

@Component({
   selector: 'twa-button-toggle-item',
   templateUrl: 'button-toggle-item.component.html',
   styleUrl: 'button-toggle-item.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [IconComponent],
})
export class ButtonToggleItemComponent<TValue> {
   get rect(): ButtonToggleItemRect {
      const boundingRect = this.elementRef.nativeElement.getBoundingClientRect();
      return {
         x: this.elementRef.nativeElement.offsetLeft,
         y: this.elementRef.nativeElement.offsetTop,
         width: boundingRect.width,
         height: boundingRect.height,
      };
   }

   readonly value = input.required<TValue>();

   protected readonly isSelected: Signal<boolean>;

   constructor(
      private readonly elementRef: ElementRef<HTMLElement>,
      private readonly parent: ButtonToggleComponent<TValue>
   ) {
      this.isSelected = computed(() => this.value() === this.parent.selected());
   }

   onClick(event: MouseEvent | Event): void {
      event.stopPropagation();
      event.preventDefault();

      this.parent.selected.set(this.value());
   }
}
