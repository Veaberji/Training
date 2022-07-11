import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from './material.module';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { PageSelectorComponent } from './page-selector/page-selector.component';
import { PageSizeSelectorComponent } from './page-size-selector/page-size-selector.component';
import { PaginationComponent } from './pagination/pagination.component';
import { FooterComponent } from './footer/footer.component';
import { LoadingComponent } from './loading/loading.component';
import { CardComponent } from './card/card.component';
import { TableComponent } from './table/table.component';
import { HeroComponent } from './hero/hero.component';
import { TabComponent } from './tab/tab.component';
import { GridComponent } from './grid/grid.component';

@NgModule({
  declarations: [
    NotFoundComponent,
    NavBarComponent,
    PaginationComponent,
    PageSizeSelectorComponent,
    PageSelectorComponent,
    FooterComponent,
    LoadingComponent,
    CardComponent,
    TableComponent,
    HeroComponent,
    TabComponent,
    GridComponent,
  ],
  imports: [CommonModule, RouterModule, MaterialModule],
  exports: [
    CommonModule,
    NotFoundComponent,
    NavBarComponent,
    PaginationComponent,
    FooterComponent,
    LoadingComponent,
    CardComponent,
    TableComponent,
    HeroComponent,
    TabComponent,
    GridComponent,
  ],
})
export class SharedModule {}
