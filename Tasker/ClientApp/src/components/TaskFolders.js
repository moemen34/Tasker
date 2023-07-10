import React, { useState, useEffect} from 'react';


const TaskFolders = () => {

    const [folders, setFolders] = useState([]);
    const employeeId = 1; //get from login

    useEffect(() => {
        fetch(`api/TaskFolder/${employeeId}`)
            .then((results) => {
                return results.json();
            })
            .then(data => {
                setFolders(data);
            })
    }, [])


    const [activeTab, setActiveTab] = useState('blue');

    const handleTabClick = (tab) => {
        setActiveTab(tab);
    };

    return (
        <div>
            <h1>
                Task Folders
            </h1>
            {
                (folders != null) ? folders.map((item) => <h3>{item.ownerId},, { item.ownerEmail}</h3>) : <>LOADING...</>
            }

            <>
                {/*<div class="flex flex-wrap -mx-3 mb-5">
                    <div class="w-full max-w-full px-3 mb-6 sm:w-1/2 mx-auto">
                        <div class="relative flex flex-col min-w-0 break-words bg-green-500 border-0 bg-clip-border rounded-2xl mb-5 draggable">
                            <div class="px-9 pt-5 flex justify-between items-stretch flex-wrap min-h-[70px] pb-0 bg-transparent">
                                <div class="flex flex-col items-start justify-center m-2 ml-0 font-medium text-xl/normal text-dark">
                                    <span class="text-white text-5xl/none font-semibold mr-2 tracking-[-0.115rem]">92</span>
                                    <span class="pt-1 font-medium text-white/80 text-lg/normal">Completed Tasks</span>
                                </div>
                            </div>
                            <div class="flex items-end flex-auto py-8 pt-0 px-9 ">
                                <div class="flex flex-col items-center w-full mt-3">
                                    <div class="flex justify-between w-full mt-auto mb-2 font-semibold text-white/80 text-lg/normal">
                                        <span class="mr-4">12 Pending</span>
                                        <span>85%</span>
                                    </div>

                                    <div class="mx-3 rounded-2xl h-[8px] w-full bg-white/20">
                                        <div class="rounded-2xl bg-white w-[85%] h-[8px]"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="flex flex-wrap -mx-3 mb-5">
                    <div class="w-full max-w-full sm:w-3/4 mx-auto text-center">
                        <p class="text-sm text-slate-500 py-1">
                            Tailwind CSS Component from <a href="https://www.loopple.com/theme/riva-dashboard-tailwind?ref=tailwindcomponents" class="text-slate-700 hover:text-slate-900" target="_blank">Riva Dashboard</a> by <a href="https://www.loopple.com" class="text-slate-700 hover:text-slate-900" target="_blank">Loopple Builder</a>.
                        </p>
                    </div>
                </div>*/}




                <div className="bg-orange-500 p-1 pb-0">
                    <div className="flex">
                        <button
                            className={`flex-1 px-4 py-2 rounded-t-lg ${activeTab === 'blue' ? 'bg-blue-500 text-white' : 'bg-white text-blue-500'}`}
                            onClick={() => handleTabClick('blue')}
                        >
                            Blue Tab
                        </button>
                        <button
                            className={`flex-1 px-4 py-2 rounded-t-lg ${activeTab === 'green' ? 'bg-green-500 text-white' : 'bg-white text-green-500'}`}
                            onClick={() => handleTabClick('green')}
                        >
                            Green Tab
                        </button>
                    </div>
                </div>





            </>



        </div>
    )
}
export default TaskFolders;