import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LazyLoadEvent } from 'primeng/api';
import { DashboardService } from '../../../@core/services/dashboard/dashboard.service';
import { TableLazyLoadEvent } from 'primeng/table';

interface AuditResponse {
  id: number;
  userId?: number;
  type?: string;
  tableName?: string;
  dateTime: string; // Using string to format in UI
}

@Component({
  selector: 'app-audit',
  templateUrl: './audit.component.html',
  styleUrls: ['./audit.component.scss']
})
export class AuditComponent implements OnInit {
  audits: AuditResponse[] = [];
  totalRecords: number = 0;
  loading: boolean = false;

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.getFilterData({ first: 0, rows: 10 }); // Initial load with first page
  }

  getFilterData(event: TableLazyLoadEvent) {
    this.dashboardService.getAllAuditsByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      debugger
      this.audits = data.data;
    })
  }
}
