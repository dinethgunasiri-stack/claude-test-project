import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  stats = {
    totalOrders: 0,
    totalRevenue: 0,
    totalProducts: 0,
    totalCustomers: 0
  };

  constructor() { }

  ngOnInit(): void {
    // TODO: Load real statistics from API
    this.stats = {
      totalOrders: 150,
      totalRevenue: 25000,
      totalProducts: 45,
      totalCustomers: 320
    };
  }
}
