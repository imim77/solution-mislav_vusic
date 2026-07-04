import type { ProductFilters } from "../components/ProductListFilters";

const EMPTY_FILTERS: ProductFilters = {
  search: "",
  slug: "",
  minPrice: null,
  maxPrice: null,
};

const parsePrice = (value: string | null): number | null => {
  if (value === null || value.trim() === "") return null;
  const n = Number(value);
  return Number.isFinite(n) ? n : null;
};

export function paramsToFilters(
  params: URLSearchParams
): ProductFilters {
  return {
    search: params.get("q") ?? "",
    slug: params.get("category") ?? "",
    minPrice: parsePrice(params.get("minPrice")),
    maxPrice: parsePrice(params.get("maxPrice")),
  };
}

export function filtersToParams(
  filters: ProductFilters,
  prev?: URLSearchParams
): URLSearchParams {
  const next = new URLSearchParams(prev);
  const setOrDelete = (key: string, value: string) => {
    if (value === "") next.delete(key);
    else next.set(key, value);
  };
  setOrDelete("q", filters.search.trim());
  setOrDelete("category", filters.slug);
  setOrDelete("minPrice", filters.minPrice == null ? "" : String(filters.minPrice));
  setOrDelete("maxPrice", filters.maxPrice == null ? "" : String(filters.maxPrice));
  return next;
}

export { EMPTY_FILTERS };