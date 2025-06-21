import { Injectable, Signal, signal, WritableSignal } from '@angular/core';
import { SectionLevel } from '../section/enums/section-level.enum';
import { SectionModel } from '../section/models/section.model';
import { SectionComponent } from '../section/section.component';

@Injectable({ providedIn: 'root' })
export class SectionMenuService {
   readonly sections: Signal<Array<SectionModel>>;

   private readonly _sections: WritableSignal<Array<SectionModel>> = signal([]);

   constructor() {
      this.sections = this._sections.asReadonly();
   }

   register(sectionComponent: SectionComponent): SectionModel {
      const section = new SectionModel({
         sectionID: this.createSectionID(sectionComponent),
         level: this.calculateLevel(sectionComponent),
         elementRef: sectionComponent.elementRef,
         name: sectionComponent.name,
         parent: sectionComponent.parent,
      });

      if (section.parent) {
         section.parent.addSubsection(section);
      } else {
         this._sections.update(sections => [...sections, section]);
      }

      return section;
   }

   deregister(section: SectionModel): void {
      if (section.parent) {
         section.parent.removeSubsection(section.sectionID);
      } else {
         this._sections.update(sections =>
            sections.filter(section => section.sectionID !== section.sectionID)
         );
      }
   }

   private createSectionID(section: SectionComponent): string {
      if (section.parent) {
         return `${section.parent.sectionID}.${section.parent.subsections().length + 1}`;
      } else {
         return `${this.sections().length + 1}`;
      }
   }

   private calculateLevel(section: SectionComponent): SectionLevel {
      if (section.parent) {
         return section.parent.level + 1;
      } else {
         return SectionLevel.Heading1;
      }
   }
}
