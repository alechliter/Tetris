import { ColorScheme, Theme } from '../types/theme.types';

export class ThemeModel {
   theme: Theme;

   scheme: ColorScheme;

   constructor(opts?: Partial<ThemeModel>) {
      this.theme = opts?.theme ?? 'default';
      this.scheme = opts?.scheme ?? 'light';
   }
}
