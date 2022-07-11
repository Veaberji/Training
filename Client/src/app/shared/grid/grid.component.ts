import { Component, Input, OnInit } from '@angular/core';
import { CardItem } from '../models/card-item';
import { GridSizes } from '../models/grid-sizes';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
})
export class GridComponent implements OnInit {
  @Input('sizes') sizes: GridSizes = {
    small: 1,
    middle: 2,
    large: 3,
    extraLarge: 4,
  };
  @Input('data') data!: CardItem[];

  breakpoint!: number;

  ngOnInit() {
    this.setBreakpoint(window.innerWidth);
  }

  onResize(event: any) {
    this.setBreakpoint(event.target.innerWidth);
  }

  private setBreakpoint(width: number) {
    if (width <= 650) {
      this.breakpoint = this.sizes.small;
    } else if (width <= 800) {
      this.breakpoint = this.sizes.middle;
    } else if (width <= 1100) {
      this.breakpoint = this.sizes.large;
    } else {
      this.breakpoint = this.sizes.extraLarge;
    }
  }
}
