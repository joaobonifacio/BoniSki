import { Component, OnInit } from '@angular/core';
import { ShopService } from '../shop.service';
import { Product } from 'src/app/shared/models/product';
import { ActivatedRoute } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {
  
  product?: Product;
  productId? : number;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
    private bcService: BreadcrumbService){}

  ngOnInit(): void {

    this.loadProduct();
  }

  loadProduct(){

    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if(id) this.shopService.getProduct(+id).subscribe({
        next : response => { 
          this.product = response;
          this.bcService.set('@productDetails', this.product.name);
        },
        error : error => console.log(error) 
      });
  }
}