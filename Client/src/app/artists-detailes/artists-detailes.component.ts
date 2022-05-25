import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-artists-detailes',
  templateUrl: './artists-detailes.component.html',
  styleUrls: ['./artists-detailes.component.css'],
})
export class ArtistsDetailesComponent implements OnInit {
  constructor(private router: Router, private route: ActivatedRoute) {}
  id!: string;

  ngOnInit(): void {
    let params = this.route.snapshot.paramMap;
    this.id = String(params.get('id'));
  }

  return() {
    this.router.navigate(['/']);
  }
}
