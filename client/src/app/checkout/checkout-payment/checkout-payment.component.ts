import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';
import { BasketService } from 'src/app/basket/basket.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Basket } from 'src/app/shared/models/Basket';
import { Address } from 'src/app/shared/models/User';
import { Order } from 'src/app/shared/models/order';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent {
  @Input() checkoutForm?: FormGroup;

  constructor(private basketService: BasketService, private checkoutService: CheckoutService, 
    private matSnackBar: MatSnackBar, private router: Router){}

  submitOrder(){
    const basket = this.basketService.getCurrentBasketValue();

    if(!basket){
      return;
    }

    //Ainda não é a Order, é a OrderToCreate
    const orderToCreate = this.getOrderToCreate(basket);

    if(!orderToCreate){
      return
    }

    //Chama o serviço que chama a API
    return this.checkoutService.createOrder(orderToCreate).subscribe({
      next: order => {
        this.matSnackBar.open('Order created!', 'Close', { 
                horizontalPosition: 'left',
                duration: 5000 });

        this.basketService.deleteLocalBasket();
        
        const navigationExtras: NavigationExtras = {state: order};
        //this.router.navigate(['checkout/success'], navigationExtras);
        this.router.navigateByUrl('checkout/success', navigationExtras)

      }
    });
  }

  private getOrderToCreate(basket: Basket) {

    // get deliveryMethod
    const deliveryMethodValue = (this.checkoutForm?.get('deliveryForm')?.value);
    const deliveryMethodId = parseInt(deliveryMethodValue.deliveryMethod, 10);

    //ship to Address
    const shipToAddress = this.checkoutForm?.get('addressForm')?.value as Address;

    // Check if the form is valid before creating the order
    if (!deliveryMethodId || !shipToAddress){
      return;
    }

    //Return OrderToCreate
    return{
      deliveryMethodId: deliveryMethodId,
      basketId: basket.id,
      shipToAddress: shipToAddress
    }
  }
}
