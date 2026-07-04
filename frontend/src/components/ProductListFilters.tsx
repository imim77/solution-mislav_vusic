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
  //const [minPrice, setminPrice] = useState<ProductFilters['minPrice']>(null);
  //const [maxPrice, setmaxPrice] = useState<ProductFilters['maxPrice']>(null);
  const [categories, setCategories] = useState<Categories[]>([]);

  useEffect(() => {
    const client = new Client();
    client.getCategories()
      .then(data => setCategories(data))
      .catch(err => console.error(err));
  }, []);

  useEffect(() => {
    onChange({search: debouncedSearch, slug: category, minPrice: null, maxPrice: null});
  }, [debouncedSearch, category]);

  return (
    <div className="flex flex-row gap-2">
        <input type="text" value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search products" />
        <select value={category} onChange={(e) => setCategory(e.target.value as ProductFilters['slug'])}>
            <option value="">All categories</option>
            {categories.map((c) => (
                <option key={c.slug} value={c.slug}>{c.name}</option>
            ))}
        </select>

    </div>

  )
}

export default ProductListFilters