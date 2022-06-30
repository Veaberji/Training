import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlbumService } from 'src/app/albums/services/album.service';
import { TrackService } from '../../tracks/services/track.service';
import { ArtistDetailsService } from '../services/artist-details.service';
import { ArtistService } from '../services/artist.service';
import { ArtistSupplements } from '../models/artist-supplements';
import { ArtistDetails } from '../models/artist-details';
import { SupplementRoute } from '../supplementRoutes';

@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
})
export class ArtistDetailsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private artistService: ArtistService,
    private artistDetailsService: ArtistDetailsService,
    private trackService: TrackService,
    private albumService: AlbumService
  ) {}

  private name!: string;
  supplementRoute = SupplementRoute;
  supplementType: string | null = null;
  readonly componentRouteName: string = 'details';

  artist: ArtistDetails = {
    name: '',
    imageUrl: '',
    biography: '',
  };
  supplements: ArtistSupplements = {
    topTracks: [],
    topAlbums: [],
    similarArtists: [],
  };

  ngOnInit(): void {
    let params = this.route.snapshot.paramMap;
    this.name = String(params.get('name'));
    this.supplementType = String(params.get('supplement'));
    this.initArtists();
    this.initSupplement();
  }

  private initArtists(): void {
    this.artistDetailsService.get(this.name).subscribe((response) => {
      this.artist = response;
    });
  }

  private initSupplement(): void {
    switch (this.supplementType) {
      case this.supplementRoute.TopTracks: {
        this.loadTracks();
        break;
      }
      case this.supplementRoute.TopAlbums: {
        this.loadAlbums();
        break;
      }
      case this.supplementRoute.SimilarArtists: {
        this.loadSimilarArtists();
        break;
      }
      default: {
        break;
      }
    }
  }

  loadTracks(): void {
    this.trackService.getTopTracks(this.name).subscribe((response) => {
      this.supplements.topTracks = response;
    });
  }

  loadAlbums(): void {
    this.albumService.getTopAlbums(this.name).subscribe((response) => {
      this.supplements.topAlbums = response;
    });
  }

  loadSimilarArtists(): void {
    this.artistService.getSimilarArtists(this.name).subscribe((response) => {
      this.supplements.similarArtists = response;
    });
  }

  onOverviewSelect(): void {
    this.changeSupplement();
  }

  onTopTracksSelect(): void {
    this.changeSupplement();
    if (this.supplements.topTracks.length != 0) {
      return;
    }
    this.loadTracks();
  }

  onTopAlbumsSelect(): void {
    this.changeSupplement();
    if (this.supplements.topAlbums.length != 0) {
      return;
    }
    this.loadAlbums();
  }

  onSimilarArtistsSelect(): void {
    this.changeSupplement();
    if (this.supplements.similarArtists.length != 0) {
      return;
    }
    this.loadSimilarArtists();
  }

  private changeSupplement(): void {
    let params = this.route.snapshot.paramMap;
    this.supplementType = String(params.get('supplement'));
  }
}
