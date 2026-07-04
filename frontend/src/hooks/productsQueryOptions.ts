import { queryOptions } from "@tanstack/react-query"
import type { ProductFilters } from "../components/ProductListFilters"
import { client } from "../Client"


export const productsQueryOptions = (filters: ProductFilters) =>
  queryOptions({
    queryKey: ['products', filters],
    queryFn: () =>
      filters.slug
        ? client.getProductsByCategory(filters.slug, filters.minPrice, filters.maxPrice)
        : client.searchProducts(filters.search),
})
