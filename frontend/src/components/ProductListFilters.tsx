import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Client } from "../Client";
import {useDebounce} from "../hooks/useDebounce";

export type ProductFilters = {
  search: string;
  slug: string;
  minPrice: number | null;
  maxPrice: number | null;
};

type ProductListFiltersProps = {
    initialFilters: ProductFilters;
    onChange: (filters: ProductFilters) => void;
};

const client = new Client();

function ProductListFilters({initialFilters, onChange}: ProductListFiltersProps) {
  const [search, setSearch] = useState<ProductFilters['search']>(initialFilters.search);
  const debouncedSearch = useDebounce(search);
  const [category, setCategory] = useState<ProductFilters['slug']>(initialFilters.slug);
  const [minPrice, setMinPrice] = useState<ProductFilters['minPrice']>(initialFilters.minPrice);
  const [maxPrice, setMaxPrice] = useState<ProductFilters['maxPrice']>(initialFilters.maxPrice);
  const debouncedMinPrice = useDebounce(minPrice);
  const debouncedMaxPrice = useDebounce(maxPrice);
  const categoriesQuery = useQuery({
    queryKey: ['categories'],
    queryFn: () => client.getCategories(),
  });
  const categories = categoriesQuery.data ?? [];

  useEffect(() => {
    onChange({search: debouncedSearch, slug: category, minPrice: debouncedMinPrice, maxPrice: debouncedMaxPrice});
  }, [debouncedSearch, category, debouncedMinPrice, debouncedMaxPrice, onChange]);

  const parsePrice = (value: string): number | null => {
    if (value.trim() === '') return null;
    const n = Number(value);
    return Number.isFinite(n) ? n : null;
  };

  return (
    <div className="flex flex-wrap items-center gap-2 sm:flex-row">
        <input type="text" value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search products"
          className="min-w-[150px] flex-1 rounded-md bg-greyscale-700 p-4" />
        <select value={category} onChange={(e) => setCategory(e.target.value as ProductFilters['slug'])}
          className="min-w-[150px] flex-1 rounded-md bg-greyscale-700 p-4">
            <option value="">All categories</option>
            {categoriesQuery.isError && (
                <option value="" disabled>Unable to load categories</option>
            )}
            {categories.map((c) => (
                <option key={c.slug} value={c.slug}>{c.name}</option>
            ))}
        </select>
        <input
            type="number"
            min={0}
            value={minPrice ?? ''}
            onChange={(e) => setMinPrice(parsePrice(e.target.value))}
            placeholder="Min price"
            className="min-w-[120px] flex-1 rounded-lg bg-greyscale-700 p-4"
        />
        <input
            type="number"
            min={0}
            value={maxPrice ?? ''}
            onChange={(e) => setMaxPrice(parsePrice(e.target.value))}
            placeholder="Max price"
            className="min-w-[120px] flex-1 rounded-lg bg-greyscale-700 p-4"
        />
    </div>

  )
}

export default ProductListFilters
