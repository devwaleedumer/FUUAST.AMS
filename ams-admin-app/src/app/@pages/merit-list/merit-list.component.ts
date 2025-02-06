import { ApplicantmanagementService } from './../../@core/services/applicant_management/applicantmanagement.service';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import autoTable from 'jspdf-autotable';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { Subject } from 'rxjs';
import { DepartmentRequest } from '../../@core/api/configuration/department/department';
import { DepartmentService } from '../../@core/services/department/department.service';
import { MessageService } from 'primeng/api';
import { FacultyService } from '../../@core/services/faculty/faculty.service';
import { ProgramService } from '../../@core/services/program/program.service';
import { GenericHttpClientService } from '../../@core/utilities/generic-http-client.service';
import { saveAs } from 'file-saver';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-merit-list',
  templateUrl: './merit-list.component.html',
  styleUrl: './merit-list.component.scss'
})
export class MeritListComponent {
  meritListForm!: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  departmentId!: number;
  departmentResponse: any[] = [];
  meritListData: any[] = [];
  facultyResponse: any[] = [];
  departmentData: any[] = [];
  ShiftData: any[] = [];
  departmentRequest: DepartmentRequest;
  submitted: boolean = false;
  cols: any[] = [];
  exportColumns: any[] = [];

  statuses: any[] = [];
  programData: any[] = [];
  searchValue: string | undefined;
  totalRecords: number = 0;
  rowsPerPageOptions = [5, 10, 20];
  destroy$: Subject<void> = new Subject<void>();
  constructor(private _programService: ProgramService,
    private _httpClient: HttpClient,
    private _service: DepartmentService, private messageService: MessageService, private fb: FormBuilder, private _facultyService: FacultyService,
    private _applicantmanagementService: ApplicantmanagementService
  ) {
    this.departmentRequest = new DepartmentRequest();
    this.departmentId = 0;
  }

  loadDepartmentData() {
    const programId = this.meritListForm.get('programId')?.value;
    this._programService.getDepartmentsByProgram(programId).subscribe((response) => {
      this.departmentData = response;
      this.ShiftData = []
    });
  }
  /**
   * Retrieves all shifts by the selected program and department and updates the `ShiftData` property.
   * This is triggered when the user selects a department from the department dropdown.
   */
  loadShiftData() {
    const programId = this.meritListForm.get('programId')?.value;
    const departmentId = this.meritListForm.get('departmentId')?.value;
    this._service.getAllShiftsByProgramAndDepartmentId(departmentId, programId).subscribe((response) => {
      this.ShiftData = response;
    });
  }
  /**
   * Retrieves all programs and updates the `programData` property.
   */
  loadProgramData() {
    this._programService.getAllprogram().subscribe((response) => {
      this.programData = response;
    });

  }
  ngOnInit() {
    this.ValidationAddFormControl();

    this.loaddepartmentData();
    this.loadProgramData();
    this.cols = [
      { field: 'id', header: 'Department ID' },
      { field: "name", header: "Name" },
      { field: "faculty", header: "Faculty" },

    ];
    this.exportColumns = this.cols.map(col => (col.header));
  }
  loaddepartmentData() {
    this._service.getAllDepartment().subscribe((response: any) => {

      //console.log('Shift data received:', response);
      this.departmentResponse = response;
      this.departmentRequest.id = response.id;
    },
      (error) => {
        console.error('Error fetching program data:', error);
      }
    );
  }
  openNew() {
    this.isEditing = true; // Add mode
    this.loadFaculity();
    this.meritListForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true;
  }

  ValidationAddFormControl() {
    this.meritListForm = this.fb.group({
      departmentId: [null, Validators.required],
      shiftId: [null, Validators.required],
      programId: [null, Validators.required],
    });
  }
  loadFaculity() {
    this._facultyService.getAllFaculty().subscribe((response: any) => {
      //console.log('Shift data received:', response)
      this.facultyResponse = response;
    }
    );
  }
  hideDialog() {
    this.addDialog = false;
    this.submitted = false;
  }
  isDeleted(response: any) {
    this.deleteDialog = true;
    this.departmentId = response.id;

  }

  saveDetails() {
    this.submitted = true;
    alert("outside")
    if (this.meritListForm.valid) {
      alert("inside")

      this._applicantmanagementService.generateMeritList(this.meritListForm.value).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'MeritList Generated  Successfully', life: 3000 });
        this.getFilterData({ first: 0, rows: 10 });  // Refresh the list
        this.hideDialog();

      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Merit List generation could not be Generated', life: 3000 });

      });
    }
    this.meritListForm.markAllAsTouched();
  }

  downloadPdf(id: number): void {
    this._httpClient.get('https://localhost:7081/api/ApplicantManagement/GetChallan/get-merit-list-report/' + id, { responseType: 'blob' }).subscribe(blob => {
      const file = new Blob([blob as any], { type: 'application/pdf' });
      saveAs(file, 'merit-list.pdf');
    }, error => {
      console.error('Download error:', error);
    });
  }

  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.departmentResponse.length; i++) {
      if (this.departmentResponse[i].id === id) {
        index = i;
        break;
      }
    }

    return index;
  }

  createId(): string {
    let id = '';
    const chars = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    for (let i = 0; i < 5; i++) {
      id += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return id;
  }
  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
  exportExcel() {
    import("xlsx").then(xlsx => {
      const worksheet = xlsx.utils.json_to_sheet(this.departmentResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "department");
    });
  }
  saveAsExcelFile(buffer: any, fileName: string): void {
    import("file-saver").then(FileSaver => {
      let EXCEL_TYPE =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8";
      let EXCEL_EXTENSION = ".xlsx";
      const data: Blob = new Blob([buffer], {
        type: EXCEL_TYPE
      });
      FileSaver.saveAs(
        data,
        fileName + "_export_" + new Date().getTime() + EXCEL_EXTENSION
      );
    });
  }

  exportPdf() {
    import("jspdf").then(jsPDF => {
      import("jspdf-autotable").then(() => {
        // Create new PDF document
        const doc = new jsPDF.default();

        // Define your columns (adjust these based on your actual data structure)
        const headerRow = this.exportColumns

        // Transform your data into rows
        const dataRows = this.departmentResponse.map(department => [
          department.id,
          department.name,
          department.faculity
          // Add other fields as needed
        ]);

        // Generate the table
        autoTable(doc, {
          head: [headerRow],
          body: dataRows,
          startY: 20,
          theme: 'grid',
          styles: {
            fontSize: 8,
            cellPadding: 3,
          },
          headStyles: {
            fillColor: [41, 128, 185],
            textColor: 255,
            fontSize: 10,
            fontStyle: 'bold',
          },
          margin: { top: 20 },
        });

        // Save the PDF
        doc.save('department.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this._applicantmanagementService.getAllMeritListByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      debugger
      this.meritListData = data.data;
    })
  }
}
