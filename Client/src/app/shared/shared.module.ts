import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { PageSelectorComponent } from './page-selector/page-selector.component';
import { PageSizeSelectorComponent } from './page-size-selector/page-size-selector.component';
import { PaginationComponent } from './pagination/pagination.component';
import { FooterComponent } from './footer/footer.component';
import { LoadingComponent } from './loading/loading.component';
import { CardComponent } from './card/card.component';
import { TableComponent } from './table/table.component';

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
  ],
  imports: [CommonModule, RouterModule],
  exports: [
    CommonModule,
    NotFoundComponent,
    NavBarComponent,
    PaginationComponent,
    FooterComponent,
    LoadingComponent,
    CardComponent,
    TableComponent,
  ],
})
export class SharedModule {}
