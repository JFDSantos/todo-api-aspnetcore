import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Task, CreateTaskDto, UpdateTaskDto } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private apiUrl = 'http://localhost:5273/api/tasks';

  constructor(private http: HttpClient) { }

  // GET /api/tasks - Listar todas as tarefas
  getAllTasks(): Observable<Task[]> {
    return this.http.get<Task[]>(this.apiUrl);
  }

  // GET /api/tasks/{id} - Obter tarefa por ID
  getTaskById(id: number): Observable<Task> {
    return this.http.get<Task>(`${this.apiUrl}/${id}`);
  }

  // POST /api/tasks - Criar nova tarefa
  createTask(dto: CreateTaskDto): Observable<Task> {
    return this.http.post<Task>(this.apiUrl, dto);
  }

  // PUT /api/tasks/{id} - Atualizar tarefa
  updateTask(id: number, dto: UpdateTaskDto): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}`, dto);
  }

  // PUT /api/tasks/{id}/start - Iniciar tarefa
  startTask(id: number): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}/start`, {});
  }

  // PUT /api/tasks/{id}/complete - Concluir tarefa
  completeTask(id: number): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}/complete`, {});
  }

  // PUT /api/tasks/{id}/reopen - Reabrir tarefa
  reopenTask(id: number): Observable<Task> {
    return this.http.put<Task>(`${this.apiUrl}/${id}/reopen`, {});
  }

  // DELETE /api/tasks/{id} - Deletar tarefa
  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
