import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, input } from '@angular/core';
import { ButtonComponent } from '@twa-core';
import { SectionModel } from '../../section/models/section.model';

@Component({
   selector: 'twa-section-menu-item',
   templateUrl: 'section-menu-item.component.html',
   styleUrl: 'section-menu-item.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, ButtonComponent],
})
export class SectionMenuItemComponent {
   readonly section = input.required<SectionModel>();

   constructor() {}

   onSectionSelected(): void {
      this.section().elementRef.nativeElement.scrollIntoView({ behavior: 'smooth' });
   }
}
