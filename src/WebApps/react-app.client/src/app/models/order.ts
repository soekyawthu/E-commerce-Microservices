export interface ShippingAddress {
    fullName: string
    email: string
    addressLine: string
    country: string
    state: string
    city: string
    zipCode: number
}

export interface Card {
    name: string
    number: string
    expiration: string
    cvv: string
}

export interface OrderItem {
    name: string
    color: string
    price: number
    quantity: number
    id: string
    createdBy: string
    createdDate: string
    lastModifiedBy: string
    lastModifiedDate: string
}

export interface Order {
    id: string;
    userName: string;
    totalPrice: number;
    shippingAddress: ShippingAddress;
    paymentCard: Card
    items: OrderItem[];
    status: string;
    orderDate: string;
}