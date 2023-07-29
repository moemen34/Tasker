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


 <img width="1181" alt="pic2" src="https://github.com/moemen34/Tasker/assets/96449074/bbd35184-5b4d-441f-ac05-1e96ea4d8621">

 <img width="141" alt="pic1" src="https://github.com/moemen34/Tasker/assets/96449074/14c2ccf4-141b-472a-a776-640985bf4348">



### Inserting relationships:
Similarly, the admin has the ability to upload an excel file with employee relations, the file would have 3 columns (sourceID, destinationID, and relation). Such that the source and destination are the IDs of the employees.  
Once uploaded, the equivalent edges are added to the OpenFGA store.


<img width="1187" alt="pic3" src="https://github.com/moemen34/Tasker/assets/96449074/ee9e01eb-f298-45a9-974a-25cdf1a2c280">
<img width="140" alt="pic4" src="https://github.com/moemen34/Tasker/assets/96449074/934f2e2b-1230-4e99-b896-c537c4eb3bf6">


### Viewing task folders:
The webApp also allows the user to view task folders of their employees or boss(someone they assist). This is achieved by using a **List Check** from OpenFGA, this returns a list of all users with relationship *supervisor_plus* to the logged in user, or *assistant*.

<img width="1175" alt="pic5" src="https://github.com/moemen34/Tasker/assets/96449074/4cc7749d-8cdf-4e3a-a45e-ae9f92c5889c">
<img width="1147" alt="pic6" src="https://github.com/moemen34/Tasker/assets/96449074/6335b2df-2701-4d1c-b626-3dbe74beb93f">


### Viewing tasks:
Users of the webapp can also view tasks they have the relationship *can_view* to as shown in the screenshot below.

<img width="1148" alt="pic7" src="https://github.com/moemen34/Tasker/assets/96449074/380b4782-64bb-4f7a-b46c-605e03ef45ba">


### assigning tasks:
Users also have the ability to assign tasks to users they have the relationship *can_assign* to their task folders.  
As shown in the screenshot below, when assigning a task, the user can choose to select from all the possible assignees.

<img width="470" alt="pic8" src="https://github.com/moemen34/Tasker/assets/96449074/3a7294c7-84ea-496d-a6dc-1068aff9290c">
