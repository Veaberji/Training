import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CardComponent {
  @Input('name') name!: string;
  @Input('imageUrl') imageUrl!: string;
  @Input('navUrl') navUrl!: string;
  @Input('newWindow') newWindow: boolean = false;

  constructor(private router: Router) {}

  goToPage(): void {
    if (this.newWindow) {
      window.open(this.navUrl, '_blank');
    } else {
      this.router.navigate([this.navUrl]);
    }
  }
}
