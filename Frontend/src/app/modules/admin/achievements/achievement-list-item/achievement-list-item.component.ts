import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AchievementDto } from '../../../../api-services/achievements/achievements-api.models';

@Component({
  selector: 'app-achievement-list-item',
  standalone: false,
  templateUrl: './achievement-list-item.component.html',
  styleUrl: './achievement-list-item.component.scss',
})
export class AchievementListItemComponent {
  @Input({ required: true }) achievement!: AchievementDto;

  @Output() editAchievement = new EventEmitter<AchievementDto>();
  @Output() deleteAchievement = new EventEmitter<AchievementDto>();

  get thumbnailUrl(): string {
    return this.achievement.imageURL?.trim() || '/carousel-placeholder-image.png';
  }

  onEdit(): void {
    this.editAchievement.emit(this.achievement);
  }

  onDelete(): void {
    this.deleteAchievement.emit(this.achievement);
  }
}
