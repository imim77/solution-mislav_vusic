import type { Product } from './models/Products'

export class Client
{   
    private baseUrl = '/api';
    async getProducts(){
        const response = await this.fetchData(`${this.baseUrl}/proizvodi`);
        if(!response.ok){
            throw new Error("Problem with response");
        }
        const data: Product[] = await response.json();
        return data;
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