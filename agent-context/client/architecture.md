# Client architecture

React SPA — API calls go through Vite dev proxy (`/api` → backend).

## Key files

- `client/src/main.tsx` — entry point
- `client/src/App.tsx` — root component
- `client/vite.config.ts` — dev server and `/api` proxy

## Patterns

**API proxy** (`vite.config.ts`):
```typescript
server: {
  proxy: {
    '/api': 'https://localhost:7221',
  },
},
```

Frontend fetches `/api/...` — Vite forwards to the backend HTTPS port.

**Stack:** React 19, TypeScript, Vite 8. No routing or state library yet.

## Do / Don't

- Do: use `/api/...` paths in fetch calls (works with proxy in dev)
- Do: update this file when adding routing, API client, or state management
- Don't: hardcode `localhost:7221` in frontend code — use `/api` prefix
