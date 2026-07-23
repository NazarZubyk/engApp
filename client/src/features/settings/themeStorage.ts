import { THEME_IDS, type ThemeId } from './types'

const STORAGE_KEY = 'engapp-theme'
const DEFAULT_THEME: ThemeId = 'light'

export function loadTheme(): ThemeId {
  const raw = localStorage.getItem(STORAGE_KEY)
  if (raw && THEME_IDS.includes(raw as ThemeId)) {
    return raw as ThemeId
  }
  return DEFAULT_THEME
}

export function saveTheme(theme: ThemeId): void {
  localStorage.setItem(STORAGE_KEY, theme)
}

export function applyThemeToDocument(theme: ThemeId): void {
  document.documentElement.dataset.theme = theme
}
