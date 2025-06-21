import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TetrisBoardComponent } from '@twa-core';

@Component({
   selector: 'twa-board-page',
   templateUrl: 'board-page.component.html',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, TetrisBoardComponent],
})
export class BoardPageComponent {
   constructor() {}
}
