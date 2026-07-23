import { useId, useState } from 'react'
import { useTheme } from './ThemeProvider'
import { THEME_IDS, THEME_LABELS, THEME_PREVIEWS, type ThemeId } from './types'

function ThemeIcon() {
  return (
    <svg
      className="settings-menu-action-icon"
      width="18"
      height="18"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      <circle cx="13.5" cy="6.5" r="2.5" />
      <circle cx="17.5" cy="10.5" r="2.5" />
      <circle cx="8.5" cy="7.5" r="2.5" />
      <circle cx="6.5" cy="12.5" r="2.5" />
      <path d="M12 22a8 8 0 0 0 8-8c0-3.5-2-6.5-4.5-8a7 7 0 1 0-9.5 10.5A8 8 0 0 0 12 22z" />
    </svg>
  )
}

function ThemePreviewSwatch({ themeId }: { themeId: ThemeId }) {
  const preview = THEME_PREVIEWS[themeId]

  return (
    <span
      className="settings-theme-swatch"
      style={{
        background: preview.bg,
        borderColor: preview.border,
      }}
      aria-hidden="true"
    >
      <span className="settings-theme-swatch-dot" style={{ background: preview.text }} />
      <span className="settings-theme-swatch-dot" style={{ background: preview.accent }} />
    </span>
  )
}

export function ThemeSettingsItem() {
  const { theme, setTheme } = useTheme()
  const [pickerOpen, setPickerOpen] = useState(false)
  const listId = useId()

  if (!pickerOpen) {
    return (
      <button
        type="button"
        className="settings-menu-action"
        aria-expanded={false}
        aria-controls={listId}
        onClick={() => setPickerOpen(true)}
      >
        <span>Theme</span>
        <ThemeIcon />
      </button>
    )
  }

  return (
    <div className="settings-theme">
      <button
        type="button"
        className="settings-menu-action settings-menu-action--active"
        aria-expanded={true}
        aria-controls={listId}
        onClick={() => setPickerOpen(false)}
      >
        <span>Theme</span>
        <ThemeIcon />
      </button>
      <ul className="settings-theme-list" id={listId} role="listbox" aria-label="Theme">
        {THEME_IDS.map((id) => (
          <li key={id} role="option" aria-selected={theme === id}>
            <button
              type="button"
              className={
                theme === id
                  ? 'settings-theme-option settings-theme-option--active'
                  : 'settings-theme-option'
              }
              onClick={() => {
                setTheme(id as ThemeId)
                setPickerOpen(false)
              }}
            >
              <span className="settings-theme-option-label">{THEME_LABELS[id]}</span>
              <ThemePreviewSwatch themeId={id} />
            </button>
          </li>
        ))}
      </ul>
    </div>
  )
}
