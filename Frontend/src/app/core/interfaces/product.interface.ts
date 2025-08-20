export interface Product {
  id: number;
  categoryId: number;
  name: string;
  sortOrder: number;
  description: string;
  price: number;
  image: string;
  isActive: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface CreateProduct {
  categoryId: number;
  name: string;
  sortOrder: number;
  description: string;
  price: number;
  image: string;
  isActive?: boolean;
}

export interface UpdateProduct {
  categoryId: number;
  name: string;
  sortOrder: number;
  description: string;
  price: number;
  image: string;
  isActive: boolean;
}