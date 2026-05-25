import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { Task, TaskStatus, TaskStatusLabel } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.css']
})
export class TaskListComponent implements OnInit {
  tasks = signal<Task[]>([]);
  loading = signal(false);
  error = signal<string | null>(null);
  actionLoading = signal<number | null>(null);
  deleteTarget = signal<Task | null>(null);

  // Filtro de status: null = todos, 0/1/2 = filtrado
  statusFilter = signal<number | null>(null);
  filteredTasks = computed(() => {
    const f = this.statusFilter();
    return f === null ? this.tasks() : this.tasks().filter(t => t.status === f);
  });

  // Modal de edição
  editTarget = signal<Task | null>(null);
  editViewOnly = signal(false);
  editTitle = '';
  editDescription = '';
  editCompletedAt = ''; // dd/MM/yyyy HH:mm (campo único com máscara)
  editCreatedAt = '';    // dd/MM/yyyy HH:mm (readonly, display)
  editLoading = signal(false);
  editError = signal<string | null>(null);

  constructor(private taskService: TaskService) { }

  ngOnInit(): void {
    this.loadTasks();
  }

  loadTasks(): void {
    this.loading.set(true);
    this.error.set(null);
    this.taskService.getAllTasks().subscribe({
      next: (data) => {
        this.tasks.set(data);
        this.loading.set(false);
      },
      error: () => {
        this.error.set('Erro ao carregar tarefas. Verifique se o servidor está rodando.');
        this.loading.set(false);
      }
    });
  }

  startTask(id: number): void {
    this.actionLoading.set(id);
    this.taskService.startTask(id).subscribe({
      next: (updated) => {
        this.updateTask(updated);
        this.actionLoading.set(null);
      },
      error: () => {
        this.error.set('Erro ao iniciar tarefa.');
        this.actionLoading.set(null);
      }
    });
  }

  completeTask(id: number): void {
    this.actionLoading.set(id);
    this.taskService.completeTask(id).subscribe({
      next: (updated) => {
        this.updateTask(updated);
        this.actionLoading.set(null);
      },
      error: () => {
        this.error.set('Erro ao concluir tarefa.');
        this.actionLoading.set(null);
      }
    });
  }

  reopenTask(id: number): void {
    this.actionLoading.set(id);
    this.taskService.reopenTask(id).subscribe({
      next: (updated) => {
        this.updateTask(updated);
        this.actionLoading.set(null);
      },
      error: () => {
        this.error.set('Erro ao reabrir tarefa.');
        this.actionLoading.set(null);
      }
    });
  }

  confirmDelete(task: Task): void {
    this.deleteTarget.set(task);
  }

  cancelDelete(): void {
    this.deleteTarget.set(null);
  }

  openEdit(task: Task): void {
    this.editViewOnly.set(task.status === 2);
    this.editTitle = task.title;
    this.editDescription = task.description ?? '';
    this.editCompletedAt = task.completedAt ? this.toDisplayDate(task.completedAt) : '';
    this.editCreatedAt = task.createdAt ? this.toDisplayDate(task.createdAt) : '';
    this.editError.set(null);
    this.editTarget.set(task);
  }

  cancelEdit(): void {
    this.editTarget.set(null);
    this.editViewOnly.set(false);
    this.editError.set(null);
  }

  saveEdit(): void {
    const task = this.editTarget();
    if (!task) return;
    if (!this.editTitle.trim()) { this.editError.set('O título é obrigatório.'); return; }
    const dtMatch = this.editCompletedAt.match(/^(\d{2})\/(\d{2})\/(\d{4}) (\d{2}):(\d{2})$/);
    if (this.editCompletedAt && !dtMatch) {
      this.editError.set('Preencha a data e hora de conclusão no formato dd/mm/aaaa HH:mm.');
      return;
    }
    this.editLoading.set(true);
    const completedAt = dtMatch
      ? `${dtMatch[3]}-${dtMatch[2]}-${dtMatch[1]}T${dtMatch[4]}:${dtMatch[5]}:00`
      : null;

    this.taskService.updateTask(task.id, {
      title: this.editTitle.trim(),
      description: this.editDescription.trim(),
      status: completedAt ? TaskStatus.Concluida : TaskStatus.EmProgresso,
      completedAt
    }).subscribe({
      next: (updated) => {
        this.updateTask(updated);
        this.editTarget.set(null);
        this.editLoading.set(false);
      },
      error: (ex) => {
        this.editError.set(ex.error?.message || 'Erro ao salvar tarefa. Verifique os dados.');
        this.editLoading.set(false);
      }
    });
  }

  deleteTask(id: number): void {
    this.deleteTarget.set(null);
    this.actionLoading.set(id);
    this.taskService.deleteTask(id).subscribe({
      next: () => {
        this.tasks.update(list => list.filter(t => t.id !== id));
        this.actionLoading.set(null);
      },
      error: () => {
        this.error.set('Erro ao deletar tarefa.');
        this.actionLoading.set(null);
      }
    });
  }

  applyDatetimeMask(event: Event): void {
    const input = event.target as HTMLInputElement;
    const digits = input.value.replace(/\D/g, '').substring(0, 12);
    let masked = digits.substring(0, 2);
    if (digits.length > 2)  masked += '/' + digits.substring(2, 4);
    if (digits.length > 4)  masked += '/' + digits.substring(4, 8);
    if (digits.length > 8)  masked += ' ' + digits.substring(8, 10);
    if (digits.length > 10) masked += ':' + digits.substring(10, 12);
    input.value = masked;
    this.editCompletedAt = masked;
  }

  private toDisplayDate(iso: string): string {
    return new Date(iso).toLocaleString('pt-BR', {
      timeZone: 'America/Sao_Paulo',
      day: '2-digit', month: '2-digit', year: 'numeric',
      hour: '2-digit', minute: '2-digit', hour12: false
    }).replace(',', '');
  }

  private updateTask(updated: Task): void {
    this.tasks.update(list => {
      const idx = list.findIndex(t => t.id === updated.id);
      if (idx === -1) return list;
      const next = [...list];
      next[idx] = updated;
      return next;
    });
  }

  getStatusLabel(status: number): string {
    return TaskStatusLabel[status as keyof typeof TaskStatusLabel] || 'Desconhecido';
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 0: return 'pendente';
      case 1: return 'progresso';
      case 2: return 'concluida';
      default: return '';
    }
  }

  pendentes = computed(() => this.tasks().filter(t => t.status === 0).length);
  emProgresso = computed(() => this.tasks().filter(t => t.status === 1).length);
  concluidas = computed(() => this.tasks().filter(t => t.status === 2).length);
}
