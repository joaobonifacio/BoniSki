import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

export const baseUrl: string = 'https://localhost:5001/api/';
export const productsUrl: string = 'products';
export const dashUrl: string = '/';
export const queryStringUrl: string = '?';
export const pageSizeUrl: string = 'pageSize';
export const equalsUrl: string = '=';
export const brandsUrl: string = 'brands';
export const typesUrl: string = 'types';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class ConstsModule { }
