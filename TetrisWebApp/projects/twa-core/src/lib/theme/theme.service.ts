import { inject, Injectable, OnDestroy, Renderer2 } from '@angular/core';
import { BehaviorSubject, Observable, pairwise, startWith, Subject, takeUntil } from 'rxjs';
import { ThemeModel } from './models/theme.model';
import { ColorScheme, Theme } from './types/theme.types';
import { DefaultThemeOptions, IThemeOptions, ThemeOptionsToken } from './models/theme-options.interface';
import { DOCUMENT } from '@angular/common';

@Injectable()
export class ThemeService implements OnDestroy {
   get theme(): Observable<ThemeModel> {
      return this._theme.asObservable();
   }

   get current(): ThemeModel {
      return new ThemeModel(this._theme.value);
   }

   private readonly themeOptions: IThemeOptions;

   private readonly renderer: Renderer2;

   private readonly document: Document;

   private readonly _theme = new BehaviorSubject<ThemeModel>(new ThemeModel());

   private readonly stopThemeListeners = new Subject<void>();

   constructor() {
      this.themeOptions = inject(ThemeOptionsToken, { optional: true }) ?? DefaultThemeOptions;
      this.document = inject(DOCUMENT);
      this.renderer = inject(Renderer2);
   }

   ngOnDestroy(): void {
      this._theme.complete();
   }

   initialize(): void {
      this.theme
         .pipe(startWith(new ThemeModel()), pairwise(), takeUntil(this.stopThemeListeners))
         .subscribe(([previous, current]) => {
            this.setThemeClass(previous.theme, current.theme);
            this.setColorScheme(current.scheme);
         });
   }

   changeTheme(theme: Partial<ThemeModel>): void {
      this._theme.next(
         new ThemeModel({
            ...this._theme.value,
            ...theme,
         })
      );
   }

   getThemes(): Array<Theme> {
      return [...this.themeOptions.themes];
   }

   private setThemeClass(previous: Theme, current: Theme): void {
      this.renderer.removeClass(this.document.body, this.getThemeClass(previous));
      this.renderer.addClass(this.document.body, this.getThemeClass(current));
   }

   private setColorScheme(scheme: ColorScheme): void {
      this.renderer.setStyle(this.document.body, `color-scheme`, scheme);
   }

   private getThemeClass(theme: Theme): string {
      return `${theme}-theme`;
   }
}
