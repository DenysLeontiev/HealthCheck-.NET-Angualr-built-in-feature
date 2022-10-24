import { Component, Inject, OnInit, } from '@angular/core';
import { from } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { error } from 'console';

@Component({
  selector: 'app-health-check',
  templateUrl: './health-check.component.html',
  styleUrls: ['./health-check.component.css']
})
export class HealthCheckComponent implements OnInit {
  public result: Result;

  constructor(private httpClient: HttpClient,@Inject("BASE_URL") private baseUrl: string) { }

  ngOnInit() {
    this.httpClient.get<Result>(this.baseUrl + "hc").subscribe(response => {
      this.result = response;
    }, error => {
        console.log(error);
    })
  }

}

interface Result {
  hecks: Check[],
  totalStatus: string,
  totalResponseTime: number,
}

interface Check {
  name: string,
  status: string,
  responseTime: number,
}
