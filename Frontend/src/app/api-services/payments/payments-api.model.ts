export interface CreateStripeCheckoutSessionRequest {
  orderId: number;
}

export interface CreateStripeCheckoutSessionResponse {
  orderId: number;
  sessionId: string;
  checkoutUrl: string;
  expiresAtUtc: string; 
}