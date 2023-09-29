import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { OrdersService } from '../orders/orders.service';
import { Order } from 'src/app/shared/models/order';

@Component({
  selector: 'app-order-detailed',
  templateUrl: './order-detailed.component.html',
  styleUrls: ['./order-detailed.component.scss']
})

export class OrderDetailedComponent implements OnInit {
  order? : Order | null;
  orderStatusMap = {
    'PaymentReceived': 'Payment Received',
    'PaymentFailed': 'Payment Failed',
    'Pending': 'Pending'
  };

  constructor(private activatedRoute: ActivatedRoute,private bcService: BreadcrumbService,
    private ordersService: OrdersService){}
  
    ngOnInit(): void {
      this.getOrder();
  }

    getOrder(){
      const id = this.activatedRoute.snapshot.paramMap.get('id');

      if(id != null){
        this.ordersService.getOrderDetailed(+id).subscribe({
          next: order => {
            this.order = order;
            //this.bcService.set('@orderDetails', `Order# ${order.id} - ${order.status}`);
            this.bcService.set('@orderDetails', `Order# ${order.id} - ${this.orderStatusMap[order.status as keyof typeof this.orderStatusMap]}`);
          } 
      });
    }
  }
}

