import { useEffect, useId, useRef, useState } from 'react'
import { useAuth } from '../auth/AuthProvider'
import { getSettingsItems } from './settingsRegistry'
import './SettingsMenu.css'

const PANEL_MS = 250

function GearIcon() {
  return (
    <svg
      width="20"
      height="20"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
      aria-hidden="true"
    >
      <path d="M12.22 2h-.44a2 2 0 0 0-2 2v.18a2 2 0 0 1-1 1.73l-.43.25a2 2 0 0 1-2 0l-.15-.08a2 2 0 0 0-2.73.73l-.22.38a2 2 0 0 0 .73 2.73l.15.1a2 2 0 0 1 1 1.72v.51a2 2 0 0 1-1 1.74l-.15.09a2 2 0 0 0-.73 2.73l.22.38a2 2 0 0 0 2.73.73l.15-.08a2 2 0 0 1 2 0l.43.25a2 2 0 0 1 1 1.73V20a2 2 0 0 0 2 2h.44a2 2 0 0 0 2-2v-.18a2 2 0 0 1 1-1.73l.43-.25a2 2 0 0 1 2 0l.15.08a2 2 0 0 0 2.73-.73l.22-.39a2 2 0 0 0-.73-2.73l-.15-.08a2 2 0 0 1-1-1.74v-.5a2 2 0 0 1 1-1.74l.15-.09a2 2 0 0 0 .73-2.73l-.22-.38a2 2 0 0 0-2.73-.73l-.15.08a2 2 0 0 1-2 0l-.43-.25a2 2 0 0 1-1-1.73V4a2 2 0 0 0-2-2z" />
      <circle cx="12" cy="12" r="3" />
    </svg>
  )
}

export function SettingsMenu() {
  const { session } = useAuth()
  const [open, setOpen] = useState(false)
  const [mounted, setMounted] = useState(false)
  const rootRef = useRef<HTMLDivElement>(null)
  const closeTimerRef = useRef<number | null>(null)
  const menuId = useId()
  const items = getSettingsItems(session)

  useEffect(() => {
    return () => {
      if (closeTimerRef.current !== null) {
        window.clearTimeout(closeTimerRef.current)
      }
    }
  }, [])

  useEffect(() => {
    if (!open) return

    const onPointerDown = (event: MouseEvent) => {
      if (rootRef.current && !rootRef.current.contains(event.target as Node)) {
        closePanel()
      }
    }

    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') closePanel()
    }

    document.addEventListener('mousedown', onPointerDown)
    document.addEventListener('keydown', onKeyDown)
    return () => {
      document.removeEventListener('mousedown', onPointerDown)
      document.removeEventListener('keydown', onKeyDown)
    }
  }, [open])

  function clearCloseTimer() {
    if (closeTimerRef.current !== null) {
      window.clearTimeout(closeTimerRef.current)
      closeTimerRef.current = null
    }
  }

  function openPanel() {
    clearCloseTimer()
    setMounted(true)
    requestAnimationFrame(() => {
      requestAnimationFrame(() => setOpen(true))
    })
  }

  function closePanel() {
    clearCloseTimer()
    setOpen(false)
    closeTimerRef.current = window.setTimeout(() => {
      setMounted(false)
      closeTimerRef.current = null
    }, PANEL_MS)
  }

  function togglePanel() {
    if (open) closePanel()
    else openPanel()
  }

  return (
    <div
      className={mounted ? 'settings-menu settings-menu--expanded' : 'settings-menu'}
      ref={rootRef}
    >
      {mounted ? (
        <div
          className={
            open
              ? 'settings-menu-panel settings-menu-panel--open'
              : 'settings-menu-panel'
          }
          id={menuId}
          role="menu"
        >
          <div className="settings-menu-panel-header">
            <div className="settings-menu-title">
              <svg
                className="settings-menu-title-mark"
                width="18"
                height="18"
                viewBox="0 0 18 18"
                fill="none"
                aria-hidden="true"
              >
                <path
                  d="M3 5.5h12M3 9h8M3 12.5h10"
                  stroke="currentColor"
                  strokeWidth="1.5"
                  strokeLinecap="round"
                />
              </svg>
              <span>Settings</span>
            </div>
            {/* spacer matches gear so title does not sit under it */}
            <span className="settings-menu-header-spacer" aria-hidden="true" />
          </div>
          <div className="settings-menu-body">
            {items.map((item) => (
              <div key={item.id} className="settings-menu-item" role="none">
                {item.render()}
              </div>
            ))}
          </div>
        </div>
      ) : null}

      <button
        type="button"
        className={
          open
            ? 'settings-menu-trigger settings-menu-trigger--active'
            : 'settings-menu-trigger'
        }
        aria-label="Settings"
        aria-expanded={open}
        aria-controls={menuId}
        onClick={togglePanel}
      >
        <GearIcon />
      </button>
    </div>
  )
}
