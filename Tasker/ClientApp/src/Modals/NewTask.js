import React, { useRef, useState } from 'react';

const generateRandomName = () => {
    // Replace this with your logic to generate random names
    const names = [
        'John Doe', 'Jane Smith', 'Michael Johnson', 'Emily Brown',
        'David Williams', 'Sarah Jones', 'Christopher Davis', 'Jessica Miller',
        'Matthew Wilson', 'Samantha Taylor', 'Andrew Anderson', 'Elizabeth Martinez',
        'James Thompson', 'Ashley Thomas', 'Robert Hernandez', 'Lauren Moore',
        'William Jackson', 'Olivia White', 'Joseph Lee', 'Amanda Lewis',
        'Daniel Scott', 'Stephanie King', 'Charles Green', 'Megan Hall',
        'Anthony Allen', 'Rachel Adams', 'Richard Baker', 'Kimberly Cook',
        'Kevin Bennett', 'Heather Turner', 'Thomas Parker', 'Nicole Phillips',
        'Benjamin Cooper', 'Maria Evans', 'Samuel Stewart', 'Michelle Rivera',
        'Patrick Ward'
    ];
    const randomIndex = Math.floor(Math.random() * names.length);
    return names[randomIndex];
};

const NewTask = ({ onClose }) => {


    ////////////////////
    const [selectedItems, setSelectedItems] = useState([]);
    const allNames = [
        'John Doe', 'Jane Smith', 'Michael Johnson', 'Emily Brown', 'David Williams', 'Sarah Jones',
        'Christopher Davis', 'Jessica Miller', 'Matthew Wilson', 'Samantha Taylor', 'Andrew Anderson',
        'Elizabeth Martinez', 'James Thompson', 'Ashley Thomas', 'Robert Hernandez', 'Lauren Moore',
        'William Jackson', 'Olivia White', 'Joseph Lee', 'Amanda Lewis', 'Daniel Scott', 'Stephanie King',
        'Charles Green', 'Megan Hall', 'Anthony Allen', 'Rachel Adams', 'Richard Baker', 'Kimberly Cook',
        'Kevin Bennett', 'Heather Turner', 'Thomas Parker', 'Nicole Phillips', 'Benjamin Cooper',
        'Maria Evans', 'Samuel Stewart', 'Michelle Rivera', 'Patrick Ward'
    ];

    const handleCheckboxChange = (name) => {
        setSelectedItems(prevSelected => {
            if (prevSelected.includes(name)) {
                return prevSelected.filter(item => item !== name);
            } else {
                return [...prevSelected, name];
            }
        });
    };

    const chunkArray = (arr, size) => {
        const chunkedArray = [];
        for (let i = 0; i < arr.length; i += size) {
            chunkedArray.push(arr.slice(i, i + size));
        }
        return chunkedArray;
    };

    /*const renderNames = () => {
        const chunkedNames = chunkArray(allNames, 4);
        return chunkedNames.map((row, rowIndex) => (
            <div key={rowIndex} style={{ display: 'flex', marginBottom: '10px' }}>
                {row.map((name) => (
                    <div key={name} style={{ display: 'flex', alignItems: 'center', marginRight: '20px' }}>
                        <input
                            type="checkbox"
                            checked={selectedItems.includes(name)}
                            onChange={() => handleCheckboxChange(name)}
                        />
                        <span style={{ marginLeft: '5px' }}>{name}</span>
                    </div>
                ))}
            </div>
        ));
    };*/

    const renderNames = () => {
        const chunkedNames = chunkArray(filteredNames, 4);
        return chunkedNames.map((row, rowIndex) => (
            <div key={rowIndex} style={{ display: 'flex', marginBottom: '10px' }}>
                {row.map((name) => (
                    <div key={name} style={{ display: 'flex', alignItems: 'center', marginRight: '20px' }}>
                        <input
                            type="checkbox"
                            checked={selectedItems.includes(name)}
                            onChange={() => handleCheckboxChange(name)}
                        />
                        <span style={{ marginLeft: '5px' }}>{name}</span>
                    </div>
                ))}
            </div>
        ));
    };

    const [searchQuery, setSearchQuery] = useState('');

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value);
    };

    const filteredNames = allNames.filter(name =>
        name.toLowerCase().includes(searchQuery.toLowerCase())
    );




    const [date, setDate] = useState('');
    const dateInputRef = useRef(null);

    const handleChange = (e) => {
        setDate(e.target.value);
    };

    ////////////////////overflow-y-auto
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
                                    Modal Title
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


                                <div style={{ height: '200px', overflowY: 'scroll', border: '1px solid #ccc', padding: '10px' }}>
                                    {renderNames()}
                                </div>

                                <div style={{ marginTop: '20px' }}>
                                    <h2 className="text-1xl font-semibold">Selected assignees:</h2>
                                    {/*<ul>
                                        {selectedItems.map((item) => (
                                            <li key={item}>{item}</li>
                                        ))}
                                    </ul>*/}
                                    <>{selectedItems.join(", ")}</>
                                </div>

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
                                    onClick={onClose}
                                >
                                    Save Changes
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



