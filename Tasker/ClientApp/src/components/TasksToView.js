import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Nav, NavItem, NavLink } from 'reactstrap';
import { Table } from 'reactstrap';

const TaskFolders = () => {

    const [tasks, setTasks] = useState([]);
    const employeeId = 10; //get from login
    
    //const relation = 1;

    useEffect(() => {

        fetch(`api/CanViewTask?employeeId=${employeeId}`)
            .then((results) => {
                return results.json();
            })
            .then(data => {
                setTasks(data);
                console.log(data);
            })
            
    }, []);


    return (
        <>
            {<div className="container mt-5">
                <Table striped bordered responsive className="table table-bordered">
                    <thead className="bg-gray-200">
                        <tr>
                            <th>Task Title</th>
                            <th>Assigner</th>
                            <th>Assigned On</th>
                            <th>Due Date</th>
                            <th>Complete</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tasks.map((task, index) => (
                            <tr key={index}>
                                <td>{task.taskTitle}</td>
                                <td>{task.assigner}</td>
                                <td>{task.assignedOn}</td>
                                <td>{task.dueOn}</td>
                                <td>{task.complete ? 'Yes' : 'No'}</td>
                            </tr>
                        ))}
                    </tbody>
                </Table>
            </div>}
        </>
    )
}
export default TaskFolders;