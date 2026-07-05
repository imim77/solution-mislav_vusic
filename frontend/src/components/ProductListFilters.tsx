import { useEffect, useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { Client } from "../Client";
import {useDebounce} from "../hooks/useDebounce";
import { INPUT_CLASS } from "../constants";

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

const inputClass = INPUT_CLASS

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
    <div className="flex flex-col gap-4">
      <input
        type="text"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        placeholder="Search products"
        className={`${inputClass} w-full px-5 py-3.5 text-base`}
      />
      <div className="flex flex-wrap gap-3">
        <select
          value={category}
          onChange={(e) => setCategory(e.target.value as ProductFilters['slug'])}
          className={`${inputClass} min-w-[160px] flex-1`}
        >
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
          className={`${inputClass} w-32 flex-1 sm:flex-none`}
        />
        <input
          type="number"
          min={0}
          value={maxPrice ?? ''}
          onChange={(e) => setMaxPrice(parsePrice(e.target.value))}
          placeholder="Max price"
          className={`${inputClass} w-32 flex-1 sm:flex-none`}
        />
      </div>
    </div>
  )
}

export default ProductListFilters