export function getTotalPages(totalItems: number, pageSize: number): number {
  if (pageSize <= 0) return 0;
  return Math.ceil(totalItems / pageSize);
}

export function getPageItems<T>(
  items: T[],
  currentPage: number,
  pageSize: number
): T[] {
  if (currentPage < 1 || pageSize <= 0) return [];

  const start = (currentPage - 1) * pageSize;
  return items.slice(start, start + pageSize);
}

export function getVisibleRange(
  totalItems: number,
  currentPage: number,
  pageSize: number
): { from: number; to: number } {
  if (totalItems === 0 || currentPage < 1 || pageSize <= 0) {
    return { from: 0, to: 0 };
  }

  const from = (currentPage - 1) * pageSize + 1;
  const to = Math.min(currentPage * pageSize, totalItems);

  return { from, to };
}
