export interface CreateStripeCheckoutSessionRequest {
  orderId: number;
}

export interface CreateStripeCheckoutSessionResponse {
  orderId: number;
  sessionId: string;
  checkoutUrl: string;
  expiresAtUtc: string; 
}

export interface ConfirmStripeSessionRequest {
  sessionId: string;
}

export interface ConfirmStripeSessionResponse {
  isSuccess: boolean;
  orderId?: number;
  message?: string;
}