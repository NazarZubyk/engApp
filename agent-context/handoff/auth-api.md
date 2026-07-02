# Auth API

JWT login for admin and users; admin-only user CRUD.

## Implemented API

| Method | Path | Auth | Request body | Response |
|--------|------|------|--------------|----------|
| POST | `/api/auth/admin/login` | none | `{ "username", "password" }` | `{ "token", "expiresAt" }` |
| POST | `/api/auth/user/login` | none | `{ "login", "password" }` | `{ "token", "expiresAt" }` |
| GET | `/api/users` | Admin JWT | — | `[{ "id", "login", "createdAt" }]` |
| GET | `/api/users/{id}` | Admin JWT | — | `{ "id", "login", "createdAt" }` |
| POST | `/api/users` | Admin JWT | `{ "login", "password" }` | `201` + `UserDto` |
| PUT | `/api/users/{id}` | Admin JWT | `{ "login"?, "password"? }` | `UserDto` |
| DELETE | `/api/users/{id}` | Admin JWT | — | `204` |

Example admin login response:

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "expiresAt": "2026-07-02T22:00:00Z"
}
```

Example create user response:

```json
{
  "id": 1,
  "login": "testuser",
  "createdAt": "2026-07-02T21:00:00Z"
}
```

## Expected API

_Not applicable._

## Notes

- Auth: JWT Bearer (`Authorization: Bearer <token>`). Admin role required for `/api/users`.
- Default dev admin (after migration): `admin` / `admin123`
- Apply migration: `cd devops/migrations && npm run migrate` (runs `migrations/0001_auth.sql`)
- Postman: `devops/postman/engApp.postman_collection.json` — run **Auth** folder in order
- Errors: `401` bad/missing credentials, `403` wrong role, `404` not found, `409` duplicate login
