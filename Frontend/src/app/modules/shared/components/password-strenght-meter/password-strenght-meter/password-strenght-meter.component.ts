import { Component } from '@angular/core';
import { Input, OnChanges } from '@angular/core';

@Component({
  selector: 'app-password-strenght-meter',
  standalone: false,
  templateUrl: './password-strenght-meter.component.html',
  styleUrl: './password-strenght-meter.component.scss',
})
export class PasswordStrenghtMeterComponent implements OnChanges {
  @Input() password: string = '';
  
  strength: string = '';
  color: string = '';
  width: string = '0%';

  ngOnChanges(): void {
    this.calculateStrength();
  }

  private calculateStrength(): void {
    if (!this.password) {
      this.strength = '';
      this.color = '';
      this.width = '0%';
      return;
    }

    let strengthScore = 0;
    
    // Length check
    if (this.password.length >= 8) strengthScore++;
    if (this.password.length >= 12) strengthScore++;
    
    // Character variety checks
    if (/[a-z]/.test(this.password)) strengthScore++;
    if (/[A-Z]/.test(this.password)) strengthScore++;
    if (/[0-9]/.test(this.password)) strengthScore++;
    if (/[#?!@$%^&*-]/.test(this.password)) strengthScore++;
    
    // Set strength level
    if (strengthScore <= 2) {
      this.strength = 'Weak';
      this.color = '#ef4444';
      this.width = '33%';
    } else if (strengthScore <= 4) {
      this.strength = 'Medium';
      this.color = '#f59e0b';
      this.width = '66%';
    } else {
      this.strength = 'Strong';
      this.color = '#22c55e';
      this.width = '100%';
    }
  }
}