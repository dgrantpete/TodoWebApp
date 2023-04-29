ROLLBACK;

BEGIN;

-- User procedures

CREATE OR REPLACE PROCEDURE create_user_account(email VARCHAR(255), password_hash VARCHAR(255), first_name VARCHAR(50), last_name VARCHAR(50))
BEGIN ATOMIC
    INSERT INTO user_account (email, password_hash, first_name, last_name)
    VALUES (email, password_hash, first_name, last_name);
END;

CREATE OR REPLACE PROCEDURE delete_user_account(user_account_id INT)
BEGIN ATOMIC
    DELETE FROM user_account
    WHERE id = user_account_id;
END;

-- Task procedures

CREATE OR REPLACE PROCEDURE set_task_completion(task_id INT, completed BOOLEAN)
BEGIN ATOMIC
    UPDATE task
    SET completed = completed
    WHERE id = task_id;
END;

CREATE OR REPLACE PROCEDURE create_task(title VARCHAR(50), description VARCHAR(255), user_account_id INT)
BEGIN ATOMIC
    INSERT INTO task (title, description, user_account_id)
    VALUES (title, description, user_account_id);
END;

CREATE OR REPLACE PROCEDURE delete_task(task_id INT)
BEGIN ATOMIC
    DELETE FROM task
    WHERE id = task_id;
END;

-- Functions

CREATE OR REPLACE FUNCTION get_user_credentials_by_email(account_email VARCHAR(255))
RETURNS TABLE(user_account_id INT, password_hash VARCHAR(255))
BEGIN ATOMIC
    SELECT id, password_hash
    FROM user_account
    WHERE user_account.email = account_email
    LIMIT 1;
END;

CREATE OR REPLACE FUNCTION get_user_tasks(user_account_id INT)
RETURNS TABLE(task_id INT, title VARCHAR(50), description VARCHAR(255), completed BOOLEAN, created TIMESTAMP(0), due TIMESTAMP(0))
BEGIN ATOMIC
    SELECT id, title, description, completed, created, due
    FROM task
    WHERE task.user_account_id = get_user_tasks.user_account_id;
END;

END;
