import type { Product } from './models/Products'
import type { Categories } from './models/Categories'

export class Client
{   
    private baseUrl = '/api';
    async getProducts(){
        const response = await this.fetchData(`${this.baseUrl}/proizvodi`);
        if(!response.ok){
            throw new Error("Problem with response");
        }
        const data: Product[] = await response.json();
        return data ?? [];
    }

    async searchProducts(q: string){
        if (!q.trim()) {
            return this.getProducts();
        }
        const response = await this.fetchData(`${this.baseUrl}/proizvodi/search?q=${encodeURIComponent(q)}`);
        if(!response.ok){
            throw new Error("Problem with response");
        }
        const data: Product[] = await response.json();
        return data ?? [];
    }

    async getCategories(){
        const response = await this.fetchData(`${this.baseUrl}/proizvodi/categories`);
        if(!response.ok){
            throw new Error("Problem with response");
        }
        const data: Categories[] = await response.json();
        return data ?? [];
    }

    async getProductsByCategory(slug: string, minPrice?: number | null, maxPrice?: number | null){
        let url = `${this.baseUrl}/proizvodi/categories/${encodeURIComponent(slug)}`;
        const params = new URLSearchParams();
        if (minPrice != null) params.set('minPrice', String(minPrice));
        if (maxPrice != null) params.set('maxPrice', String(maxPrice));
        if (params.toString()) url += `?${params.toString()}`;
        const response = await this.fetchData(url);
        if(!response.ok){
            throw new Error("Problem with response");
        }
        const data: Product[] = await response.json();
        return data ?? [];
    }

    private async fetchData(url: string){
        try{
            const response = await fetch(url);
            return response;
        }catch(error){
            throw new Error('We are currently experiencing issues loading the data');
        }
    }
}