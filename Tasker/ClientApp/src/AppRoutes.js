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
