import { ChangeDetectionStrategy, Component, computed, contentChildren, model, Signal } from '@angular/core';
import { ButtonToggleItemComponent } from './item/button-toggle-item.component';

export interface ButtonToggleItemRect {
   x: number;
   y: number;
   width: number;
   height: number;
}

@Component({
   selector: 'twa-button-toggle',
   templateUrl: 'button-toggle.component.html',
   styleUrl: 'button-toggle.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ButtonToggleComponent<TValue> {
   readonly selected = model.required<TValue>();

   protected readonly items = contentChildren(ButtonToggleItemComponent<TValue>);

   protected readonly selectionRect: Signal<ButtonToggleItemRect | undefined>;

   constructor() {
      this.selectionRect = computed(this.computeSelectionRect.bind(this));
   }

   private computeSelectionRect(): ButtonToggleItemRect | undefined {
      const selectedItem = this.items().find(item => item.value() === this.selected());
      if (!selectedItem) {
         return;
      }

      return selectedItem.rect;
   }
}
