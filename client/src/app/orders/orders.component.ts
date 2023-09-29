import { Component, OnInit } from '@angular/core';
import { OrdersService } from './orders.service';
import { Order } from '../shared/models/order';
import { Router } from '@angular/router';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit{
  orders: Order[] = [];
  
  constructor(private ordersService: OrdersService, private router: Router){}

  ngOnInit(): void {
    this.getOrders()
  }
 
  getOrders(){
    this.ordersService.getOrdersForUser().subscribe({
       next: orders => {
        this.orders = orders;
      }
    });
  }
}
