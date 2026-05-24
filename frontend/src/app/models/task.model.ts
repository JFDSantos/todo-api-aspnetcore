export interface Task {
  id: number;
  title: string;
  description: string;
  createdAt: string;
  completedAt: string | null;
  status: number; // 0 = Pendente, 1 = EmProgresso, 2 = Concluída
}

export interface CreateTaskDto {
  title: string;
  description: string;
}

export interface UpdateTaskDto {
  title: string;
  description: string;
  status: number;
  createdAt?: string | null;
  completedAt: string | null;
}

export enum TaskStatus {
  Pendente = 0,
  EmProgresso = 1,
  Concluida = 2
}

export const TaskStatusLabel: { [key in TaskStatus]: string } = {
  [TaskStatus.Pendente]: '⏳ Pendente',
  [TaskStatus.EmProgresso]: '⚙️ Em Progresso',
  [TaskStatus.Concluida]: '✅ Concluída'
};
