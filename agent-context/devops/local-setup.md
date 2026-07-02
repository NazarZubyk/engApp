# Local dev setup

PostgreSQL via Docker for local backend development.

## Key files

- `devops/docker-compose.yml` — Postgres service
- `devops/Dockerfile` — Postgres image build
- `devops/.env.example` — env var template
- `devops/migrations/` — `node-pg-migrate` project (config, `package.json`, `.env`)
- `devops/migrations/migrations/` — SQL migration files (e.g. `0001_auth.sql`)

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

**Run DB migrations:**
```bash
cd devops/migrations
cp .env.example .env   # if .env does not exist; set DATABASE_URL
npm install
npm run migrate
```

- Config: `migration.config.json` (JSON only — `node-pg-migrate` v8)
- Env: `DATABASE_URL=postgres://engapp:engapp@localhost:5432/engapp` in `devops/migrations/.env`
- New migration: `npm run create -- <name>` (creates file in `migrations/`)

## Do / Don't

- Do: keep `.env` out of git (already in `.gitignore`)
- Do: reference `.env.example` when documenting env vars
- Don't: put real passwords in agent context files
