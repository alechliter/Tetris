import { Routes } from '@angular/router';
import { HomePageComponent } from './home-page/home-page.component';
import { ThemePageComponent } from './theme-page/theme-page.component';

export const routes: Routes = [
   { title: 'Home', path: 'home', component: HomePageComponent },
   { title: 'Themes', path: 'themes', component: ThemePageComponent },
   { path: '', redirectTo: '/home', pathMatch: 'full' },
];
