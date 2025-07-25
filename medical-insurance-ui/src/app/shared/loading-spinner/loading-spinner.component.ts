import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading-spinner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loading-spinner.component.html',
  styleUrl: './loading-spinner.component.scss'
})
export class LoadingSpinnerComponent {
  @Input() show: boolean = false;
  @Input() size: 'small' | 'medium' | 'large' = 'medium';
  @Input() message: string = 'جاري التحميل...';
}
