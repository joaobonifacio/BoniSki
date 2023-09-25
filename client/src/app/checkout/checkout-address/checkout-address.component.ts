import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent {
  @Input() checkoutForm?: FormGroup;

  constructor(private accountService: AccountService, private matSnackBar: MatSnackBar){}

  saveUserAdress(){

      this.accountService.updateUserAddress(this.checkoutForm?.get('addressForm')?.value)
      .subscribe({
        next: ()  => {
          this.matSnackBar.open('Your address has been saved', 'Close', {
            duration: 5000,
            horizontalPosition: 'start'
          });

          this.checkoutForm?.get('addressForm')?.reset(this.checkoutForm?.get('addressForm')?.value);
        }
      });
  }
}
