import { ErrorHandler, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared/shared.module';
import { ArtistModule } from './artists/artist.module';
import { AlbumModule } from './albums/album.module';
import { AppComponent } from './app.component';
import { AppErrorHandler } from './shared/common/errors/app-error-handler';

@NgModule({
  declarations: [AppComponent],
  providers: [{ provide: ErrorHandler, useClass: AppErrorHandler }],
  imports: [BrowserModule, AppRoutingModule, HttpClientModule, SharedModule, ArtistModule, AlbumModule],
  bootstrap: [AppComponent],
})
export class AppModule {}
