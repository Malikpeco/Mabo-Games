import { Component, inject } from '@angular/core';
import {LoadingBarService} from '../../../../core/services/loading-bar.service';

@Component({
  selector: 'app-fit-loading-bar',
  standalone: false,
  templateUrl: './fit-loading-bar.component.html',
  styleUrl: './fit-loading-bar.component.scss',
})
export class FitLoadingBarComponent {
  /**
   * Inject LoadingBarService to access loading state
   * Component subscribes to loading$ observable to show/hide bar
   */
  protected loadingBar = inject(LoadingBarService);
}
