// src/app/core/services/category.service.ts
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { Category, CreateCategory, UpdateCategory } from '../../shared/models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly root: string = 'category';
  private categoriesSubject = new BehaviorSubject<Category[]>([]);
  
  public categories$ = this.categoriesSubject.asObservable();

  constructor(private readonly http: HttpClient) {}

  findAll(): Observable<Category[]> {
    return this.http.get<Category[]>(`/api/${this.root}`);
  }

  findOneById(categoryId: number): Observable<Category> {
    return this.http.get<Category>(`/api/${this.root}/${categoryId}`);
  }

  create(category: CreateCategory): Observable<Category> {
    return this.http.post<Category>(`/api/${this.root}`, category);
  }

  update(categoryId: number, category: UpdateCategory): Observable<Category> {
    return this.http.put<Category>(`/api/${this.root}/${categoryId}`, category);
  }
}