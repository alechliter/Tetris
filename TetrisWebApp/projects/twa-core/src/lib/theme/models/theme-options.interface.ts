import { InjectionToken } from '@angular/core';
import { Theme } from '../types/theme.types';

export const ThemeOptionsToken = new InjectionToken<IThemeOptions>('IThemeOptions');

export interface IThemeOptions {
   themes: Array<Theme>;
}

export const DefaultThemeOptions: IThemeOptions = {
   themes: ['default', 'forest'],
};
