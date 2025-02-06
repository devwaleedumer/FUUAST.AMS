import { Component } from '@angular/core';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { FacultyService } from '../../../@core/services/faculty/faculty.service';
import { MessageService } from 'primeng/api';
import { Faculty } from '../../../@core/api/configuration/faculty/faculty';
import { Product } from '../../../@core/api/dashboard/product';
import autoTable from 'jspdf-autotable';
import { ShiftService } from '../../../@core/services/shift/shift.service';
import { Subject } from 'rxjs';
import { ShiftRequest } from '../../../@core/api/configuration/shift/shift';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RolesAndPermissionService } from '../../../@core/utilities/roles-and-permission.service';
import { AuthService } from '../../../@core/utilities/auth-service.service';
declare var $: any;
@Component({
  selector: 'app-shift',
  templateUrl: './shift.component.html',
  styleUrl: './shift.component.scss'
})

export class ShiftComponent {
  addShiftForm!: FormGroup;
  editshiftDialog: boolean = false;
  isEditing: boolean = false;
  deleteShiftDialog: boolean = false;
  addShiftDialog: boolean = false;
  shiftId!: number;
  shiftResponse: any[] = [];
  shiftRequest: ShiftRequest;
  submitted: boolean = false;
  cols: any[] = [];
  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];
  destroy$: Subject<void> = new Subject<void>();
  userdetail: any;
  constructor(private _shiftService: ShiftService, private messageService: MessageService, private fb: FormBuilder, public _auth: AuthService, public _permission: RolesAndPermissionService) {
    this.shiftRequest = new ShiftRequest();
    this.shiftId = 0;
  }

  ngOnInit() {
    this.ValidationAddFormControl();

    this.loadShiftData();
    this.userdetail = this._auth.User;



    this.cols = [
      { field: 'id', header: 'Shift ID' },
      { field: "name", header: "Name" },

    ];
    this.statuses = [
      { label: 'INSTOCK', value: 'instock' },
      { label: 'LOWSTOCK', value: 'lowstock' },
      { label: 'OUTOFSTOCK', value: 'outofstock' }
    ];
    this.exportColumns = this.cols.map(col => (col.header));

  }
  loadShiftData() {

    this._shiftService.getAllShift().subscribe((response: any) => {
      console.log('Shift data received:', response);
      this.shiftResponse = response;
      this.shiftRequest.id = response.id;
    },
      (error) => {
        console.error('Error fetching shift data:', error);
      }
    );
  }
  openNew() {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Shift.Create")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to create shift.', life: 3000 });
      return;
    }
    this.isEditing = true; // Add mode
    this.addShiftForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addShiftDialog = true;
  }
  filterText(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const filteredValue = inputElement.value.replace(/[^a-zA-Z\s]/g, ''); // Allow only letters and spaces
    inputElement.value = filteredValue;
    this.addShiftForm.get('name')?.setValue(filteredValue, { emitEvent: true }); // Update the form control value
  }

  ValidationAddFormControl() {
    this.addShiftForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required]
    });
  }
  hideDialog() {
    this.addShiftDialog = false;
    this.submitted = false;
  }
  deleteshift(response: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Shift.Delete")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to delete shift.', life: 3000 });
      return;
    }
    this.deleteShiftDialog = true;
    this.shiftId = response.id;

  }
  editDetails() {

    if (this.addShiftForm.valid) {

      this.shiftRequest = this.addShiftForm.value;
      this.shiftRequest.id = this.shiftId;
      this._shiftService.updateShift(this.shiftRequest).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Shift Added', life: 3000 });
        this.loadShiftData();  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Shift could not be added', life: 3000 });
      });
    }
    this.addShiftForm.markAllAsTouched();
  }
  saveDetails() {
    this.submitted = true;
    if (this.addShiftForm.valid) {

      this._shiftService.addShift(this.addShiftForm.value).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Shift Added', life: 3000 });
        this.loadShiftData();  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Shift could not be added', life: 3000 });
      });
    }
    this.addShiftForm.markAllAsTouched();
  }

  showEditShiftModal(response: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Shift.Update")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to update shift.', life: 3000 });
      return;
    }
    this.addShiftDialog = true;
    this.submitted = true;
    this.isEditing = false;
    this.addShiftForm.patchValue(response);
    this.shiftId = response.id;
  }

  confirmDelete() {
    debugger
    this._shiftService.deleteShift(this.shiftId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Shift Deleted', life: 3000 });
      this.loadShiftData();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Shift could not be deleted', life: 3000 });
    });
    this.deleteShiftDialog = false;
  }
  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.shiftResponse.length; i++) {
      if (this.shiftResponse[i].id === id) {
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
      const worksheet = xlsx.utils.json_to_sheet(this.shiftResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "Shifts");
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
        const dataRows = this.shiftResponse.map(shift => [
          shift.id,
          shift.name,
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
        doc.save('shifts.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this._shiftService.getAllShiftByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.shiftResponse = data.data;
    })
  }


}
