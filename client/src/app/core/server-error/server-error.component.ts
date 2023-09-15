import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent {
  
  error: any;

  constructor(private router: Router) { 

    const navigaton = this.router.getCurrentNavigation();
    
    this.error = navigaton?.extras?.state?.['error'];

    // Now, you can use errorData in your component logic
    console.log("server error " + this.error);
  }

}
