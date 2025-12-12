export abstract class BaseComponent {
  isLoading = false;
  errorMessage: string | null = null;

  startLoading(): void {
    this.isLoading = true;
    this.errorMessage = null;
  }

  stopLoading(error?: string): void {
    this.isLoading = false;
    if (error) this.errorMessage = error;
  }
}
