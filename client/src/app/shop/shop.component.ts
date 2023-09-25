import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ShopService } from './shop.service';
import { Product } from '../shared/models/product';
import { Type } from '../shared/models/type';
import { Brand } from '../shared/models/brand';
import { shopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  @ViewChild('search') searchTerm? :ElementRef;

  products : Product[] = [];
  types: Type[] = [];
  brands: Brand[] = [];
  shopParams = new shopParams();
  sortOptions =[
    { name : "Alphabetical", value: "name" },
    { name : "Price: Low to High", value: "priceAsc" },
    { name : "Price: High Low Low ", value: "priceDesc" }
  ]
  totalCount = 0;
  
  constructor(private shopService: ShopService){}

  ngOnInit(): void {
    this.getProducts();
    this.getTypes();
    this.getBrands();  
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
      next: response => { 
        if(response == null){
        this.totalCount = 0;
        }
        else{
          this.products = response.data;
          this.shopParams.pageIndex = response.pageIndex;
          this.shopParams.pageSize = response.pageSize;
          this.totalCount = response.count;
          console.log(this.totalCount) 
        }  
     },
      error: error => console.log(error)
    });
  }

  getTypes(){
    this.shopService.getTypes().subscribe({
      next: response => this.types = [{ id: 0, name:'All'}, ...response],
      error: error => console.log(error)
    });
  }

  getBrands(){
    this.shopService.getBrands().subscribe({
      next: response => this.brands  = [{ id: 0, name:'All'}, ...response],
      error: error => console.log(error)
    });
  }

  onBrandSelected(brandId : number){
    this.shopParams.brandId = brandId;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onTypeSelected(typeId : number){
    this.shopParams.typeId = typeId;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onSortSelected(event : any)
  {
      this.shopParams.sort =  event.target.value;
      this.getProducts();
  }

  onPageChanged(event: any){
    //se a página mudar muda-se de página
    if(this.shopParams.pageIndex != event.page){
      this.shopParams.pageIndex = event.page;
      this.getProducts();
    }
  }

  onSearch(){
    this.shopParams.search = this.searchTerm?.nativeElement.value;
    this.shopParams.pageIndex = 1;
    this.getProducts();
  }

  onReset(){
    if(this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new shopParams;
    this.getProducts();
  }
}
