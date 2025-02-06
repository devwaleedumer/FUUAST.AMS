import { Component, OnInit, OnDestroy, inject, PLATFORM_ID } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Subscription, debounceTime } from 'rxjs';
import { Product } from '../../@core/api/dashboard/product';
import { LayoutService } from '../../@core/services/layout/layout.service';
import { ProductService } from '../../@core/services/dashboard/product.service';
import { DashboardService } from '../../@core/services/dashboard/dashboard.service';
import { isPlatformBrowser } from '@angular/common';

@Component({
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit, OnDestroy {

  items!: MenuItem[];

  products!: Product[];
  dashboardDetails: any = {};

  basicData: any;
  isLoading: boolean = false;
  revenueData: any;
  applicationsByDepartmentData: any;
  chart2Data: any;
  chart2Options: any;
  chartOptions: any;
  basicOptions: any;
  doughChartData: any;
  doughChartOptions: any;
  subscription!: Subscription;

  platformId = inject(PLATFORM_ID);
  constructor(private productService: ProductService, public layoutService: LayoutService, private _service: DashboardService) {
    this.subscription = this.layoutService.configUpdate$
      .pipe(debounceTime(25))
      .subscribe((config) => {
        this.initChart();
      });
  }

  ngOnInit() {

    this.getDashboard();
    this.productService.getProductsSmall().then(data => this.products = data);
    this.items = [
      { label: 'Add New', icon: 'pi pi-fw pi-plus' },
      { label: 'Remove', icon: 'pi pi-fw pi-minus' }
    ];
  }

  initializeChart2() {

    // Configure Chart Data
    this.chart2Data = {
      labels: this.applicationsByDepartmentData?.label,
      datasets: [
        {
          label: 'No. of Applications',
          data: this.applicationsByDepartmentData?.data,
          backgroundColor: '#42A5F5'
        }
      ]
    };

    // Configure Chart Options
    this.chart2Options = {
      responsive: true,
      plugins: {
        legend: {
          display: true,
          position: 'top'
        }
      },
      scales: {
        x: {
          ticks: {
            color: '#495057'
          }
        },
        y: {
          ticks: {
            color: '#495057'
          },
          beginAtZero: true
        }
      }
    };

  }

  getDashboard() {
    this.isLoading = true;
    this._service.feeChallanData().subscribe((response: any) => {
      this.revenueData = response;
      this.initChart();
    },
      (error) => {
        console.error('Error fetching  data:', error);
      })
    this._service.applicationByData().subscribe((response: any) => {
      this.applicationsByDepartmentData = response;
      this.initializeChart2();
    },
      (error) => {
        console.error('Error fetching  data:', error);
      })
    this._service.getDashboard().subscribe((response: any) => {
      debugger
      this.dashboardDetails = response;
      this.isLoading = false;
    },
      (error) => {
        console.error('Error fetching  data:', error);
      }

    );
  }


  initChart() {

    if (isPlatformBrowser(this.platformId)) {
      const documentStyle = getComputedStyle(document.documentElement);
      const textColor = documentStyle.getPropertyValue('--p-text-color');
      const textColorSecondary = documentStyle.getPropertyValue('--p-text-muted-color');
      const surfaceBorder = documentStyle.getPropertyValue('--p-content-border-color');

      this.basicData = {
        labels: this.revenueData?.label,
        datasets: [
          {
            label: 'Fee Revenue',
            data: this.revenueData?.data,
            backgroundColor: [
              'rgba(249, 115, 22, 0.2)',
              'rgba(6, 182, 212, 0.2)',
              'rgb(107, 114, 128, 0.2)',
              'rgba(139, 92, 246, 0.2)',
            ],
            borderColor: ['rgb(249, 115, 22)', 'rgb(6, 182, 212)', 'rgb(107, 114, 128)', 'rgb(139, 92, 246)'],
            borderWidth: 1,
          },
        ],
      };

      this.basicOptions = {
        plugins: {
          legend: {
            labels: {
              color: textColor,
            },
          },
        },
        scales: {
          x: {
            ticks: {
              color: textColorSecondary,
            },
            grid: {
              color: surfaceBorder,
            },
          },
          y: {
            beginAtZero: true,
            ticks: {
              color: textColorSecondary,
            },
            grid: {
              color: surfaceBorder,
            },
          },
        },
      };
    }
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
