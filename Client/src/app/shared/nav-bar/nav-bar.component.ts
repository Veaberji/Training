import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css'],

  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NavBarComponent {
  @Input('title') title!: string;
}
