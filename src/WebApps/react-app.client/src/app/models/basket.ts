export interface BasketItem {
    productId: string;
    productName?: string;
    image?: string;
    color?: string;
    price: number;
    quantity: number;
}

export interface Basket {
    shoppingCartId?: number;
    userName?: string;
    items: BasketItem[];
    totalPrice?: number;
}