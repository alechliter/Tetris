import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
   selector: 'twa-tetris-board',
   templateUrl: 'tetris-board.component.html',
   styleUrl: 'tetris-board.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class TetrisBoardComponent {
   readonly rowCount = input.required<number>();

   readonly columnCount = input.required<number>();

   constructor() {}
}
