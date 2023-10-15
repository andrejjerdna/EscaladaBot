CREATE TABLE problem_creator_state
(
    chat_id BIGINT PRIMARY KEY,
    trace_creator_state INTEGER NOT NULL,
    problem_id INTEGER NOT NULL,
    update_at DATETIME
);

CREATE TABLE problem
(
    id BIGINT PRIMARY KEY,
    file_id UUID NOT NULL,
    author VARCHAR(150) NOT NULL,
    timestamp DATETIME NOT NULL
);

CREATE TABLE subscribe_user
(
    chat_id BIGINT PRIMARY KEY,
    user_name VARCHAR(300)
);

CREATE TABLE admin
(
    chat_id BIGINT PRIMARY KEY,
    user_name VARCHAR(300)
);