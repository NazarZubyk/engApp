# Settings

Site-wide settings gear (theme and future options) with role-filtered item registry.

## Backend

_Not applicable — client-only (localStorage)._

## Frontend

_Key files:_
- `client/src/features/settings/ThemeProvider.tsx` — theme state + `data-theme` on `html`
- `client/src/features/settings/themeStorage.ts` — `engapp-theme` in `localStorage`
- `client/src/features/settings/settingsRegistry.tsx` — `publicItems` / `userItems` / `adminItems`
- `client/src/features/settings/SettingsMenu.tsx` — fixed top-right gear + dropdown
- `client/src/features/settings/ThemeSettingsItem.tsx` — theme select
- `client/src/features/settings/LogoutSettingsItem.tsx` — log out (auth users)
- `client/src/features/settings/types.ts` — `ThemeId`, `SettingsItem`
- `client/src/index.css` — `[data-theme="light|dark|sepia|contrast|ocean|clash"]` tokens
- Mounted in `client/src/App.tsx` inside `AuthProvider` (all routes, including `/login`)

_Themes:_ `light`, `dark`, `sepia`, `contrast`, `ocean`, `clash` (default `light`).

_Registry audiences (code-level only — UI is a flat list):_
- `publicItems` — always shown
- `userItems` — when authenticated (includes Log out)
- `adminItems` — when `session.role === 'admin'`

_Add a settings item:_ push a `SettingsItem` into the matching array in `settingsRegistry.tsx`.

## Handoff

API contract: `handoff/settings-api.md` (no HTTP API).

## Do / Don't

- Do: put new settings entries in the correct registry array
- Do: keep the dropdown flat — no visible Public/User/Admin section labels
- Don't: mount `SettingsMenu` per page — it stays in `App.tsx`
