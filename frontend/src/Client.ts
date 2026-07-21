import type { Product, ProductDetails } from './models/Products'
import type { Categories } from './models/Categories'
import type { PagedResult } from './models/PagedResult'
import type { LoginResult } from './models/Auth'
import { getUserId, saveAuthSession } from './utils/auth'

const BASE_URL = '/api'
const RESPONSE_ERROR_MESSAGE = 'Problem with response'
const NETWORK_ERROR_MESSAGE = 'We are currently experiencing issues loading the data'

export class Client {
    async getProducts(): Promise<Product[]> {
        return this.getJson(`${BASE_URL}/proizvodi`)
    }

    async getProductsPage(page: number, pageSize: number): Promise<PagedResult<Product>> {
        return this.getJson(`${BASE_URL}/proizvodi?page=${page}&pageSize=${pageSize}`)
    }

    async searchProducts(q: string): Promise<Product[]> {
        if (!q.trim()) {
            return this.getProducts()
        }
        return this.getJson(`${BASE_URL}/proizvodi/search?q=${encodeURIComponent(q)}`)
    }

    async searchProductsPage(q: string, page: number, pageSize: number): Promise<PagedResult<Product>> {
        if (!q.trim()) {
            return this.getProductsPage(page, pageSize)
        }
        return this.getJson(
            `${BASE_URL}/proizvodi/search?q=${encodeURIComponent(q)}&page=${page}&pageSize=${pageSize}`
        )
    }

    async getCategories(): Promise<Categories[]> {
        return this.getJson(`${BASE_URL}/proizvodi/categories`)
    }

    async getProductsByCategory(
        slug: string,
        minPrice?: number | null,
        maxPrice?: number | null
    ): Promise<Product[]> {
        return this.getJson(
            `${BASE_URL}/proizvodi/categories/${encodeURIComponent(slug)}${buildQueryString({ minPrice, maxPrice })}`
        )
    }

    async getProductsByCategoryPage(
        slug: string,
        minPrice: number | null,
        maxPrice: number | null,
        page: number,
        pageSize: number
    ): Promise<PagedResult<Product>> {
        return this.getJson(
            `${BASE_URL}/proizvodi/categories/${encodeURIComponent(slug)}${buildQueryString({ minPrice, maxPrice, page, pageSize })}`
        )
    }

    async getProductById(id: number): Promise<ProductDetails> {
        return this.getJson(`${BASE_URL}/proizvodi/${id}`)
    }

    async login(username: string, password: string, expiresInMins?: number): Promise<LoginResult> {
        const data = await this.postJson<LoginResult>(`${BASE_URL}/proizvodi/login`, {
            username,
            password,
            ...(expiresInMins != null && { expiresInMins }),
        })
        saveAuthSession(data.accessToken, data.id)
        return data
    }

    async addFavorite(
        productId: number,
        title: string,
        price: number,
        description: string,
        thumbnail: string
    ): Promise<Product> {
        const userId = getUserId()
        if (userId == null) {
            throw new Error('User not authenticated')
        }
        return this.postJson(`${BASE_URL}/proizvodi/favorites`, {
            userId,
            productId,
            title,
            price,
            description,
            thumbnail,
        })
    }

    async getFavorites(userId: number): Promise<Product[]> {
        return this.getJson(`${BASE_URL}/proizvodi/favorites/${userId}`)
    }

    private async getJson<T>(url: string): Promise<T> {
        const response = await this.fetchData(url)
        return readJson<T>(response)
    }

    private async postJson<T>(url: string, body: unknown): Promise<T> {
        const response = await this.fetchData(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body),
        })
        return readJson<T>(response)
    }

    private async fetchData(url: string, init?: RequestInit): Promise<Response> {
        try {
            return init ? await fetch(url, init) : await fetch(url)
        } catch (error) {
            throw new Error(NETWORK_ERROR_MESSAGE, { cause: error })
        }
    }
}

async function readJson<T>(response: Response): Promise<T> {
    if (!response.ok) {
        throw new Error(RESPONSE_ERROR_MESSAGE)
    }
    return (await response.json()) as T
}

function buildQueryString(params: Record<string, number | null | undefined>): string {
    const searchParams = new URLSearchParams()
    for (const [key, value] of Object.entries(params)) {
        if (value != null) {
            searchParams.set(key, String(value))
        }
    }
    const query = searchParams.toString()
    return query ? `?${query}` : ''
}

export const client = new Client()
