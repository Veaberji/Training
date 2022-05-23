import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data-service';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css'],
})
export class TestComponent implements OnInit {
  data: any;
  constructor(private service: DataService) {}

  ngOnInit() {
    this.service.getAll().subscribe((response) => {
      this.data = response;
      console.log(this.data);
    });
  }
}
