import { it, expect, describe, vi, afterEach } from 'vitest'
import {Client} from '../src/Client';
import type { Product } from '../src/models/Products';
import type { Categories } from '../src/models/Categories';

const mockFetchOk = <T>(data: T) =>
    vi.spyOn(globalThis, "fetch").mockResolvedValue({
        ok: true,
        json: async () => data,
    } as Response);

const mockFetchFail = () =>
    vi.spyOn(globalThis, "fetch").mockResolvedValue({
        ok: false,
    } as Response);

describe('Client', () => {
    afterEach(() => {
      vi.restoreAllMocks();
      localStorage.clear();
    });
    it("returns products", async () => {
    const products = [
      {
        title: "Bomboni",
        price: 10.23,
        description: "Haribo bomboni",
        thumbnail: "image_haribo.jpg",
      },
    ] as Product[];

    mockFetchOk(products);

    const client = new Client();

    const result = await client.getProducts();

    expect(result).toEqual(products);
  });
  it("throws when response is not ok", async () => {
  mockFetchFail();

  const client = new Client();

  await expect(client.getProducts()).rejects.toThrow(
    "Problem with response"
  );
  });

  it("searchProducts falls back to getProducts when query is empty", async () => {
    const products = [
      {
        title: "Bomboni",
        price: 10.23,
        description: "Haribo bomboni",
        thumbnail: "image_haribo.jpg",
      },
    ] as Product[];

    const fetchSpy = mockFetchOk(products);

    const client = new Client();

    const result = await client.searchProducts("");

    expect(result).toEqual(products);
    expect(fetchSpy).toHaveBeenCalledWith("/api/proizvodi");
  });

  it("searchProducts calls the search endpoint with encoded query", async () => {
    const products = [
      {
        title: "Haribo",
        price: 5.0,
        description: "Gummy bears",
        thumbnail: "haribo.jpg",
      },
    ] as Product[];

    const fetchSpy = mockFetchOk(products);

    const client = new Client();

    const result = await client.searchProducts("gummy bears");

    expect(result).toEqual(products);
    expect(fetchSpy).toHaveBeenCalledWith(
      "/api/proizvodi/search?q=gummy%20bears"
    );
  });

  it("searchProducts throws when response is not ok", async () => {
    mockFetchFail();

    const client = new Client();

    await expect(client.searchProducts("bomboni")).rejects.toThrow(
      "Problem with response"
    );
  });

  it("returns categories", async () => {
    const categories = [{ name: "Groceries", slug: "groceries" }] as Categories[];
    const fetchSpy = mockFetchOk(categories);

    const client = new Client();

    const result = await client.getCategories();

    expect(result).toEqual(categories);
    expect(fetchSpy).toHaveBeenCalledWith("/api/proizvodi/categories");
  });

  it("getProductsByCategory includes encoded slug and price filters", async () => {
    const products = [
      {
        id: 1,
        title: "Candy",
        price: 10,
        description: "Sweet candy",
        thumbnail: "candy.jpg",
      },
    ] as Product[];
    const fetchSpy = mockFetchOk(products);

    const client = new Client();

    const result = await client.getProductsByCategory("sweet snacks", 5, 20);

    expect(result).toEqual(products);
    expect(fetchSpy).toHaveBeenCalledWith(
      "/api/proizvodi/categories/sweet%20snacks?minPrice=5&maxPrice=20"
    );
  });

  it("login posts credentials and stores auth data", async () => {
    const fetchSpy = vi.spyOn(globalThis, "fetch").mockResolvedValue({
      ok: true,
      json: async () => ({ accessToken: "token", id: 7 }),
    } as Response);

    const client = new Client();

    await client.login("ana", "secret", 30);

    expect(fetchSpy).toHaveBeenCalledWith("/api/proizvodi/login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        username: "ana",
        password: "secret",
        expiresInMins: 30,
      }),
    });
    expect(localStorage.getItem("accessToken")).toBe("token");
    expect(localStorage.getItem("userId")).toBe("7");
  });

  it("addFavorite requires an authenticated user", async () => {
    const client = new Client();

    await expect(
      client.addFavorite(1, "Candy", 10, "Sweet candy", "candy.jpg")
    ).rejects.toThrow("User not authenticated");
  });

  it("addFavorite posts the favorite for the stored user", async () => {
    localStorage.setItem("userId", "7");
    const fetchSpy = vi.spyOn(globalThis, "fetch").mockResolvedValue({
      ok: true,
      json: async () => ({ id: 1 }),
    } as Response);

    const client = new Client();

    const result = await client.addFavorite(1, "Candy", 10, "Sweet candy", "candy.jpg");

    expect(result).toEqual({ id: 1 });
    expect(fetchSpy).toHaveBeenCalledWith("/api/proizvodi/favorites", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        userId: 7,
        productId: 1,
        title: "Candy",
        price: 10,
        description: "Sweet candy",
        thumbnail: "candy.jpg",
      }),
    });
  });
})
