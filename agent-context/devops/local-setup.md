# Local dev setup

PostgreSQL via Docker for local backend development.

## Key files

- `devops/docker-compose.yml` — Postgres service
- `devops/Dockerfile` — Postgres image build
- `devops/.env.example` — env var template
- `devops/migrations/` — SQL init scripts (placeholder)

## Patterns

**Start Postgres:**
```bash
cd devops
cp .env.example .env   # if .env does not exist
docker compose up -d
```

**Default credentials** (match `back/appsettings.Development.json`):
- Host: `localhost`, Port: `5432`
- Database: `engapp`, User: `engapp`, Password: `engapp`

**Env vars** (`.env.example`): `POSTGRES_USER`, `POSTGRES_PASSWORD`, `POSTGRES_DB`, `POSTGRES_PORT`

Backend reads connection string from `DefaultConnection` in appsettings — not from Docker env directly.

## Do / Don't

- Do: keep `.env` out of git (already in `.gitignore`)
- Do: reference `.env.example` when documenting env vars
- Don't: put real passwords in agent context files
