# Monorepo layout

## Key files

| Path | What |
|------|------|
| `back/` | .NET 10 Web API |
| `client/` | React 19 + TypeScript + Vite 8 SPA |
| `devops/` | Docker Compose (Postgres), DB migrations |
| `devops/migrations/migrations/` | SQL migration files (`node-pg-migrate`) |
| `back/back.http` | HTTP request examples |

## Stack

- Backend: .NET 10, Npgsql, OpenAPI
- Frontend: React 19, Vite 8
- Database: PostgreSQL 17 (Docker)

## Ports

| Service | URL |
|---------|-----|
| Backend HTTP | `http://localhost:5286` |
| Backend HTTPS | `https://localhost:7221` |
| Postgres | `localhost:5432` |
| Vite dev server | `http://localhost:5173` (default) |

## Run commands

```bash
# Postgres
cd devops && docker compose up -d

# Backend (https profile: 7221 + 5286)
cd back && dotnet run

# Frontend
cd client && npm run dev

# DB migrations (after Postgres is up)
cd devops/migrations && npm install && npm run migrate
```

Vite proxies `/api` → `https://localhost:7221` (see `client/vite.config.ts`).

## Do / Don't

- Do: use `back.http` or Postman collection in `devops/postman/` for API tests
- Don't: commit `devops/.env` or real secrets
