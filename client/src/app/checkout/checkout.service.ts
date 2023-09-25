import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { DeliveryMethod } from '../shared/models/deliveryMethods';
import { map } from 'rxjs';
import { Order, OrderToCreate } from '../shared/models/order';
import { BasketService } from '../basket/basket.service';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl=environment.apiUrl;

  constructor(private http: HttpClient, private basketService: BasketService) { }

  getDeliveryMethods(){
    return this.http.get<DeliveryMethod[]>(this.baseUrl + 'orders/deliveryMethods').pipe(
      map(dm => {
        return dm.sort((a, b)=> b.price - a.price)
      } ))
  }

  createOrder(order: OrderToCreate){
    return this.http.post<Order>(this.baseUrl + 'orders/createorder', order);
  }
}
