import { it, expect, describe, vi, afterEach } from 'vitest'
import {Client} from '../src/Client';
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
    ];

    vi.spyOn(globalThis, "fetch").mockResolvedValue({
      ok: true,
      json: async () => products,
    } as Response);

    const client = new Client();

    const result = await client.getProducts();

    expect(result).toEqual(products);
  });
  it("throws when response is not ok", async () => {
  vi.spyOn(globalThis, "fetch").mockResolvedValue({
    ok: false,
  } as Response);

  const client = new Client();

  await expect(client.getProducts()).rejects.toThrow(
    "Problem with response"
  );
});
})