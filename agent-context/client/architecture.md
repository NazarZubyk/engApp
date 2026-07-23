# Client architecture

React SPA — API calls go through Vite dev proxy (`/api` → backend).

## Key files

- `client/src/main.tsx` — entry point
- `client/src/App.tsx` — router and route definitions
- `client/src/lib/apiClient.ts` — authenticated `fetch` helper
- `client/vite.config.ts` — dev server and `/api` proxy

## Patterns

**API proxy** (`vite.config.ts`):
```typescript
server: {
  proxy: {
    '/api': {
      target: 'https://localhost:7221',
      changeOrigin: true,
      secure: false, // ASP.NET dev HTTPS cert
    },
  },
},
```

Frontend fetches `/api/...` — Vite forwards to the backend HTTPS port.

**Authenticated API** (`apiClient.ts`):
```typescript
await apiFetch('/api/grammar/topics', { token: session.token })
```

**Stack:** React 19, TypeScript, Vite 8, React Router 7.

**Routing** (`App.tsx`):
- `/login` — sign in (user or admin)
- `/menu` — main menu (protected)
- `/grammar` — topic tree (protected)
- `/grammar/progress` — topic stats (protected)
- `/grammar/:slug` — topic detail + explanation (protected)
- `/admin/grammar/import` — bulk import (admin only)
- other paths → redirect to `/menu`

**Features:** `client/src/features/{kebab-name}/` — e.g. `auth/`, `grammar/`, `grammar-admin/`, `settings/`.

**Auth:** JWT stored in `localStorage` (`engapp-auth`). `AuthProvider` + `RequireAuth` guard protected routes. `RequireAdmin` for admin-only routes. Token expiry checked on load.

**Theme + settings:** `ThemeProvider` wraps routes in `App.tsx` and sets `html[data-theme]`. Tokens live in `index.css` under `[data-theme="light|dark|sepia|contrast|ocean|clash"]`. Preference in `localStorage` (`engapp-theme`). `SettingsMenu` is a fixed top-right overlay on all routes. Item visibility via `settingsRegistry.tsx` (`publicItems` / `userItems` / `adminItems`) — see `features/settings.md`.

## Do / Don't

- Do: use `/api/...` paths in fetch calls (works with proxy in dev)
- Do: add new feature UI under `client/src/features/{kebab-name}/`
- Do: use `apiFetch` with `session.token` for authenticated endpoints
- Do: add settings entries in `settingsRegistry.tsx` (correct audience array)
- Do: update this file when adding routing, API client, or state management
- Don't: hardcode `localhost:7221` in frontend code — use `/api` prefix
- Don't: remount `SettingsMenu` inside individual pages