// src/app/core/services/product.service.ts
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject } from 'rxjs';
import { Product, CreateProduct, UpdateProduct } from '../../shared/models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly root: string = 'product';
  private productsSubject = new BehaviorSubject<Product[]>([]);

  public products$ = this.productsSubject.asObservable();

  constructor(private readonly http: HttpClient) { }

  findAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`/api/${this.root}`);
  }

  findOneById(productId: number): Observable<Product> {
    return this.http.get<Product>(`/api/${this.root}/${productId}`);
  }

  findByCategory(categoryId: number): Observable<Product[]> {
    return this.http.get<Product[]>(`/api/${this.root}/category/${categoryId}`);
  }

  create(product: CreateProduct, imageFile?: File): Observable<Product> {
    if (imageFile) {
      const formData = new FormData();
      
      // Agregar datos del producto
      Object.keys(product).forEach(key => {
        const value = (product as any)[key];
        if (value !== undefined && value !== null) {
          formData.append(key, value.toString());
        }
      });
      
      formData.append('image', imageFile);
      
      return this.http.post<Product>(`/api/${this.root}`, formData);
    } else {
      return this.http.post<Product>(`/api/${this.root}`, product);
    }
  }

  update(id: number, product: UpdateProduct, imageFile?: File): Observable<Product> {
    if (imageFile) {
      const formData = new FormData();
      
      Object.keys(product).forEach(key => {
        const value = (product as any)[key];
        if (value !== undefined && value !== null) {
          formData.append(key, value.toString());
        }
      });
      
      // Agregar archivo de imagen
      formData.append('image', imageFile);
      
      return this.http.put<Product>(`/api/${this.root}/${id}`, formData);
    } else {
      return this.http.put<Product>(`/api/${this.root}/${id}`, product);
    }
  }

    validateImageFile(file: File): { isValid: boolean; error?: string } {
    const maxSize = 5 * 1024 * 1024; // 5MB
    const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif', 'image/webp'];
    
    if (!allowedTypes.includes(file.type)) {
      return {
        isValid: false,
        error: 'Tipo de archivo no válido. Solo se permiten: JPG, PNG, GIF, WebP'
      };
    }
    
    if (file.size > maxSize) {
      return {
        isValid: false,
        error: 'El archivo es demasiado grande. Máximo 5MB'
      };
    }
    
    return { isValid: true };
  }
}