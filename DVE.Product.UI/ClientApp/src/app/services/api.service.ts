import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProductName, ProductDetail } from '../model/product';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ApiService {
    apiURL: string = environment.apiURL;
   
    constructor(private httpClient: HttpClient) { }

 
    public getProductNames() {
        
        return this.httpClient.get<ProductName[]>(`${this.apiURL}/product`);
    }
    public getProductById(id: string) {
        return this.httpClient.get<ProductDetail>(`${this.apiURL}/product/${id}`);
    }
}