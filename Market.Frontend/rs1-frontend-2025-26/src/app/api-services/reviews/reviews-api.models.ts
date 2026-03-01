export interface CreateReviewCommand {
  userGameId: number;
  content?: string;
  rating: number;
}