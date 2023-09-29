import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';
import { BasketService } from 'src/app/basket/basket.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Basket } from 'src/app/shared/models/Basket';
import { Address } from 'src/app/shared/models/User';
import { NavigationExtras, Router } from '@angular/router';
import { Stripe, StripeCardCvcElement, StripeCardExpiryElement, StripeCardNumberElement, loadStripe } from '@stripe/stripe-js';
import { firstValueFrom } from 'rxjs';
import { OrderToCreate } from 'src/app/shared/models/order';


@Component({
  selector: 'app-checkout-payment',
  templateUrl: './checkout-payment.component.html',
  styleUrls: ['./checkout-payment.component.scss']
})
export class CheckoutPaymentComponent implements OnInit {
  @Input() checkoutForm?: FormGroup;
  @ViewChild('cardNumber') cardNumberElement? : ElementRef;
  @ViewChild('cardExpiry') cardExpiryElement? : ElementRef;
  @ViewChild('cardCvc') cardCVCElement? : ElementRef;
  stripe: Stripe | null = null;
  cardNumber? : StripeCardNumberElement;
  cardExpiry? : StripeCardExpiryElement;
  cardCvc? : StripeCardCvcElement;
  cardErrors: any;
  loading = false;
  cardNumberComplete = false;
  cardExpiryComplete = false;
  cardCvcComplete = false;
  username = "Miguel";


  constructor(private basketService: BasketService, private checkoutService: CheckoutService, 
    private matSnackBar: MatSnackBar, private router: Router){}

  ngOnInit(): void {
    loadStripe('pk_test_51Nv1WQCVzv1pkjPzUVVxoy73m95DWVMtyFRwnzN36qqANWLPDbPRw2YITmSbgPZvRJnPtgDbXDDqv2hNiyJj1YBl00hb3V9tks')
      .then(stripe =>{
        this.stripe = stripe;
        
        const elements = stripe?.elements();

        if(elements){

          this.cardNumber = elements.create('cardNumber');
          this.cardNumber.mount(this.cardNumberElement?.nativeElement);
          this.cardNumber.on('change', event => {

            this.cardNumberComplete = event.complete;

            if(event.error){
              this.cardErrors = event.error.message
            }
            else{
              this.cardErrors = null;
            }
          })

          this.cardExpiry = elements.create('cardExpiry');
          this.cardExpiry.mount(this.cardExpiryElement?.nativeElement);
          this.cardExpiry.on('change', event => {

            this.cardExpiryComplete = event.complete;

            if(event.error){
              this.cardErrors = event.error.message
            }
            else{
              this.cardErrors = null;
            }
          })

          this.cardCvc = elements.create('cardCvc');
          this.cardCvc.mount(this.cardCVCElement?.nativeElement);
          this.cardCvc.on('change', event => {

            this.cardCvcComplete = event.complete;

            if(event.error){
              this.cardErrors = event.error.message
            }
            else{
              this.cardErrors = null;
            }
          })
        }
      }
    )
  }

  get paymentFormComplete(){
    return this.checkoutForm?.get('paymentForm')?.valid && this.cardNumberComplete 
      && this.cardExpiryComplete && this.cardCvcComplete
  }

  async submitOrder(){
    this.loading = true;
    const basket = this.basketService.getCurrentBasketValue();

    if(!basket) throw new Error("Can't get Basket!");

    try{
      const createdOrder = await this.createOrder(basket); 
      const paymentResult = await this.confirmCardPaymentWithStripe(basket);

      if(paymentResult.paymentIntent){
        //Apaga o basket no redi e localmente
        this.basketService.deleteBasket(basket);        
        const navigationExtras : NavigationExtras = { state: createdOrder };
        this.router.navigate(['checkout/success'], navigationExtras);
      }
      else{
        this.matSnackBar.open('Error: ' + paymentResult.error.message, 'Close', { 
          horizontalPosition: 'left',
          duration: 5000 });
      }
    }
    catch(error: any){ //Aqui apanhamos os throws dos métodos auxiliares 
      console.log(error);

      this.matSnackBar.open('Error: ' + error.message, 'Close', { 
        horizontalPosition: 'left',
        duration: 5000 });
    } finally{
      this.loading = false;
    }
  }
  
  private async createOrder(basket: Basket | null) {
    if(!basket){
      throw new Error("Basket is null!");
    }

    const orderToCreate = this.getOrderToCreate(basket);

    //Chama o serviço que chama a API
    return firstValueFrom(this.checkoutService.createOrder(orderToCreate));
  }

  private async confirmCardPaymentWithStripe(basket: Basket | null) {
    if(!basket){
      throw new Error("Basket is null!");
    }

    //Vai à API do stripe confirmar se o pagamento foi feito
    const result = this.stripe?.confirmCardPayment(basket.clientSecret!, {
      payment_method: {
        card: this.cardNumber!,
        billing_details:{
          name: this.checkoutForm?.get('paymentForm')?.get('nameOnCard')?.value
        }
      }
    });

    if(!result){
      throw new Error('Problem attempting payment with Stripe!')
    }

    return result;
  }


  private getOrderToCreate(basket: Basket): OrderToCreate {

    // get deliveryMethod
    const deliveryMethodValue = (this.checkoutForm?.get('deliveryForm')?.value);
    const deliveryMethodId = parseInt(deliveryMethodValue.deliveryMethod, 10);

    //ship to Address
    const shipToAddress = this.checkoutForm?.get('addressForm')?.value as Address;

    // Check if the form is valid before creating the order
    if (!deliveryMethodId || !shipToAddress){
      throw new Error('Problem with basket');
    }

    //Return OrderToCreate
    return{
      deliveryMethodId: deliveryMethodId,
      basketId: basket.id,
      shipToAddress: shipToAddress
    }
  }
}
