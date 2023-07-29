# Tasker
## Description:
Tasker is a prototype task assignment and management web application that allows it's users to assign tasks to each other deppending on their relationships.  
To achieve this Relationship Based Access Control (ReBAC) and authorization, OpenFGA, an open source authorization solution was used.  
The main purpose of this prototype application is to experiment with OpenFGA and it's solutions.
### Authorization Model:
The authorization model below defines 3 types (employee, task, and task_folder):
* The **employee** type has 3 relations, 2 of the relations (assistant and supervisor) are direct relations to other employees, while the last one (supervisor_plus) is an implied relation.
* The **task_folder** type also has three relations, but only one of (owner) is a direct relation, while the rest (can_assign and can_view) are implied relations.
* The **task** type, has 2 direct relations (assigner and parent_folder), one completely implied relation (assignee). ANd finaly, a can_view relation that is also implied, but can also be added as a direct relation.

``` py
model
  schema 1.1
type employee
  relations
    define assistant: [employee]
    define supervisor: [employee]
    define supervisor_plus: supervisor or supervisor_plus from supervisor
type task
  relations
    define assignee: owner from parent_folder
    define assigner: [employee]
    define can_view: [employee] or can_view from parent_folder or assigner or assistant from assigner
    define parent_folder: [task_folder]
type task_folder
  relations
    define can_assign: owner or supervisor_plus from owner
    define can_view: owner or assistant from owner
    define owner: [employee]
```

## Implementation:
### Inserting employees:  
On the webApp's navigation bar, there is an admin pannel that allows the upload of an excel file with employees information, the excel file would have 5 colums (first name, last name, email, phone number, and password). After uploading the employees, and **owner** edge between the employee and their task_folder is created in OpenFGA. 


 # pic1
 ![My Remote Image](https://github.com/moemen34/pics/blob/main/pic1.png)
 # pic2

### Inserting relationships:
Similarly, the admin has the ability to upload an excel file with employee relations, the file would have 3 columns (sourceID, destinationID, and relation). Such that the source and destination are the IDs of the employees.  
Once uploaded, the equivalent edges are added to the OpenFGA store.

# pic3
# pic4

### Viewing task folders:
The webApp also allows the user to view task folders of their employees or boss(someone they assist). This is achieved by using a **List Check** from OpenFGA, this returns a list of all users with relationship *supervisor_plus* to the logged in user, or *assistant*.

# pic5
# pic6

### Viewing tasks:
Users of the webapp can also view tasks they have the relationship *can_view* to as shown in the screenshot below.

# pic7

### assigning tasks:
Users also have the ability to assign tasks to users they have the relationship *can_assign* to their task folders.  
As shown in the screenshot below, when assigning a task, the user can choose to select from all the possible assignees.

# pic8
