import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CreateSale, Sale, UpdateSale } from "../interfaces/sale.interface";

@Injectable({
  providedIn: 'root'
})
export class SaleService {
  root: string = 'sale';

  constructor(
    private readonly http: HttpClient
  ) { }

  findOne(saleId: number) {
    return this.http.get<Sale>(`/api/${this.root}/${saleId}`);
  }

  findAll() {
    return this.http.get<Sale[]>(`/api/${this.root}`);
  }

  create(createSaleDto: CreateSale) {
    return this.http.post<Sale>(`/api/${this.root}`, createSaleDto);
  }

  update(saleId: number, updateSaleDto: UpdateSale) {
    return this.http.put<Sale>(`/api/${this.root}/${saleId}`, updateSaleDto);
  }
}
