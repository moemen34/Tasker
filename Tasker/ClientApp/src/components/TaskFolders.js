import React, { useState, useEffect} from 'react';
import { Container, Row, Col, Nav, NavItem, NavLink } from 'reactstrap';

const TaskFolders = () => {

    const [folders, setFolders] = useState([]);
    const employeeId = 7; //get from login
    const [activeTab, setActiveTab] = useState('Employees');

    useEffect(() => {
        fetch(`api/TaskFolder/${employeeId}`)
            .then((results) => {
                return results.json();
            })
            .then(data => {
                setFolders(data);
            })
    }, [])


    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    return (
        <div>

            <Container className="bg-white border border-black">
                <Row className="bg-orange-400 pt-1">
                                               
                    <Nav>
                        <Col className="flex justify-center" xs="2">
                            <p className="font-bold text-lg p-1">Task Folders of: </p>
                        </Col>
                        <Col>
                            <NavItem>
                                <NavLink
                                    className={`hover:bg-gray-500 flex justify-center px-4 py-2 rounded-t-lg ${activeTab === 'Employees' ? 'bg-gray-700 text-white' : 'bg-gray-400 text-white'}`}
                                    onClick={() => handleTabClick('Employees')}
                                >
                                    Employees
                                </NavLink>
                            </NavItem>
                        </Col>
                        <Col>
                            <NavItem>
                                <NavLink
                                    className={`hover:bg-gray-500 flex justify-center px-4 py-2 rounded-t-lg ${activeTab === 'Bosses' ? 'bg-gray-700 text-white' : 'bg-gray-400 text-white'}`}
                                    onClick={() => handleTabClick('Bosses')}
                                >
                                    Bosses
                                </NavLink>
                            </NavItem>
                        </Col>
                        </Nav>            
                </Row>

                {activeTab === "Employees" ?
                    <>

                        {folders != null ? (
                            <>
                                <Row className="p-3">
                                    {folders.map((item, index) => (
                                        <Col key={index} xs={12} sm={6} md={4} lg={3} className="flex justify-center">
                                            <div className="pb-4">
                                                <button className="hover:scale-125 inline-flex items-center relative bg-zinc-100 px-4 h-9" title={item.ownerEmail}>
                                                <div className="w-5 h-5 bg-orange-400"></div>
                                                <div className="ml-2 text-black text-base font-light">{item.ownerName} </div>
                                                </button>
                                            </div>
                                        
                                        </Col>
                                    ))}
                                </Row>
                            </>
                        ) : (
                            <>LOADING...</>
                        )}


                    </> : <> NO EMPLOYEES</>
                }

            </Container>

        </div>
    )
}
export default TaskFolders;