-- Auth tables and default dev admin (username: admin, password: admin123)
-- Apply via: npm run migrate (from devops/migrations/)
-- Or manually: psql -h localhost -U engapp -d engapp -f devops/migrations/migrations/0001_auth.sql

CREATE TABLE IF NOT EXISTS admins (
    id            SERIAL PRIMARY KEY,
    username      VARCHAR(100) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
    id            SERIAL PRIMARY KEY,
    login         VARCHAR(100) UNIQUE NOT NULL,
    password_hash TEXT NOT NULL,
    created_at    TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

INSERT INTO admins (username, password_hash)
VALUES (
    'admin',
    '$2y$11$UMNkbxvHX.xVyd.9qNK6.OyemYeEso4TXV/acOBG6zjOg9XOy45ne'
)
ON CONFLICT (username) DO NOTHING;
