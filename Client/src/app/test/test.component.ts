import { Component, OnInit } from '@angular/core';
import { Artist } from '../models/artist';
import { ArtistService } from '../services/artist.service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css'],
})
export class TestComponent implements OnInit {
  data: Artist[] = [];
  constructor(private service: ArtistService) {}

  ngOnInit() {
    this.service.getAll().subscribe((response) => {
      this.data = response;
      console.log(this.data);
    });
  }
}
