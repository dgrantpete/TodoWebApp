ROLLBACK;

BEGIN;

-- Reset the SERIAL counter for user_account and task tables
ALTER SEQUENCE user_account_id_seq RESTART WITH 1;
ALTER SEQUENCE task_id_seq RESTART WITH 1;

TRUNCATE task CASCADE;
TRUNCATE user_account CASCADE;

-- Insert user_account test values using stored procedure
CALL create_user_account('user1@example.com', 'password_hash_1', 'John', 'Doe');
CALL create_user_account('user2@example.com', 'password_hash_2', 'Jane', 'Doe');
CALL create_user_account('user3@example.com', 'password_hash_3', 'Jim', 'Smith');
CALL create_user_account('user4@example.com', 'password_hash_4', 'Susan', 'Brown');
CALL create_user_account('user5@example.com', 'password_hash_5', 'Anna', 'Johnson');

-- Insert task test values using stored procedure
CALL create_task('Task 1', 'Task 1 description', 1);
CALL create_task('Task 2', 'Task 2 description', 1);
CALL create_task('Task 3', 'Task 3 description', 2);
CALL create_task('Task 4', 'Task 4 description', 2);
CALL create_task('Task 5', 'Task 5 description', 3);
CALL create_task('Task 6', 'Task 6 description', 3);
CALL create_task('Task 7', 'Task 7 description', 4);
CALL create_task('Task 8', 'Task 8 description', 4);
CALL create_task('Task 9', 'Task 9 description', 5);
CALL create_task('Task 10', 'Task 10 description', 5);

END;
