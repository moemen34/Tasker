import React, { useRef, useState, useEffect } from 'react';
import axios from "axios";


const NewTask = ({ onClose }) => {



    const [selectedItems, setSelectedItems] = useState([]);
    const employeeId = 10;

    const [folders, setFolders] = useState([]);

    useEffect(() => {
        fetch(`api/TaskFolder?employeeId=${employeeId}&relation=3`)
            .then((results) => {
                return results.json();
            })
            .then((data) => {
                setFolders(data);
            });
    }, []);

    const [selectedOwnerIds, setSelectedOwnerIds] = useState([]);

    const handleCheckboxChange = (id) => {
        setSelectedItems((prevSelected) => {
            if (prevSelected.find((item) => item.ownerId === id)) {
                return prevSelected.filter((item) => item.ownerId !== id);
            } else {
                const newItem = folders.find((item) => item.ownerId === id);
                return [...prevSelected, newItem];
            }
        });
    };

    useEffect(() => {
        // When selectedItems changes, update selectedOwnerIds
        setSelectedOwnerIds(selectedItems.map(({ ownerId }) => ownerId));
    }, [selectedItems]);

    const chunkArray = (arr, size) => {
        const chunkedArray = [];
        for (let i = 0; i < arr.length; i += size) {
            chunkedArray.push(arr.slice(i, i + size));
        }
        return chunkedArray;
    };

    const renderNames = () => {
        const chunkedNames = chunkArray(filteredFolders, 4);
        return chunkedNames.map((row, rowIndex) => (
            <div key={rowIndex} style={{ display: 'flex', marginBottom: '10px' }}>
                {row.map(({ ownerId, ownerName }) => (
                    <div
                        key={ownerId}
                        style={{ display: 'flex', alignItems: 'center', marginRight: '20px' }}
                    >
                        <input
                            type="checkbox"
                            checked={selectedItems.some((item) => item.ownerId === ownerId)}
                            onChange={() => handleCheckboxChange(ownerId)}
                        />
                        <span style={{ marginLeft: '5px' }}>{ownerName}</span>
                    </div>
                ))}
            </div>
        ));
    };

    const [searchQuery, setSearchQuery] = useState('');

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value);
    };

    const filteredFolders = folders.filter(({ ownerName }) =>
        ownerName.toLowerCase().includes(searchQuery.toLowerCase())
    );

    const [date, setDate] = useState('');
    const dateInputRef = useRef(null);

    const [taskTitle, setTaskTitle] = useState('');


    const handleChange = (e) => {
        setDate(e.target.value);
    };

    const assignTask = async () => {
        console.log("HIIII");

        try {
            const url = 'https://localhost:7293/api/newtask'; 

            console.log("id: " + employeeId);
            const requestBody = {
                Assignees: selectedOwnerIds.map((item) => `'${item}'`),//wrap items in quotes ['12', '13', '14', '15'],
                AssignerID: employeeId,
                TaskTitle: taskTitle,
                DueDate: date
            };

            const response = await axios.post(url, requestBody);

            console.log('Response:', response.data);
        } catch (error) {
            console.error('Error:', error);
        }




        onClose();
    }


    return (
        <div>
            <>
                <div
                    className="justify-center items-center flex overflow-x-hidden overflow-y-auto fixed inset-0 z-50 outline-none focus:outline-none"
                >
                    <div className="relative w-auto my-6 mx-auto max-w-3xl">
                        {/*content*/}
                        <div className="border-0 rounded-lg shadow-lg relative flex flex-col w-full bg-white outline-none focus:outline-none">
                            {/*header*/}
                            <div className="flex items-start justify-between p-5 border-b border-solid border-slate-200 rounded-t">
                                <h3 className="text-3xl font-semibold">
                                    New Task
                                </h3>
                                <button
                                    className="p-1 ml-auto bg-transparent border-0 text-black opacity-5 float-right text-3xl leading-none font-semibold outline-none focus:outline-none"
                                    onClick={onClose}
                                >
                                    <span className="bg-transparent text-black opacity-5 h-6 w-6 text-2xl block outline-none focus:outline-none">
                                        ×
                                    </span>
                                </button>
                            </div>
                            {/*body*/}




                            {/* Task Title */}
                            <div className="mb-2 w-96">
                                <input
                                    type="text"
                                    placeholder="Task Title"
                                    value={taskTitle}
                                    onChange={(e) => setTaskTitle(e.target.value)}
                                    className="w-full px-3 py-1 border border-gray-300 rounded focus:outline-none focus:ring focus:ring-blue-500"
                                />
                            </div>






                            
                            <div className="relative p-6 flex-auto">
                                <h3 className="text-2xl font-semibold">Select Assignees: </h3>


                                <div style={{ marginBottom: '10px' }}>
                                    <input
                                        type="text"
                                        placeholder="Search for a name..."
                                        value={searchQuery}
                                        onChange={handleSearchChange}
                                    />
                                </div>


                                <div className="h-48 border border-gray-300 overflow-y-scroll p-2">
                                    {renderNames()}
                                </div>

                                {/* Selected Assignees */}
                                <div className="mt-4">
                                    <h2 className="text-lg font-semibold">Selected assignees:</h2>
                                    <p className="mt-1">{selectedItems.map(({ ownerName }) => ownerName).join(", ")}</p>
                                </div>

                                {/* Selected ownerId values */}
                                {/*<div className="mt-4">
                                    <h2 className="text-lg font-semibold">Selected ownerId values:</h2>
                                    <p className="mt-1">{selectedOwnerIds.join(", ")}</p>
                                </div>*/}


                                <br/>

                                <h2 className="text-1xl font-semibold">Due date:</h2>
                                <input
                                    type="date"
                                    onChange={handleChange}
                                    ref={dateInputRef}
                                />


                            </div>






                            {/*footer*/}
                            <div className="flex items-center justify-end p-6 border-t border-solid border-slate-200 rounded-b">
                                <button
                                    className="text-red-500 background-transparent font-bold uppercase px-6 py-2 text-sm outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                                    type="button"
                                    onClick={onClose}
                                >
                                    Close
                                </button>
                                <button
                                    className="bg-emerald-500 text-white active:bg-emerald-600 font-bold uppercase text-sm px-6 py-3 rounded shadow hover:shadow-lg outline-none focus:outline-none mr-1 mb-1 ease-linear transition-all duration-150"
                                    type="button"
                                    onClick={assignTask}
                                >
                                    assign Task
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="opacity-25 fixed inset-0 z-40 bg-black"></div>
            </>
        </div>
    );
};

export default NewTask;



