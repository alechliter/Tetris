import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ExampleComponent } from '@twa-core';

@Component({
   selector: 'app-root',
   imports: [RouterOutlet, ExampleComponent],
   templateUrl: './app.component.html',
   styleUrl: './app.component.scss',
})
export class AppComponent {}
