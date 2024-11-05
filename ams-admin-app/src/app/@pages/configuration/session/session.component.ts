import { Component } from '@angular/core';
import { MessageService } from 'primeng/api';
import autoTable from 'jspdf-autotable';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SessionRequest} from '../../../@core/api/configuration/session/sessionrequest';
import { Subject } from 'rxjs';
import { AcademicyearService } from '../../../@core/services/academicyear/academicyear.service';
import { SessionService } from '../../../@core/services/session/session.service';

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html',
  styleUrl: './session.component.scss'
})
export class SessionComponent {
  sessionForm!: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  sessionId!: number;
  sessionResponse: any[] = [];
  academicyearResponse: any[] = [];
 sessionRequest:SessionRequest;
   submitted: boolean = false;
   cols: any[] = [];
  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];
  destroy$: Subject<void> = new Subject<void>();
  constructor(private _academicservice: AcademicyearService,private _service:SessionService, private messageService: MessageService,private fb:FormBuilder,) {
   this. sessionRequest=new SessionRequest();
    this.sessionId=0;
   }

  ngOnInit() {
    this.ValidationAddFormControl();
   
     this.loadsessionData();
     
     

    this.cols = [
      { field: 'id', header: 'Session ID' },
      { field: "name", header: "Name" },

    ];
    this.statuses = [
      { label: 'INSTOCK', value: 'instock' },
      { label: 'LOWSTOCK', value: 'lowstock' },
      { label: 'OUTOFSTOCK', value: 'outofstock' }
    ];
    this.exportColumns = this.cols.map(col => (col.header));

  }
  loadsessionData() {
    this._service.getAllSession().subscribe((response:any) => 
    {
      this.sessionResponse = response;
      this.sessionRequest.id=response.id;
     
   },
   (error) => {
      console.error('Error fetching Academicyear data:', error);
   }
);
}

loadacademicData() {
  this._academicservice.getAllAcademicyear().subscribe((response:any) => 
  {
    this.academicyearResponse = response;
 },
 (error) => {
    console.error('Error fetching session data:', error);
 }
);
}
openNew() {
  debugger
  this.isEditing = true;
  this.addDialog = true;  // Add mode
  this.loadacademicData();
    this.sessionForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
     
}

ValidationAddFormControl(){
this.sessionForm = this.fb.group({
  name: ['', Validators.required], 
   startDate:['', Validators.required],
   endDate:['', Validators.required],
   academicYearId:[null,Validators.required]
});
}

hideDialog() {
  this.addDialog = false;
  this.submitted = false;
}
isDeleted(response:any) {
    this.deleteDialog = true;
    this.sessionId=response.id;
    
  }
editDetails(){
  if (this.sessionForm.valid) {
   this.sessionRequest=this.sessionForm.value;
   this.sessionRequest.id=this.sessionId;
    this._service.updateSession(this.sessionRequest).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'session Added', life: 3000 });
      this.loadsessionData();  // Refresh the list
      this.hideDialog();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'session could not be added', life: 3000 });
    });
  }
  this.sessionForm.markAllAsTouched();
}
saveDetails() {
  this.submitted = true;
  if (this.sessionForm.valid) {
   
    this._service.addSession(this.sessionForm.value).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'session Added', life: 3000 });
      this.loadsessionData();  // Refresh the list
      this.hideDialog();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'session could not be added', life: 3000 });
    });
  }
  this.sessionForm.markAllAsTouched();
}
  
showEditModal(response:any){
    this.addDialog=true;
    this.submitted=true;
    this.isEditing=false;
    this.loadacademicData();
    debugger
    const startdate = new Date(response.startDate).toISOString().split('T')[0]; // Extract date only
    const enddate = new Date(response.endDate).toISOString().split('T')[0]; 
    this.sessionForm.patchValue({
      ...response,
      startDate: startdate,
      endDate: enddate
  });
    this.sessionId=response.id;
  }

  confirmDelete() {
 
    this._service.deleteSession(this.sessionId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Academicyear Deleted', life: 3000 });
      this.loadsessionData();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Academicyear could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
  }
  findIndexById(id: string): number {
      let index = -1;
      for (let i = 0; i < this.sessionResponse.length; i++) {
          if (this.sessionResponse[i].id === id) {
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
      const worksheet = xlsx.utils.json_to_sheet(this.sessionResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "session");
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
        const dataRows = this.sessionResponse.map(session => [
          session.id,
          session.name,
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
        doc.save('Session.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this._service.getAllSessionByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.sessionResponse = data.data;
    })
  }
}

