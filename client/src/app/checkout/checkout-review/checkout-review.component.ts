import { CdkStep, CdkStepper } from '@angular/cdk/stepper';
import { Component, Input } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-checkout-review',
  templateUrl: './checkout-review.component.html',
  styleUrls: ['./checkout-review.component.scss']
})
export class CheckoutReviewComponent {
  @Input() appStepper?: CdkStepper;

  constructor(private basketService: BasketService, private matSnackBar: MatSnackBar,
    private router: Router, private cdkStepper: CdkStepper){}

  createPaymentIntent(){
    console.log('checkout review payment intent início');
    this.basketService.createPaymentIntent().subscribe({
      next: () => {
        // this.matSnackBar.open('Payment intent created!', 'Close', { 
        //   horizontalPosition: 'left',
        //   duration: 5000 });
        
        this.appStepper?.next();
      },
      error: error => {
        this.matSnackBar.open(error.message, 'Close', { 
          horizontalPosition: 'left',
          duration: 5000 });
      }
    });
  }
}
