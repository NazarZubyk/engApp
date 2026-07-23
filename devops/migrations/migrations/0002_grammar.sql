-- Grammar content and progress tables
-- Apply via: npm run migrate (from devops/migrations/)

CREATE TABLE IF NOT EXISTS grammar_topics (
    id              SERIAL PRIMARY KEY,
    parent_id       INT REFERENCES grammar_topics (id) ON DELETE SET NULL,
    slug            VARCHAR(200) UNIQUE NOT NULL,
    title           VARCHAR(500) NOT NULL,
    explanation_md  TEXT NOT NULL DEFAULT '',
    sort_order      INT NOT NULL DEFAULT 0,
    created_at      TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_grammar_topics_parent_id ON grammar_topics (parent_id);

CREATE TABLE IF NOT EXISTS grammar_exercises (
    id          SERIAL PRIMARY KEY,
    topic_id    INT NOT NULL REFERENCES grammar_topics (id) ON DELETE CASCADE,
    prompt      TEXT NOT NULL,
    sort_order  INT NOT NULL DEFAULT 0,
    notes       TEXT
);

CREATE INDEX IF NOT EXISTS idx_grammar_exercises_topic_id ON grammar_exercises (topic_id);

CREATE TABLE IF NOT EXISTS exercise_answer_slots (
    id          SERIAL PRIMARY KEY,
    exercise_id INT NOT NULL REFERENCES grammar_exercises (id) ON DELETE CASCADE,
    slot_index  INT NOT NULL,
    label       VARCHAR(100),
    UNIQUE (exercise_id, slot_index)
);

CREATE INDEX IF NOT EXISTS idx_exercise_answer_slots_exercise_id ON exercise_answer_slots (exercise_id);

CREATE TABLE IF NOT EXISTS accepted_answers (
    id              SERIAL PRIMARY KEY,
    slot_id         INT NOT NULL REFERENCES exercise_answer_slots (id) ON DELETE CASCADE,
    text            TEXT NOT NULL,
    normalized_text TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_accepted_answers_slot_id ON accepted_answers (slot_id);

CREATE TABLE IF NOT EXISTS mcq_distractors (
    id      SERIAL PRIMARY KEY,
    slot_id INT NOT NULL REFERENCES exercise_answer_slots (id) ON DELETE CASCADE,
    text    TEXT NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_mcq_distractors_slot_id ON mcq_distractors (slot_id);

CREATE TABLE IF NOT EXISTS study_sessions (
    id              SERIAL PRIMARY KEY,
    user_id         INT NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    topic_id        INT NOT NULL REFERENCES grammar_topics (id) ON DELETE CASCADE,
    answer_mode     VARCHAR(20) NOT NULL,
    goal_type       VARCHAR(20) NOT NULL DEFAULT 'none',
    goal_value      INT,
    started_at      TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    ended_at        TIMESTAMPTZ,
    total_attempts  INT NOT NULL DEFAULT 0,
    correct_count   INT NOT NULL DEFAULT 0,
    end_reason      VARCHAR(30)
);

CREATE INDEX IF NOT EXISTS idx_study_sessions_user_id ON study_sessions (user_id);
CREATE INDEX IF NOT EXISTS idx_study_sessions_topic_id ON study_sessions (topic_id);

CREATE TABLE IF NOT EXISTS exercise_attempts (
    id              SERIAL PRIMARY KEY,
    session_id      INT NOT NULL REFERENCES study_sessions (id) ON DELETE CASCADE,
    user_id         INT NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    exercise_id     INT NOT NULL REFERENCES grammar_exercises (id) ON DELETE CASCADE,
    answer_mode     VARCHAR(20) NOT NULL,
    responses_json  JSONB NOT NULL,
    is_correct      BOOLEAN NOT NULL,
    attempted_at    TIMESTAMPTZ NOT NULL DEFAULT NOW()
);

CREATE INDEX IF NOT EXISTS idx_exercise_attempts_session_id ON exercise_attempts (session_id);
CREATE INDEX IF NOT EXISTS idx_exercise_attempts_user_exercise ON exercise_attempts (user_id, exercise_id);

CREATE TABLE IF NOT EXISTS user_topic_progress (
    user_id         INT NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    topic_id        INT NOT NULL REFERENCES grammar_topics (id) ON DELETE CASCADE,
    total_attempts  INT NOT NULL DEFAULT 0,
    total_correct   INT NOT NULL DEFAULT 0,
    last_studied_at TIMESTAMPTZ,
    PRIMARY KEY (user_id, topic_id)
);

CREATE TABLE IF NOT EXISTS user_exercise_stats (
    user_id             INT NOT NULL REFERENCES users (id) ON DELETE CASCADE,
    exercise_id         INT NOT NULL REFERENCES grammar_exercises (id) ON DELETE CASCADE,
    attempt_count       INT NOT NULL DEFAULT 0,
    correct_count       INT NOT NULL DEFAULT 0,
    wrong_count         INT NOT NULL DEFAULT 0,
    last_attempted_at   TIMESTAMPTZ,
    last_is_correct     BOOLEAN,
    PRIMARY KEY (user_id, exercise_id)
);

CREATE INDEX IF NOT EXISTS idx_user_exercise_stats_user_id ON user_exercise_stats (user_id);
