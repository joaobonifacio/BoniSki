import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Type } from '../shared/models/type';
import { Brand } from '../shared/models/brand';

import { productsUrl } from '../consts/consts.module';
import { baseUrl } from '../consts/consts.module';
import { queryStringUrl } from '../consts/consts.module';
import { pageSizeUrl } from '../consts/consts.module';
import { equalsUrl } from '../consts/consts.module';
import { dashUrl } from '../consts/consts.module';
import { brandsUrl } from '../consts/consts.module';
import { typesUrl } from '../consts/consts.module';
import { shopParams } from '../shared/models/shopParams';


@Injectable({
  providedIn: 'root'
})
export class ShopService {

  constructor(private http: HttpClient){ }

    //https://localhost:5001/api/products?brandId=1&typeId=2
    getProducts(shopParams : shopParams){
    let params = new HttpParams();

    if(shopParams.brandId > 0) params = params.append('brandId', shopParams.brandId);
    if(shopParams.typeId > 0 ) params = params.append('typeId', shopParams.typeId);
    params = params.append('sort', shopParams.sort);
    params = params.append('pageIndex', shopParams.pageIndex);
    params = params.append('pageSize', shopParams.pageSize);
    if(shopParams.search) params = params.append('search', shopParams.search);


      //https://localhost:5001/api/products?brandId=1&typeId=2
    return this.http.get<Pagination<Product[]>>(baseUrl + productsUrl, { params });
  }

  getProduct(id: number){
    return this.http.get<Product>(baseUrl + productsUrl + dashUrl + id);
  }

  //https://localhost:5001/api/products/brands
  getTypes(){
    return this.http.get<Type[]>(baseUrl + productsUrl +  dashUrl 
      +  typesUrl);
  }

//https://localhost:5001/api/products/brands
getBrands(){
  return this.http.get<Brand[]>(baseUrl + productsUrl +  dashUrl 
    +  brandsUrl);
  }
}
