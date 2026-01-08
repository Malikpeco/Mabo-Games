import { Component } from '@angular/core';

@Component({
  selector: 'app-storefront',
  standalone: false,
  templateUrl: './storefront.component.html',
  styleUrl: './storefront.component.scss',
})
export class StorefrontComponent {
  currentYear: string = "2025";

}
