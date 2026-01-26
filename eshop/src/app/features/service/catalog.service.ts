import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product, ProductPaged } from "src/app/features/productcard/product.type";
import { Filter } from "src/app/features/toggle/toggle.type";
import { DataService } from "./data.service";

@Injectable()
export class CatalogService {
  constructor(private readonly dataService: DataService) {}

  find(filtes: Filter): Observable<ProductPaged> {
    return this.dataService.get<ProductPaged>(filtes)
  }

  get(id: number): Observable<Product> {
    return this.dataService.getById<Product>(id);
  }
}
