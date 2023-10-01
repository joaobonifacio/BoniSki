import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ShopService } from './shop.service';
import { Product } from '../shared/models/product';
import { Type } from '../shared/models/type';
import { Brand } from '../shared/models/brand';
import { ShopParams } from '../shared/models/ShopParams';

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
  shopParams: ShopParams;
  sortOptions =[
    { name : "Alphabetical", value: "name" },
    { name : "Price: Low to High", value: "priceAsc" },
    { name : "Price: High to Low ", value: "priceDesc" }
  ]
  totalCount = 0;
  
  constructor(private shopService: ShopService){
    this.shopParams = this.shopService.getShopParams();
  }

  ngOnInit(): void {
    this.getProducts();
    this.getTypes();
    this.getBrands();  
  }

  getProducts(){
    this.shopService.getProducts().subscribe({
      next: response => { 
          this.products = response.data;
          this.totalCount = response.count;
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
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onTypeSelected(typeId : number){
    const params = this.shopService.getShopParams();
    params.typeId = typeId;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onSortSelected(event : any){
    const params = this.shopService.getShopParams();
    params.sort = event.target.value;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onPageChanged(event: any){
    const params = this.shopService.getShopParams();
    //se a página mudar muda-se de página
    if(params.pageIndex != event.page){
      params.pageIndex = event.page;
      this.shopService.setShopParams(params);
      this.shopParams = params;
      this.getProducts();
    }
  }

  onSearch(){
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm?.nativeElement.value;
    params.pageIndex = 1;
    this.shopService.setShopParams(params);
    this.shopParams = params;
    this.getProducts();
  }

  onReset(){
    if(this.searchTerm) this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams;
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }
}
