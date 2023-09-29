import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, BasketItem, BasketTotals } from '../shared/models/Basket';
import { HttpClient } from '@angular/common/http';
import { Product } from '../shared/models/product';
import { DeliveryMethod } from '../shared/models/deliveryMethods';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = environment.apiUrl;

  private basketSource = new BehaviorSubject<Basket | null>(null);
  basketSource$ = this.basketSource.asObservable();

  private basketTotalSource = new BehaviorSubject<BasketTotals | null>(null);
  basketTotalSource$ = this.basketTotalSource.asObservable();
  itemProduct?: Product | BasketItem;

  constructor(private http: HttpClient) { }

  createPaymentIntent(){
    return this.http.post<Basket>(this.baseUrl + 'payments/' + 
      this.getCurrentBasketValue()?.id, {})
      .pipe(
        map(basket => {
          this.basketSource.next(basket);
        }
      )
    );
  }

  getBasket(id: string){
    return this.http.get<Basket>(this.baseUrl + 'basket?id=' + id).subscribe({
      next: basket => { 
        this.basketSource.next(basket);
        this.calculateTotal();
      }
    });
  }

  setBasket(basket : Basket){
    return this.http.post<Basket>(this.baseUrl + 'basket', basket).subscribe({
      next: basket => { 
        this.basketSource.next(basket);
        this.calculateTotal();
      }
    });
  }

  getCurrentBasketValue(){
    return this.basketSource.value;
  }
  
  addItemToBasket(item: Product | BasketItem, quantity? : number){

    //mapeamos o Product item para ItemToAdd, que é um BasketItem 
    if(this.isProduct(item)) item = this.mapProductItemToBasketItem(item);

    //Se houver basket vamos buscá-lo, senão criamos
    const basket = this.getCurrentBasketValue() ?? this.createBasket();

    //Se o basket tiver este product adicionamos quantity
    //senão adicionamos o product ao array de basket items na quantity passada
    basket.items = this.addOrUpdateItem(basket.items, item, 
      quantity == null || undefined ? 1 : quantity);

    //call API
    this.setBasket(basket);
  }

  removeItemFromBasket(id: number, quantity = 1){
    const basket = this.getCurrentBasketValue();

    if(!basket) return;

    const item = basket.items.find(x=>x.id==id);
    
    if(item){
      item.quantity -= quantity;
      if(item.quantity === 0){
        basket.items = basket.items.filter(x=>x.id !== id);
      }
      if(basket.items.length > 0){
        this.setBasket(basket);
      }
      else{
        this.deleteBasket(basket);
      }
    }
  }
  
  deleteBasket(basket: Basket) {
    return this.http.delete<Basket>(this.baseUrl + 'basket?id=' + basket.id)
    .subscribe({
      next: () => { 
        this.deleteLocalBasket();
      }
    });
  }

  deleteLocalBasket(){
    this.basketSource.next(null);
    this.basketTotalSource.next(null);
    localStorage.removeItem('basket_id');
  }

  private isProduct(item: Product | BasketItem) : item is Product{
    return (item as Product).productBrand !== undefined; 
  }

  private createBasket(): Basket  {
    const basket = new Basket();

    //Quando criamos um basket o ciud dá-lhe um id
    localStorage.setItem('basket_id', basket.id);

    return basket;
  }

  addOrUpdateItem(items: BasketItem[], itemToAdd: BasketItem, quantity: number): BasketItem[] {
    
    //Verificar se p array de BasketItem já tem este product id
    const item = items.find(x=>x.id===itemToAdd.id);

    //Update
    if(item) item.quantity += quantity;
    else{

      //Add item
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }

    return items;
  }
  
  private mapProductItemToBasketItem(item: Product) : BasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      brand: item.productBrand,
      type: item.productType,
      quantity: 0
    }
  }

  setShippingPrice(deliveryMethod: DeliveryMethod){
    const basket = this.getCurrentBasketValue();
    
    if(basket){
      basket.deliveryMethodId = deliveryMethod.id;
      basket.shippingPrice = deliveryMethod.price;
      this.setBasket(basket);
    }
  }

  private calculateTotal(){
    const basket = this.getCurrentBasketValue();

    if(!basket) return;

    const subtotal = basket.items.reduce((a, b)=> (b.price*b.quantity) + a, 0);
    const total = subtotal + basket.shippingPrice;

    this.basketTotalSource.next({shipping: basket.shippingPrice, total, subtotal});
  }

  countItemInBasket(product: Product){
    const basket = this.getCurrentBasketValue();

    const items = basket?.items;
    
    if(!items) return 0;

    const item = items.find(x=>x.id===product.id);

    if(!item) return 0;

    return item.quantity;
  }
}
