import { CommonModule } from '@angular/common';
import {
   ChangeDetectionStrategy,
   Component,
   ElementRef,
   Host,
   input,
   OnDestroy,
   Optional,
   SkipSelf,
} from '@angular/core';
import { SectionMenuService } from '../services/section-menu.service';
import { SectionLevel } from './enums/section-level.enum';
import { SectionModel } from './models/section.model';

@Component({
   selector: 'twa-section',
   templateUrl: 'section.component.html',
   styleUrl: 'section.component.scss',
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class SectionComponent implements OnDestroy {
   get parent(): SectionModel | undefined {
      return this.parentSection?.section;
   }

   readonly name = input.required<string>();

   readonly section: SectionModel;

   protected readonly sectionLevel = SectionLevel;

   constructor(
      public readonly elementRef: ElementRef<HTMLElement>,
      private readonly sectionMenuService: SectionMenuService,
      @Optional() @SkipSelf() @Host() private readonly parentSection?: SectionComponent
   ) {
      this.section = this.sectionMenuService.register(this);
   }

   ngOnDestroy(): void {
      this.sectionMenuService.deregister(this.section);
   }
}
