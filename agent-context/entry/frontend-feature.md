# Start: frontend feature

You are implementing **frontend for a feature**. The user attached this file — follow it end to end.

## You need from the user

- **Feature name** — kebab-case or PascalCase, e.g. `Words` / `words`
- **Description** — one sentence what the UI does
- **Optional** — pages, components, user flows

If missing, ask briefly before coding.

## Read first (minimal)

1. `agent-context/client/architecture.md`
2. `agent-context/conventions.md` — use `/api` prefix for API calls
3. If exists: `agent-context/handoff/{kebab-name}-api.md` — **Implemented API** section
4. If exists: `agent-context/features/{kebab-name}.md`

If handoff exists (backend went first): follow **Implemented API**. Do not change it.

If handoff does not exist (frontend goes first): you will create **Expected API** section.

Do not read all context files.

## Implement

1. Create UI under `client/src/features/{kebab-name}/` (or match existing client patterns)
2. Call API via `/api/...` paths (Vite proxy — see `client/architecture.md`)
3. Add types matching API contract from handoff or your Expected API

Keep changes minimal. Update `client/architecture.md` only if you introduce a new global pattern (routing, API client).

## Before you finish (mandatory — for the next agent)

Do not skip. Write files, not a long chat summary.

1. **`agent-context/features/{kebab-name}.md`**
   - Create from `features/_TEMPLATE.md` or update existing file
   - Fill **Frontend** section: key files, routes, components
   - **Backend** section: link to handoff or `Not yet`

2. **`agent-context/handoff/{kebab-name}-api.md`**
   - If backend already filled **Implemented API** — do not overwrite; verify UI matches it
   - If frontend goes first — copy `handoff/_TEMPLATE-api.md`, fill **Expected API** (methods, paths, request/response shapes backend must build)
   - Leave **Implemented API** as `Not implemented yet`

3. **Update navigation**
   - Append rows to `_NAVIGATION.md` and `_INDEX.md` if `features/{kebab}.md` or handoff are new

## Do / Don't

- Do: make **Expected API** concrete enough for a backend agent to implement without guessing
- Do: use `/api` paths in handoff (not `localhost:7221`)
- Don't: overwrite **Implemented API** when backend already shipped
- Don't: edit `AGENTS.md` per feature
- Don't: commit unless the user asks
