import type { ReactNode } from 'react'

export type ThemeId = 'light' | 'dark' | 'sepia' | 'contrast' | 'ocean' | 'clash'

export const THEME_IDS: ThemeId[] = [
  'light',
  'dark',
  'sepia',
  'contrast',
  'ocean',
  'clash',
]

export const THEME_LABELS: Record<ThemeId, string> = {
  light: 'Light',
  dark: 'Dark',
  sepia: 'Sepia',
  contrast: 'High contrast',
  ocean: 'Ocean',
  clash: 'Clash',
}

/** Preview swatch colors (match index.css theme tokens). */
export const THEME_PREVIEWS: Record<
  ThemeId,
  { bg: string; accent: string; text: string; border: string }
> = {
  light: {
    bg: '#fff',
    accent: '#aa3bff',
    text: '#08060d',
    border: '#e5e4e7',
  },
  dark: {
    bg: '#16171d',
    accent: '#c084fc',
    text: '#f3f4f6',
    border: '#2e303a',
  },
  sepia: {
    bg: '#f4ecd8',
    accent: '#8b5e3c',
    text: '#2c2416',
    border: '#d4c4a8',
  },
  contrast: {
    bg: '#fff',
    accent: '#0000ee',
    text: '#000',
    border: '#000',
  },
  ocean: {
    bg: '#e8f7f5',
    accent: '#0d9488',
    text: '#134e4a',
    border: '#99d5cd',
  },
  clash: {
    bg: '#fff0f6',
    accent: '#16a34a',
    text: '#14532d',
    border: '#f472b6',
  },
}

export type SettingsAudience = 'public' | 'user' | 'admin'

export interface SettingsItem {
  id: string
  label: string
  render: () => ReactNode
}
