export interface Category {
  id: number;
  name: string;
  sortOrder: number;
  description?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCategory {
  name: string;
  sortOrder: number;
  description?: string;
  isActive?: boolean;
}

export interface UpdateCategory {
  name: string;
  sortOrder: number;
  description?: string;
  isActive: boolean;
}