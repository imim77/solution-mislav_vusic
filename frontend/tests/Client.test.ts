import { it, expect, describe, vi, afterEach } from 'vitest'
import {Client} from '../src/Client';
import type { Product } from '../src/models/Products';

const mockFetchOk = (products: Product[]) =>
    vi.spyOn(globalThis, "fetch").mockResolvedValue({
        ok: true,
        json: async () => products,
    } as Response);

const mockFetchFail = () =>
    vi.spyOn(globalThis, "fetch").mockResolvedValue({
        ok: false,
    } as Response);

describe('Client', () => {
    afterEach(() => { vi.restoreAllMocks(); });
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
})