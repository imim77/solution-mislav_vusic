import type { ProductFilters } from "./components/ProductListFilters";

export const PAGE_SIZE = 8;

export const STARS = [1, 2, 3, 4, 5] as const;

export const EMPTY_FILTERS: ProductFilters = {
  search: "",
  slug: "",
  minPrice: null,
  maxPrice: null,
};

export const INPUT_CLASS = "rounded-lg border border-greyscale-300 bg-greyscale-700 px-4 py-3 text-sm text-greyscale-900 placeholder:text-greyscale-500";

export const INPUT_CLASS_FULL = "w-full rounded-lg border border-greyscale-300 bg-greyscale-700 px-4 py-3 text-sm text-greyscale-900 placeholder:text-greyscale-500";
