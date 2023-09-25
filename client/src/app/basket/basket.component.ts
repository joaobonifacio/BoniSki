import { Component, ViewEncapsulation } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../shared/models/Basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss'],
  encapsulation: ViewEncapsulation.None  // Add this line
})
export class BasketComponent {

  constructor(public basketService: BasketService){}

  incrementQuantity(item: BasketItem){
    this.basketService.addItemToBasket(item);
  }

  removeItem(event: {id: number, quantity?: number }){
    this.basketService.removeItemFromBasket(event.id, event.quantity);
  }
}
