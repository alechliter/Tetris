import { ElementRef, signal, Signal, WritableSignal } from '@angular/core';
import { PartiallyRequired } from '@twa-core';
import { SectionLevel } from '../enums/section-level.enum';

export class SectionModel {
   readonly subsections: Signal<Array<SectionModel>>;

   readonly sectionID: string;

   readonly name: Signal<string>;

   readonly elementRef: ElementRef<HTMLElement>;

   readonly level: SectionLevel;

   readonly parent?: SectionModel;

   private readonly _subsections: WritableSignal<Array<SectionModel>> = signal([]);

   constructor(opts: PartiallyRequired<SectionModel, 'sectionID' | 'elementRef' | 'name' | 'level'>) {
      this.sectionID = opts.sectionID;
      this.elementRef = opts.elementRef;
      this.name = opts.name;
      this.level = opts.level;
      this.parent = opts.parent;

      this.subsections = this._subsections.asReadonly();
   }

   addSubsection(section: SectionModel): void {
      this._subsections.update(sections => [...sections, section]);
   }

   removeSubsection(sectionID: string): void {
      this._subsections.update(sections => sections.filter(section => section.sectionID !== sectionID));
   }
}
