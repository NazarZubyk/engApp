import type { AuthSession } from '../auth/types'
import { LogoutSettingsItem } from './LogoutSettingsItem'
import { ThemeSettingsItem } from './ThemeSettingsItem'
import type { SettingsItem } from './types'

/** Always visible (guests and signed-in users). */
export const publicItems: SettingsItem[] = [
  {
    id: 'theme',
    label: 'Theme',
    render: () => <ThemeSettingsItem />,
  },
]

/** Visible when authenticated (user or admin). */
export const userItems: SettingsItem[] = [
  {
    id: 'logout',
    label: 'Log out',
    render: () => <LogoutSettingsItem />,
  },
]

/** Visible when session.role === 'admin'. Empty — ready for future items. */
export const adminItems: SettingsItem[] = []

export function getSettingsItems(session: AuthSession | null): SettingsItem[] {
  const items = [...publicItems]
  if (session) items.push(...userItems)
  if (session?.role === 'admin') items.push(...adminItems)
  return items
}
