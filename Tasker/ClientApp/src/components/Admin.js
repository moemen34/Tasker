import React, { useState, useEffect } from 'react';
import { Container, Row, Col, Nav, NavItem, NavLink } from 'reactstrap';
import axios from "axios";

const Admin = () => {

    const [activeTab, setActiveTab] = useState('Add Employees');


    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    /////////

    const [selectedFile, setSelectedFile] = useState(null);

    const handleFileChange = (event) => {
        setSelectedFile(event.target.files[0]);
    };

    const handleRemoveFile = () => {
        setSelectedFile(null);
    };

    const handleUpload = async (e) => {
        // Handle file upload logic here
        const formData = new FormData();
        formData.append("FileName", selectedFile.name);
        formData.append("file", selectedFile);
        if (selectedFile) {
            // Perform upload operation
            //console.log('Selected file:', selectedFile);

            
                activeTab === "Add Employees" ? (
                    await axios.post('https://localhost:7293/api/employeesfileupload/', formData).then((response) => {
                        console.log(response);
                    })
                )
                : (
                        await axios.post('https://localhost:7293/api/RelationsFileUpload/', formData).then((response) => {
                        console.log(response);
                    })
                )

            // Reset selected file
            setSelectedFile(null);
        } else {
            console.log('No file selected');
        }
    };

    /////////

    return (
        <div>

            <Container className="bg-white border border-black">
                <Row className="bg-orange-400 pt-1">

                    <Nav>
                        <Col className="flex justify-center" xs="2">
                            <p className="font-bold text-lg p-1">Admin Panel: </p>
                        </Col>
                        <Col>
                            <NavItem>
                                <NavLink
                                    className={`hover:bg-gray-500 flex justify-center px-4 py-2 rounded-t-lg ${activeTab === 'Add Employees' ? 'bg-gray-700 text-white' : 'bg-gray-400 text-white'}`}
                                    onClick={() => handleTabClick('Add Employees')}
                                >
                                    Add Employees
                                </NavLink>
                            </NavItem>
                        </Col>
                        <Col>
                            <NavItem>
                                <NavLink
                                    className={`hover:bg-gray-500 flex justify-center px-4 py-2 rounded-t-lg ${activeTab === 'Add Relationships' ? 'bg-gray-700 text-white' : 'bg-gray-400 text-white'}`}
                                    onClick={() => handleTabClick('Add Relationships')}
                                >
                                    Add Relationship
                                </NavLink>
                            </NavItem>
                        </Col>
                    </Nav>
                </Row>


                <div className="flex items-center justify-center">
                    {activeTab === "Add Employees" ? (<div>Ulpoad excel sheet with Employees information:</div>)
                        : (<div>Ulpoad excel sheet with Employee relations:</div>)
                    }
                        
                        <label className="relative flex flex-col items-center px-4 py-6 bg-white rounded-lg shadow-md tracking-wide uppercase border border-blue cursor-pointer hover:bg-blue hover:text-white">

                            <span className="mt-2 text-base leading-normal">
                                {selectedFile ? selectedFile.name : 'Choose an Excel file'}
                            </span>
                            {selectedFile && (
                                <button
                                    className="absolute top-0 right-0 mr-2 mt-2 text-gray-500 hover:text-red-500 focus:outline-none"
                                    onClick={handleRemoveFile}
                                >
                                    <svg
                                        className="w-4 h-4"
                                        fill="currentColor"
                                        xmlns="http://www.w3.org/2000/svg"
                                        viewBox="0 0 20 20"
                                    >
                                        <path
                                            fillRule="evenodd"
                                            d="M10 2a8 8 0 100 16 8 8 0 000-16zm4.95 10.243l-1.414 1.414L10 11.414l-3.536 3.536-1.414-1.414L8.586 10 5.05 6.464l1.414-1.414L10 8.586l3.536-3.536 1.414 1.414L11.414 10l3.536 3.536z"
                                            clipRule="evenodd"
                                        />
                                    </svg>
                                </button>
                            )}
                            <input
                                type="file"
                                className="hidden"
                                onChange={handleFileChange}
                                accept=".xlsx,.xls"
                            />
                        </label>
                        {selectedFile != null ? (
                            <><>HGH</>
                                <button
                                    className="ml-4 px-4 py-2 text-white bg-sky-500 rounded-lg hover:bg-blue-dark"
                                    onClick={handleUpload}
                                >
                                    Upload
                                </button>
                            </>
                        ) : (<></>)
                        }
                    </div>

                    
                



            </Container>

        </div>
    )
}
export default Admin;