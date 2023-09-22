import { Component } from '@angular/core';
import { AccountService } from '../account.service';
import { User, UserDTO } from 'src/app/shared/models/User';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  
  returnUrl: string;

  constructor(public accountService: AccountService, private router: Router,
    private activatedRoute: ActivatedRoute){
      this.returnUrl = activatedRoute.snapshot.queryParams['returnUrl'] || '/shop'
    }

  loginForm = new FormGroup ({
    email: new FormControl('',[Validators.required, Validators.email]),
    password: new FormControl(' ', [Validators.required])
  });
  
  onSubmit(){
    this.accountService.login(this.loginForm.value).subscribe({
      next: () => this.router.navigateByUrl(this.returnUrl)
    }) 
   }
}
