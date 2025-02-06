import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import autoTable from 'jspdf-autotable';
import { MessageService } from 'primeng/api';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { Subject } from 'rxjs';
import { ProgramtypeService } from '../../../@core/services/programtype/programtype.service';
import { ProgramtypeRequest } from '../../../@core/api/configuration/programtype/programtype';
import { RolesAndPermissionService } from '../../../@core/utilities/roles-and-permission.service';
import { AuthService } from '../../../@core/utilities/auth-service.service';

@Component({
  selector: 'app-programtype',
  templateUrl: './programtype.component.html',
  styleUrl: './programtype.component.scss'
})
export class ProgramtypeComponent {

  addprogramtypeForm!: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  programtypeId!: number;
  programtypeResponse: any[] = [];
  progamtypeRequest: ProgramtypeRequest;
  submitted: boolean = false;
  cols: any[] = [];
  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];
  destroy$: Subject<void> = new Subject<void>();
  userdetail: any;

  constructor(private _service: ProgramtypeService, private messageService: MessageService, private fb: FormBuilder, public _auth: AuthService, public _permission: RolesAndPermissionService) {
    this.progamtypeRequest = new ProgramtypeRequest();
    this.programtypeId = 0;
  }

  ngOnInit() {
    this.userdetail = this._auth.User;
    this.ValidationAddFormControl();

    this.loadprogramtypeData();



    this.cols = [
      { field: 'id', header: 'Programtype ID' },
      { field: "name", header: "Name" },

    ];
    this.statuses = [
      { label: 'INSTOCK', value: 'instock' },
      { label: 'LOWSTOCK', value: 'lowstock' },
      { label: 'OUTOFSTOCK', value: 'outofstock' }
    ];
    this.exportColumns = this.cols.map(col => (col.header));

  }
  loadprogramtypeData() {

    this._service.getAllprogramtype().subscribe((response: any) => {
      //console.log('Shift data received:', response);
      this.programtypeResponse = response;
      this.progamtypeRequest.id = response.id;
    },
      (error) => {
        console.error('Error fetching programtype data:', error);
      }

    );
  }
  filterText(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const filteredValue = inputElement.value.replace(/[^a-zA-Z\s]/g, ''); // Allow only letters and spaces
    inputElement.value = filteredValue;
    this.addprogramtypeForm.get('name')?.setValue(filteredValue, { emitEvent: true }); // Update the form control value
  }
  openNew() {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.ProgramType.Create")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to create programtype.', life: 3000 });
      return;
    }
    this.isEditing = true; // Add mode
    this.addprogramtypeForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true;
  }

  ValidationAddFormControl() {
    this.addprogramtypeForm = this.fb.group({
      name: ['', Validators.required],

    });
  }
  hideDialog() {
    this.addDialog = false;
    this.submitted = false;
  }
  isDeleted(response: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.ProgramType.Delete")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to delete programtype.', life: 3000 });
      return;
    }
    this.deleteDialog = true;
    this.programtypeId = response.id;

  }
  editDetails() {
    debugger
    if (this.addprogramtypeForm.valid) {

      this.progamtypeRequest = this.addprogramtypeForm.value;
      this.progamtypeRequest.id = this.programtypeId;
      this._service.updateProgramtype(this.progamtypeRequest).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'programtype Added', life: 3000 });
        this.loadprogramtypeData();  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'programtype could not be added', life: 3000 });
      });
    }
    this.addprogramtypeForm.markAllAsTouched();
  }
  saveDetails() {
    this.submitted = true;
    if (this.addprogramtypeForm.valid) {

      this._service.addProgramtype(this.addprogramtypeForm.value).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'programtype Added', life: 3000 });
        this.loadprogramtypeData();  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'programtype could not be added', life: 3000 });
      });
    }
    this.addprogramtypeForm.markAllAsTouched();
  }

  showEditModal(response: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.ProgramType.Update")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to update programtype.', life: 3000 });
      return;
    }
    this.addDialog = true;
    this.submitted = true;
    this.isEditing = false;
    this.addprogramtypeForm.patchValue(response);
    this.programtypeId = response.id;
  }

  confirmDelete() {
    debugger
    this._service.deleteProgramtype(this.programtypeId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'programtype Deleted', life: 3000 });
      this.loadprogramtypeData();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'programtype could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
  }
  findIndexById(id: string): number {
    let index = -1;
    for (let i = 0; i < this.programtypeResponse.length; i++) {
      if (this.programtypeResponse[i].id === id) {
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
      const worksheet = xlsx.utils.json_to_sheet(this.programtypeResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "programtype");
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
        const dataRows = this.programtypeResponse.map(programtype => [
          programtype.id,
          programtype.name,
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
        doc.save('programtype.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this._service.getAllProgramtypeByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.programtypeResponse = data.data;
    })
  }
}
