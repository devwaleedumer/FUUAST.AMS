import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Table, TableLazyLoadEvent } from 'primeng/table';
import { DepartmentRequest } from '../../../@core/api/configuration/department/department';
import { Subject } from 'rxjs';
import { DepartmentService } from '../../../@core/services/department/department.service';
import { MessageService } from 'primeng/api';
import { FacultyService } from '../../../@core/services/faculty/faculty.service';
import autoTable from 'jspdf-autotable';
import { RolesAndPermissionService } from '../../../@core/utilities/roles-and-permission.service';
import { AuthService } from '../../../@core/utilities/auth-service.service';
import { User } from '../../../@core/api/auth/user';

@Component({
  selector: 'app-department',
  templateUrl: './department.component.html',
  styleUrl: './department.component.scss'
})
export class DepartmentComponent {
  adddepartmentForm!: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  departmentId!: number;
  departmentResponse: any[] = [];
  facultyResponse: any[] = [];
  departmentRequest: DepartmentRequest;
  submitted: boolean = false;
  cols: any[] = [];
  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];
  destroy$: Subject<void> = new Subject<void>();
  user: any;
  userdetail: any;

  constructor(private _service: DepartmentService, public _auth: AuthService, private messageService: MessageService, private fb: FormBuilder, private _facultyService: FacultyService, public _permission: RolesAndPermissionService) {
    this.departmentRequest = new DepartmentRequest();
    this.departmentId = 0;
  }

  ngOnInit() {
    this.ValidationAddFormControl();
    this.user = this._auth.User;
    this.loaddepartmentData();
    this.userdetail = this._auth.User;


    this.cols = [
      { field: 'id', header: 'Department ID' },
      { field: "name", header: "Name" },
      { field: "faculty", header: "Faculty" },

    ];
    this.statuses = [
      { label: 'INSTOCK', value: 'instock' },
      { label: 'LOWSTOCK', value: 'lowstock' },
      { label: 'OUTOFSTOCK', value: 'outofstock' }
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
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Department.Create")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to create department.', life: 3000 });
      return;
    }
    this.isEditing = true; // Add mode
    this.loadFaculity();
    this.adddepartmentForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true;
  }

  ValidationAddFormControl() {
    this.adddepartmentForm = this.fb.group({
      name: ['', Validators.required],
      faculityId: [null, Validators.required]
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
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Department.Delete")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to delete deparmtent.', life: 3000 });
      return;
    }
    this.deleteDialog = true;
    this.departmentId = response.id;

  }
  editDetails() {
    if (this.adddepartmentForm.valid) {
      this.departmentRequest = this.adddepartmentForm.value;
      this.departmentRequest.id = this.departmentId;
      this._service.updateDepartment(this.departmentRequest).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'department Added', life: 3000 });
        this.loaddepartmentData();  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'department could not be added', life: 3000 });
      });
    }
    this.adddepartmentForm.markAllAsTouched();
  }
  saveDetails() {
    this.submitted = true;
    if (this.adddepartmentForm.valid) {

      this._service.addDepartment(this.adddepartmentForm.value).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'department Added', life: 3000 });
        this.loaddepartmentData();  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'department could not be added', life: 3000 });
      });
    }
    this.adddepartmentForm.markAllAsTouched();
  }
  filterText(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const filteredValue = inputElement.value.replace(/[^a-zA-Z\s]/g, ''); // Allow only letters and spaces
    inputElement.value = filteredValue;
    this.adddepartmentForm.get('name')?.setValue(filteredValue, { emitEvent: true }); // Update the form control value
  }


  showEditModal(response: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Department.Update")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to update department.', life: 3000 });
      return;
    }
    this.addDialog = true;
    this.submitted = true;
    this.isEditing = false;
    this.loadFaculity();
    debugger
    this.adddepartmentForm.patchValue(response);
    this.departmentId = response.id;
  }

  confirmDelete() {

    this._service.deleteDepartment(this.departmentId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'department Deleted', life: 3000 });
      this.loaddepartmentData();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'department could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
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
        fileName + "export" + new Date().getTime() + EXCEL_EXTENSION
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
    this._service.getAllDepartmentByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      debugger
      this.departmentResponse = data.data;
    })
  }

}
