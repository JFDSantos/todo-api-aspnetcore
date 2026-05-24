import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskService } from '../../services/task.service';
import { CreateTaskDto } from '../../models/task.model';

@Component({
  selector: 'app-task-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-form.component.html',
  styleUrls: ['./task-form.component.css']
})
export class TaskFormComponent implements OnInit {
  form: CreateTaskDto = {
    title: '',
    description: ''
  };

  loading = false;
  error: string | null = null;
  fieldErrors: { field: string; message: string }[] = [];
  success = false;

  constructor(private taskService: TaskService, private router: Router) { }

  ngOnInit(): void {
  }

  submitForm(): void {
    if (!this.form.title.trim()) {
      this.error = 'O título é obrigatório';
      return;
    }

    if (this.form.title.length > 100) {
      this.error = 'O título não pode ter mais de 100 caracteres';
      return;
    }

    this.loading = true;
    this.error = null;
    this.fieldErrors = [];
    this.success = false;

    this.taskService.createTask(this.form).subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.loading = false;
        if (err.error?.errors && Array.isArray(err.error.errors)) {
          this.fieldErrors = err.error.errors;
        } else {
          this.error = err.error?.message || 'Erro ao criar tarefa. Verifique os dados.';
        }
      }
    });
  }

  resetForm(): void {
    this.form = { title: '', description: '' };
    this.error = null;
    this.fieldErrors = [];
    this.success = false;
  }
}
