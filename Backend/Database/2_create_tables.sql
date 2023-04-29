ROLLBACK;

BEGIN;

DROP TABLE IF EXISTS user_account CASCADE;

CREATE TABLE IF NOT EXISTS user_account
(
    id SERIAL PRIMARY KEY NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    created TIMESTAMP(0) NOT NULL DEFAULT NOW()
);

DROP TABLE IF EXISTS task CASCADE;

CREATE TABLE IF NOT EXISTS task
(
    id SERIAL PRIMARY KEY NOT NULL,
    title VARCHAR(50),
    description VARCHAR(255),
    completed BOOLEAN NOT NULL DEFAULT false,
    created TIMESTAMP(0) NOT NULL DEFAULT NOW(),
    due TIMESTAMP(0),
    user_account_id INT NOT NULL,
    FOREIGN KEY (user_account_id) REFERENCES user_account (id) ON DELETE CASCADE
);

END;
