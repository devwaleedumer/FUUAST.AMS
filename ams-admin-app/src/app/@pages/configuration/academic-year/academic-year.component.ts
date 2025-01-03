import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import autoTable from 'jspdf-autotable';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { AcademicyearService } from '../../../@core/services/academicyear/academicyear.service';
import { MessageService } from 'primeng/api';
import { Subject } from 'rxjs';
import { AcademicyearRequest } from '../../../@core/api/configuration/academicyear/academicyearrequest';

@Component({
  selector: 'app-academic-year',
  templateUrl: './academic-year.component.html',
  styleUrl: './academic-year.component.scss'
})
export class AcademicYearComponent {

  academicyearForm!: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  academicyearId!: number;
  academicyearResponse: any[] = [];
  academicyearRequest:AcademicyearRequest;
   submitted: boolean = false;
   cols: any[] = [];
  exportColumns: any[] = [];
  totalRecords: number = 0;
  rowsPerPageOptions = [5, 10, 20];
  constructor(private _service: AcademicyearService, private messageService: MessageService,private fb:FormBuilder,) {
   this. academicyearRequest=new AcademicyearRequest();
    this.academicyearId=0;
   }

  ngOnInit() {
    this.ValidationAddFormControl();

    this.cols = [
      { field: 'id', header: 'Academicyear ID' },
      { field: "name", header: "Name" },
      { field: "startdate", header: "Start Date" },
      { field: "enddate", header: "End Date" },

    ];

    this.exportColumns = this.cols.map(col => (col.header));

  }
  loadacademicData() {
    this._service.getAllAcademicyear().subscribe((response:any) =>
    {
      this.academicyearResponse = response;
      this.academicyearRequest.id=response.id;

   },
   (error) => {
      console.error('Error fetching Academicyear data:', error);
   }
);
}
openNew() {
  this.isEditing = true; // Add mode
    this.academicyearForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true;
}

ValidationAddFormControl(){
this.academicyearForm = this.fb.group({
  name: ['', Validators.required],
   startDate:['', Validators.required],
   endDate:['', Validators.required]

});
}

hideDialog() {
  this.addDialog = false;
  this.submitted = false;
}
isDeleted(response:any) {
    this.deleteDialog = true;
    this.academicyearId=response.id;

  }
editDetails(){
  if (this.academicyearForm.valid) {
   this.academicyearRequest=this.academicyearForm.value;
   this.academicyearRequest.id=this.academicyearId;
    this._service.updateAcademicyear(this.academicyearRequest).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academicyear Added', life: 3000 });
      this.loadacademicData();  // Refresh the list
      this.hideDialog();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Academicyear could not be added', life: 3000 });
    });
  }
  this.academicyearForm.markAllAsTouched();
}
saveDetails() {
  this.submitted = true;
  if (this.academicyearForm.valid) {

    this._service.addAcademicyear(this.academicyearForm.value).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academicyear Added', life: 3000 });
      this.loadacademicData();  // Refresh the list
      this.hideDialog();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Academicyear could not be added', life: 3000 });
    });
  }
  this.academicyearForm.markAllAsTouched();
}

showEditModal(response:any){
    this.addDialog=true;
    this.submitted=true;
    this.isEditing=false;
    debugger
    const startdate = new Date(response.startDate).toISOString().split('T')[0]; // Extract date only
    const enddate = new Date(response.endDate).toISOString().split('T')[0];
    this.academicyearForm.patchValue({
      ...response,
      startDate: startdate,
      endDate: enddate
  });
    this.academicyearId=response.id;
  }

  confirmDelete() {

    this._service.deleteAcademicyear(this.academicyearId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academicyear Deleted', life: 3000 });
      this.loadacademicData();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Academicyear could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
  }
  findIndexById(id: string): number {
      let index = -1;
      for (let i = 0; i < this.academicyearResponse.length; i++) {
          if (this.academicyearResponse[i].id === id) {
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
      const worksheet = xlsx.utils.json_to_sheet(this.academicyearResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "academicyear");
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
        const dataRows = this.academicyearResponse.map(academicyear => [
          academicyear.id,
          academicyear.name,
          academicyear.startDate,
          academicyear.endDate
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
        doc.save('AcademicYear.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this._service.getAllAcademicyearByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.academicyearResponse = data.data;
    })
  }
}
