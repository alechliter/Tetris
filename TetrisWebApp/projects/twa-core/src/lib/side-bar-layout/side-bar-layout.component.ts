import { ChangeDetectionStrategy, Component, contentChild, input, OnInit } from '@angular/core';
import { SideBarComponent } from './side-bar/side-bar.component';
import { SideBarContentComponent } from './side-bar-content/side-bar-content.component';
import { CommonModule } from '@angular/common';

@Component({
   selector: 'twa-side-bar-layout',
   templateUrl: 'side-bar-layout.component.html',
   styleUrl: 'side-bar-layout.component.scss',
   standalone: true,
   changeDetection: ChangeDetectionStrategy.OnPush,
   imports: [CommonModule],
})
export class SideBarLayoutComponent {
   readonly sidebar = contentChild.required(SideBarComponent);

   readonly content = contentChild.required(SideBarContentComponent);
}
