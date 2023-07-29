CREATE TABLE employee(
    employee_id SERIAL PRIMARY KEY,
    first_name  VARCHAR(50)  NOT NULL,
    last_name   VARCHAR(50)  NOT NULL,
    email       VARCHAR(255) NOT NULL,
    phone       VARCHAR(10),
    password    VARCHAR(255) NOT NULL
);

CREATE TABLE relation(
    source INT              NOT NULL,
    destination INT         NOT NULL,
    relation VARCHAR(255)   NOT NULL,
    FOREIGN KEY (source) REFERENCES employee (employee_id) on delete cascade,
    FOREIGN KEY (destination) REFERENCES employee (employee_id) on delete cascade
);

--can I add viewer relation in OpenFGA  GO BACK TO THIS

Create Table task(
    assigner INT NOT NULL, 
    task_id SERIAL,
    Complete BOOLEAN NOT NULL,
    assigned_on TIMESTAMP DEFAULT (NOW() AT TIME ZONE 'UTC') NOT NULL,	
    due_date TIMESTAMP NOT NULL,
    PRIMARY KEY (task_id),
    FOREIGN KEY (assigner) REFERENCES employee (employee_id) on delete cascade
);

Create Table task_info(
    task_id INT,
    assignee INT,
    status_per_assignee BOOLEAN NOT NULL,
    viewed BOOLEAN NOT NULL,
    FOREIGN KEY (task_id) REFERENCES task (task_id) on delete cascade,
    FOREIGN KEY (assignee) REFERENCES employee (employee_id) on delete cascade
); 

Create Table notification(
    employee_id INT,
    notification_content VARCHAR(255),
    assigned_on TIMESTAMP DEFAULT (NOW() AT TIME ZONE 'UTC') NOT NULL,
    viewed BOOLEAN,
    FOREIGN KEY (employee_id) REFERENCES employee (employee_id) on delete cascade
);



Create Table chat(

)

