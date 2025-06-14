import { ConnectedPosition, OverlayModule } from '@angular/cdk/overlay';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { CommonModule } from '@angular/common';
import { booleanAttribute, ChangeDetectionStrategy, Component, input, model } from '@angular/core';

@Component({
   selector: 'twa-overlay',
   templateUrl: 'overlay.component.html',
   styleUrl: 'overlay.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, OverlayModule, ScrollingModule],
})
export class TetrisWebAppOverlayComponent {
   readonly isOpen = model<boolean>(false);

   readonly disableClose = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly disposeOnNavigation = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly flexibleDimensions = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly growAfterOpen = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly hasBackdrop = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly lockPosition = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly height = input<number | string>('');

   readonly minHeight = input<number | string>('');

   readonly width = input<number | string>('');

   readonly minWidth = input<number | string>('');

   readonly offsetX = input<number>(0);

   readonly offsetY = input<number>(0);

   readonly positions = input<Array<ConnectedPosition>>(defaultPositionList);

   readonly push = input<boolean, unknown>(false, { transform: booleanAttribute });

   readonly transformOriginSelector = input<string>('');

   readonly viewportMargin = input<number>(0);

   onDetach(): void {
      this.isOpen.set(false);
   }
}

const defaultPositionList: ConnectedPosition[] = [
   {
      originX: 'start',
      originY: 'bottom',
      overlayX: 'start',
      overlayY: 'top',
   },
   {
      originX: 'start',
      originY: 'top',
      overlayX: 'start',
      overlayY: 'bottom',
   },
   {
      originX: 'end',
      originY: 'top',
      overlayX: 'end',
      overlayY: 'bottom',
   },
   {
      originX: 'end',
      originY: 'bottom',
      overlayX: 'end',
      overlayY: 'top',
   },
];
