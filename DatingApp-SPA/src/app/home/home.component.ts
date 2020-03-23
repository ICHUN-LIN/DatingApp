import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  values: any;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getvalue();
  }

  registerToggle(): void {
    this.registerMode = !this.registerMode;
  }

  cancelEventHandler(registerMode: boolean ): void {
    this.registerMode = registerMode;
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
