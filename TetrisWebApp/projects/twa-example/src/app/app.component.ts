import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ExampleComponent, ThemeService } from '@twa-core';
import { DefaultThemeOptions, ThemeOptionsToken } from '@twa-core';

@Component({
   selector: 'app-root',
   providers: [ThemeService],
   imports: [RouterOutlet, ExampleComponent],
   templateUrl: './app.component.html',
   styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
   constructor(private themeService: ThemeService) {}

   ngOnInit(): void {
      this.themeService.initialize();
   }
}
