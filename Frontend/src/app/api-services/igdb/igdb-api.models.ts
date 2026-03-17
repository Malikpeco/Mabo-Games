export interface SearchIgdbGameDto {
  id: number;
  name: string;
  coverUrl?: string | null;
}

export interface GetIgdbGameDetailsDto {
  id: number;
  name: string;
  summary?: string | null;
  releaseDate?: string | null;
  coverUrl?: string | null;
  screenshots: string[];
  artworks: string[];
  genres: string[];
  publisher?: string | null;
}
