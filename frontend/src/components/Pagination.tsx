type PaginationProps = {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

function Pagination({ currentPage, totalPages, onPageChange }: PaginationProps) {
  if (totalPages <= 1) return null;

  const pages = Array.from({ length: totalPages }, (_, index) => index + 1);

  return (
    <nav
      aria-label="Product pages"
      className="flex flex-wrap items-center justify-center gap-2"
    >
      {pages.map((page) => (
        <button
          key={page}
          type="button"
          onClick={() => onPageChange(page)}
          aria-current={page === currentPage ? "page" : undefined}
          className={`min-w-10 rounded-md border px-3 py-2 text-sm ${
            page === currentPage
              ? "border-transparent bg-greyscale-700 font-semibold"
              : "border-greyscale-700"
          }`}
        >
          {page}
        </button>
      ))}
    </nav>
  );
}

export default Pagination;
