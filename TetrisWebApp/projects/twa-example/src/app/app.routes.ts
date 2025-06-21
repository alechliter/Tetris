import { Routes } from '@angular/router';
import { BoardPageComponent } from '../pages/board-page/board-page.component';
import { HomePageComponent } from '../pages/home-page/home-page.component';
import { ThemePageComponent } from '../pages/theme-page/theme-page.component';

export const routes: Routes = [
   { title: 'Home', path: 'home', component: HomePageComponent },
   { title: 'Themes', path: 'themes', component: ThemePageComponent },
   { title: 'Board', path: 'board', component: BoardPageComponent },
   { path: '', redirectTo: '/home', pathMatch: 'full' },
];
