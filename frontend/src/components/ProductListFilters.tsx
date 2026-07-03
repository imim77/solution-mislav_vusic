import { useEffect, useState } from "react";

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
  const [debouncedSearch, setDebouncedSearch] = useState<ProductFilters['search']>('');

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedSearch(search);
    }, 500);
    return () => clearTimeout(handler);
  }, [search]);

  useEffect(() => {
    onChange({search: debouncedSearch, slug: '', minPrice: null, maxPrice: null});
  }, [debouncedSearch])

  return (
    <input type="text" value={search} onChange={(e) => setSearch(e.target.value)} placeholder="Search products...">
    </input>
    
  )
}

export default ProductListFilters