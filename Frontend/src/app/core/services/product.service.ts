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

  constructor(private readonly http: HttpClient) {}

  findAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`/api/${this.root}`);
  }

  findOneById(productId: number): Observable<Product> {
    return this.http.get<Product>(`/api/${this.root}/${productId}`);
  }

  create(product: CreateProduct): Observable<Product> {
    return this.http.post<Product>(`/api/${this.root}`, product);
  }

  update(productId: number, product: UpdateProduct): Observable<Product> {
    return this.http.put<Product>(`/api/${this.root}/${productId}`, product);
  }

  findByCategory(categoryId: number): Observable<Product[]> {
    return this.http.get<Product[]>(`/api/${this.root}/category/${categoryId}`);
  }
}