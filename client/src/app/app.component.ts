import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Product } from './models/product';
import { Pagination } from './models/pagination';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Skinet';
  products : Product[] = [];
  pagination!: Pagination<Product>;

  constructor(private http: HttpClient)
  {}

  ngOnInit(): void {
    this.http.get<Pagination<Product[]>>('https://localhost:5001/api/products?pageSize=50')
    .subscribe({
      next: response => 
      {
        this.products = response.data //console.log(response),
        // this.pagination.count = response.count,
        // this.pagination.pageIndex = response.pageIndex,
        // this.pagination.pageSize = response.pageSize
      }, 
      error: error => console.log(error), // what to do if there is an error
      complete: () => {
        console.log('request completed');
      }
    })
  }
}
