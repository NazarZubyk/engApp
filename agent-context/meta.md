# Context system

How `agent-context/` works — read this when changing or extending agent context files.

## Key files

| File | Role |
|------|------|
| `AGENTS.md` (repo root) | Auto-loaded router (~40 lines). Pointers only — no architecture detail. |
| `agent-context/_NAVIGATION.md` | Problem → file map. Agents read this to pick one topic file. |
| `agent-context/_INDEX.md` | Catalog of all context files. |
| `agent-context/_TEMPLATE.md` | Copy when adding a new topic file. |
| `agent-context/GUIDE.md` | **Human only** — do not read unless user attaches it. |
| `.cursor/rules/agent-behavior.mdc` | Always-on behavior (commits, laconic replies, critical flags). |

## Folder layout

```
agent-context/
├── meta.md              ← this file
├── _NAVIGATION.md
├── _INDEX.md
├── _TEMPLATE.md
├── GUIDE.md             (humans)
├── conventions.md
├── monorepo.md
├── back/
├── client/
├── devops/
└── features/
    ├── _TEMPLATE.md
    └── {feature-name}.md
```

## Lean format (topic files)

- No YAML frontmatter — navigation lives in `_NAVIGATION.md` only
- `# Title` + optional one-line subtitle
- Sections: `Key files` → `Patterns` (or domain sections) → `Do / Don't`
- Short, factual, paths + snippets — no tutorials

## Patterns

**Add a new topic file** (e.g. new area `agent-context/testing/`):
1. Copy `_TEMPLATE.md` → new file
2. Add row to `_NAVIGATION.md` and `_INDEX.md`
3. Add one row to `AGENTS.md` "What to read next" if agents need it often
4. Do not duplicate content from other files

**Add a feature context file:**
1. Copy `features/_TEMPLATE.md` → `features/{name}.md`
2. Add row to `_NAVIGATION.md` and `_INDEX.md`

**Change routing / hard rules:**
- `AGENTS.md` — only short pointers and global agent rules
- `agent-behavior.mdc` — only behavior (chat style, commits, critical flags)
- Facts about code → topic file in `agent-context/`, not AGENTS.md

**One fact, one file** — update the matching file; do not copy across AGENTS.md, GUIDE.md, and topic files.

## Do / Don't

- Do: keep `AGENTS.md` and rules small — they load every session (~400 tokens fixed)
- Do: put deep knowledge in topic files agents load on demand
- Do: update `_NAVIGATION.md` + `_INDEX.md` when adding any new context file
- Don't: put human explanations in agent topic files — use `GUIDE.md`
- Don't: add YAML frontmatter to topic files
- Don't: create one giant context file
