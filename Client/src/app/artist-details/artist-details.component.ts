import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-artist-details',
  templateUrl: './artist-details.component.html',
  styleUrls: ['./artist-details.component.css'],
})
export class ArtistdetailsComponent implements OnInit {
  constructor(private router: Router, private route: ActivatedRoute) {}
  name!: string;

  ngOnInit(): void {
    let params = this.route.snapshot.paramMap;
    this.name = String(params.get('name'));
    console.log(this.name);
  }

  return() {
    this.router.navigate(['/']);
  }
}

// TODO: add not found
