import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Pagination } from '../shared/models/pagination';
import { Product } from '../shared/models/product';
import { Type } from '../shared/models/type';
import { Brand } from '../shared/models/brand';
import { productsUrl } from '../consts/consts.module';
import { baseUrl } from '../consts/consts.module';
import { dashUrl } from '../consts/consts.module';
import { brandsUrl } from '../consts/consts.module';
import { typesUrl } from '../consts/consts.module';
import { ShopParams } from '../shared/models/ShopParams';
import { Observable, map, of } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class ShopService {
  products: Product[] = [];
  types: Type[] = [];
  brands: Brand[] = [];
  pagination?: Pagination<Product[]>;
  shopParams = new ShopParams();
  productCache = new Map<string, Pagination<Product[]>>();


  constructor(private http: HttpClient){ }

    //https://localhost:5001/api/products?brandId=1&typeId=2
    getProducts(useCache = true): Observable<Pagination<Product[]>>{
      if(!useCache){
        this.productCache = new Map();
      }
      //Se for para usar cache e tivermos coisas na cache
      if(this.productCache.size > 0 && useCache){
        if(this.productCache.has(Object.values(this.shopParams).join('-'))){
          //Object.values(this.shopParams) extrai os values de this.shopParams e forma um array c eles
          //Se shop params for brandId = 0; typeId = 0; sort = 'name'; pageIndex = 1; pageSize= 6; search = '';
          //'0-0-name-1-6-'seria a key de um value de pagination
          this.pagination = this.productCache.get(Object.values(this.shopParams).join('-'));

          if(this.pagination ){
            return of(this.pagination);
          }
        }
      }

      let params = new HttpParams();

      if(this.shopParams.brandId > 0) params = params.append('brandId', this.shopParams.brandId);
      if(this.shopParams.typeId > 0 ) params = params.append('typeId', this.shopParams.typeId);
      params = params.append('sort', this.shopParams.sort);
      params = params.append('pageIndex', this.shopParams.pageIndex);
      params = params.append('pageSize', this.shopParams.pageSize);
      if(this.shopParams.search) params = params.append('search', this.shopParams.search);


        //https://localhost:5001/api/products?brandId=1&typeId=2
      return this.http.get<Pagination<Product[]>>(baseUrl + productsUrl, { params }).pipe(
        map(response => {
          //Object.values(this.shopParams).join('-') é a key, response é o value
          this.productCache.set(Object.values(this.shopParams).join('-'), response);
          this.pagination = response;
          return response;
        })
      );
    }

    setShopParams(params: ShopParams){
      this.shopParams = params;
    }

    getShopParams(){
      return this.shopParams
    }

    getProduct(id: number){
      //Vamos percorrer todos os objectos à procura do product com id==id
      const product = [...this.productCache.values()]
        .reduce((acc, paginatedResult) => {
          return {...acc, ...paginatedResult.data.find(x=>x.id===id)}
        }, {} as Product);

      console.log(product);

      //Se tivermos esta key, retornamos o produto
      if(Object.keys(product).length !== 0){
        return of(product);
      }

      //Se não tivermos a key vamos à API
      return this.http.get<Product>(baseUrl + productsUrl + dashUrl + id);
    }

    //https://localhost:5001/api/products/brands
    getTypes(){

      if(this.types.length > 0){
        return of(this.types);
      }


      return this.http.get<Type[]>(baseUrl + productsUrl +  dashUrl +  typesUrl).pipe(
        map(types => this.types = types)
      );
    }

  //https://localhost:5001/api/products/brands
  getBrands(){
    
    if(this.brands.length > 0){
      return of(this.brands);
    }

    return this.http.get<Brand[]>(baseUrl + productsUrl +  dashUrl +  brandsUrl).pipe(
      map(brands => this.brands = brands)
    );
  }
}
