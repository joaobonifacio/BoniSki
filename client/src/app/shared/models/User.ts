export interface User {
    email: string;
    displayName: string;
    token: string;
}

export interface Address {
    firstName: string;
    lastName: string;
    street: string;
    city: string;
    state: string;
    zipCode: string;
}

export interface UserDTO {
    email: string;
    password: string;
}