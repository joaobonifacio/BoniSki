import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';
import { BasketService } from 'src/app/basket/basket.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  
  product?: Product;
  productId? : number;
  //quantityOfItem = 0;
  quantity = 1;
  quantityInBasket = 0;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService, private basketService: BasketService){
      this.bcService.set('@productDetails', ' ')
    }

  ngOnInit(): void {

    this.loadProduct();
  }

  loadProduct(){

    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if(id) this.shopService.getProduct(+id).subscribe({
        next : response => { 
          this.product = response;
          this.bcService.set('@productDetails', this.product.name);
          if(this.product){
            this.basketService.basketSource$.pipe(take(1)).subscribe({
              next: basket => {
                const item = basket?.items.find(x=> x.id == +id);
                if(item){
                  this.quantity = item.quantity;
                  this.quantityInBasket = item.quantity;
                }
              }
            })
          }
        },
        error : error => console.log(error) 
      });
  }

  // numberOfProducts(){
  //   if(!this.product) this.quantityOfItem = 0;

  //   if(this.product != null && this.product != undefined)
  //   this.quantityOfItem = this.basketService.countItemInBasket(this.product);
  // }

  incrementQuantity(){
    this.quantity++;
  }

  decrementQuantity(){
    if(this.quantity > 0)
    this.quantity--;
  }

  updateBasket(){
    if(this.product){
      if(this.quantity > this.quantityInBasket){
        const itemsToAdd = this.quantity - this.quantityInBasket;
        this.quantityInBasket += itemsToAdd;
        this.basketService.addItemToBasket(this.product, itemsToAdd);
      }
      else{
        const itemsToRemove = this.quantityInBasket - this.quantity;
        this.quantityInBasket -= itemsToRemove;
        this.basketService.removeItemFromBasket(this.product.id, itemsToRemove);
      }
    }
  }

  get buttonText(){
    return this.quantityInBasket === 0 ? 'Add to Basket' : 'Update Basket';
  }
}