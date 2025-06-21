import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Signal } from '@angular/core';
import { SectionModel } from '../section/models/section.model';
import { SectionMenuService } from '../services/section-menu.service';
import { SectionMenuItemComponent } from './section-menu-item/section-menu-item.component';

@Component({
   selector: 'twa-section-menu',
   templateUrl: 'section-menu.component.html',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, SectionMenuItemComponent],
})
export class SectionMenuComponent {
   readonly sections: Signal<Array<SectionModel>>;

   constructor(private readonly sectionMenuService: SectionMenuService) {
      this.sections = this.sectionMenuService.sections;
   }
}
