import { Address } from "./User";

export interface OrderToCreate {
    basketId: string;
    deliveryMethodId: number;
    shipToAddress: Address;
}

export interface OrderItem {
    productId: number;
    productName: string;
    pictureUrl: string;
    price: number;
    quantity: number;
}

export interface Order {
    id: number;
    buyerEmail: string;
    orderDate: Date;
    shipToAddress: Address;
    deliveryMethod: string;
    orderItems: OrderItem[];
    subTotal: number;
    total: number;
    status: string;
}
