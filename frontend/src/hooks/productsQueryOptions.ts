import { keepPreviousData, queryOptions } from "@tanstack/react-query"
import type { ProductFilters } from "../models/Filters"
import { client } from "../Client"
import { PAGE_SIZE } from "../constants"

export const productsQueryOptions = (filters: ProductFilters, page: number) =>
  queryOptions({
    queryKey: ['products', filters, page],
    queryFn: () => {
      if (filters.slug) {
        return client.getProductsByCategoryPage(
          filters.slug,
          filters.minPrice,
          filters.maxPrice,
          page,
          PAGE_SIZE
        )
      }
      if (filters.search.trim()) {
        return client.searchProductsPage(filters.search, page, PAGE_SIZE)
      }
      return client.getProductsPage(page, PAGE_SIZE)
    },
    placeholderData: keepPreviousData,
  })
