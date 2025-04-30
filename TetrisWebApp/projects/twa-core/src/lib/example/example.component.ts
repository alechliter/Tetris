import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

@Component({
   selector: 'twa-lib-example',
   templateUrl: 'example.component.html',
   styleUrl: 'example.component.scss',
   imports: [CommonModule],
})
export class ExampleComponent {
   constructor() {}
}
