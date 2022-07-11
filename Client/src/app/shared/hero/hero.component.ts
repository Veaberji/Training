import { Component, ChangeDetectionStrategy, Input } from '@angular/core';

@Component({
  selector: 'app-hero',
  templateUrl: './hero.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeroComponent {
  @Input('imageName') imageName!: string;
  @Input('imageUrl') imageUrl!: string;
  @Input('title') title!: string;
}
