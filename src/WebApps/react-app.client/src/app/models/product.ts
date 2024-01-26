export interface Product {
    id: string,
    name: string,
    category: string,
    summary: string,
    description: string,
    image: string,
    price: number,
}

export interface ProductParams {
    orderBy: string;
    searchTerm?: string;
    categories: string[];
    pageNumber: number;
    pageSize: number;
}