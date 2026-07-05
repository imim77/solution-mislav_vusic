export interface Product {
  id: number;
  title: string;
  price: number;
  description: string;
  thumbnail: string;
}

export interface ProductReview {
  rating: number;
  comment: string;
  reviewerName: string;
  date: string;
}

export interface ProductDetails {
  id: number;
  title: string;
  price: number;
  description: string;
  category: string;
  brand: string | null;
  rating: number;
  stock: number;
  availabilityStatus: string;
  images: string[];
  tags: string[];
  reviews: ProductReview[];
}