import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { PageSelectorComponent } from './page-selector/page-selector.component';
import { PageSizeSelectorComponent } from './page-size-selector/page-size-selector.component';
import { PaginationComponent } from './pagination/pagination.component';
import { FooterComponent } from './footer/footer.component';
import { PagingService } from './services/paging.service';
import { DataService } from './services/data.service';

@NgModule({
  declarations: [
    NotFoundComponent,
    NavBarComponent,
    PaginationComponent,
    PageSizeSelectorComponent,
    PageSelectorComponent,
    FooterComponent,
  ],
  providers: [PagingService, DataService],
  imports: [CommonModule, RouterModule],
  exports: [CommonModule, NotFoundComponent, NavBarComponent, PaginationComponent, FooterComponent],
})
export class SharedModule {}
