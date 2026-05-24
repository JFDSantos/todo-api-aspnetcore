import { Routes } from '@angular/router';
import { TaskFormComponent } from './components/task-form/task-form.component';
import { TaskListComponent } from './components/task-list/task-list.component';

export const routes: Routes = [
  { path: '', component: TaskListComponent },
  { path: 'criar', component: TaskFormComponent },
  { path: '**', redirectTo: '' }
];
