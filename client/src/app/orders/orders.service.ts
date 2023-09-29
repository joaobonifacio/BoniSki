import { Injectable } from '@angular/core';
import { Order } from '../shared/models/order';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { AccountService } from '../account/account.service';


@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  baseUrl=environment.apiUrl;
  token?: string | null;

  constructor(private http: HttpClient, private accountService: AccountService) {}
  
  getOrdersForUser(){
    return this.http.get<Order[]>(`${this.baseUrl}orders/getorders`);
  }
  
  getOrderDetailed(id: number){
  
    const theUrl = this.baseUrl + 'orders/' + id;
    return this.http.get<Order>(theUrl);
  }
}
