import { useEffect, useState } from "react";
import { Client } from "../Client";
import type { Categories } from "../models/Categories";
import {useDebounce} from "../hooks/useDebounce";

export type ProductFilters = {
  search: string;
  slug: string;
  minPrice: number | null;
  maxPrice: number | null;
};

type ProductListFiltersProps = {
    onChange: (filters: ProductFilters) => void;
};


function ProductListFilters({onChange}: ProductListFiltersProps) {
  const [search, setSearch] = useState<ProductFilters['search']>('');
  const debouncedSearch = useDebounce(search);
  const [category, setCategory] = useState<ProductFilters['slug']>('');
  const [minPrice, setMinPrice] = useState<ProductFilters['minPrice']>(null);
  const [maxPrice, setMaxPrice] = useState<ProductFilters['maxPrice']>(null);
  const debouncedMinPrice = useDebounce(minPrice);
  const debouncedMaxPrice = useDebounce(maxPrice);
  const [categories, setCategories] = useState<Categories[]>([]);

  useEffect(() => {
    const client = new Client();
    client.getCategories()
      .then(data => setCategories(data))
      .catch(err => console.error(err));
  }, []);

  useEffect(() => {
    onChange({search: debouncedSearch, slug: category, minPrice: debouncedMinPrice, maxPrice: debouncedMaxPrice});
  }, [debouncedSearch, category, debouncedMinPrice, debouncedMaxPrice]);

  const parsePrice = (value: string): number | null => {
    if (value.trim() === '') return null;
    const n = Number(value);
    return Number.isFinite(n) ? n : null;
  };

  return (
    <div className="flex flex-row gap-2">
        <input type="text" value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search products" />
        <select value={category} onChange={(e) => setCategory(e.target.value as ProductFilters['slug'])}>
            <option value="">All categories</option>
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
        />
        <input
            type="number"
            min={0}
            value={maxPrice ?? ''}
            onChange={(e) => setMaxPrice(parsePrice(e.target.value))}
            placeholder="Max price"
        />
    </div>

  )
}

export default ProductListFilters