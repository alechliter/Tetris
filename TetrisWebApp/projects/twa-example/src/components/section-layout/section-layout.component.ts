import { ChangeDetectionStrategy, Component, input } from '@angular/core';

@Component({
   selector: 'twa-section-layout',
   templateUrl: 'section-layout.component.html',
   styleUrl: 'section-layout.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SectionLayoutComponent {
   readonly title = input.required<string>();

   constructor() {}
}
