import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component } from '@angular/core';
import { SectionLayoutComponent } from '../../components/section-layout/section-layout.component';
import { SectionComponent } from '../../components/section-layout/section/section.component';

@Component({
   selector: 'twa-home-page',
   templateUrl: 'home-page.component.html',
   styleUrl: 'home-page.component.scss',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule, SectionLayoutComponent, SectionComponent],
})
export class HomePageComponent {
   constructor() {}
}
