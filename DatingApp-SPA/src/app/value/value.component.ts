import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
// this is client
@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
  values: any;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getvalue();
  }

  getvalue(): void {

    this.http.get('http://localhost:5000/api/values').subscribe(Response => {
      // tslint:disable-next-line: no-unused-expression
      this.values = Response;

    }, error => {
      console.log(error);
    });


  }

}
