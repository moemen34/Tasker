import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import TaskFolders from "./components/TaskFolders";
import Admin from "./components/Admin";
import TasksToView from "./components/TasksToView";

const AppRoutes = [
  {
    index: true,
    element: <Home/>
  },
  {
    path: '/counter',
    element: <Counter/>
  },
  {
    path: '/fetch-data',
    element: <FetchData/>
  },
  {
    path: '/task-folders',
    element: <TaskFolders/>
  },
  {
    path: '/admin',
    element: <Admin />
  },
  {
    path: '/view-tasks',
    element: <TasksToView/>
  }
];

export default AppRoutes;
