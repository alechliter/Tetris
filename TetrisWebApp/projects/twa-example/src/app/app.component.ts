import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SideBarComponent, SideBarContentComponent, SideBarLayoutComponent, ThemeService } from '@twa-core';
import { NavigationMenuComponent } from './navigation-menu/navigation-menu.component';
import { SectionMenuComponent } from './section-layout/menu/section-menu.component';

@Component({
   selector: 'app-root',
   providers: [ThemeService],
   imports: [
      RouterOutlet,
      SideBarComponent,
      SideBarContentComponent,
      SideBarLayoutComponent,
      NavigationMenuComponent,
      SectionMenuComponent,
   ],
   templateUrl: './app.component.html',
   styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
   readonly title = 'Example App';

   constructor(private themeService: ThemeService) {}

   ngOnInit(): void {
      this.themeService.initialize();
   }
}
