import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { Subject } from 'rxjs';
import { ProgramRequest } from '../../../@core/api/configuration/program/program';
import autoTable from 'jspdf-autotable';
import { ProgramService } from '../../../@core/services/program/program.service';
import { ProgramtypeService } from '../../../@core/services/programtype/programtype.service';

@Component({
  selector: 'app-program',
  templateUrl: './program.component.html',
  styleUrl: './program.component.scss'
})
export class ProgramComponent {
  addprogramForm!: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  programId!: number;
  programResponse: any[] = [];
  programtypeResponse: any[] = [];
  progamRequest:ProgramRequest;
   submitted: boolean = false;
   cols: any[] = [];
  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];
  destroy$: Subject<void> = new Subject<void>();
  constructor(private _service: ProgramService, private messageService: MessageService,private fb:FormBuilder,private _programtypeService:ProgramtypeService) {
   this.progamRequest=new ProgramRequest();
    this.programId=0;
   }

  ngOnInit() {
    this.ValidationAddFormControl();
   
     this.loadprogramData();
     
     

    this.cols = [
      { field: 'id', header: 'Program ID' },
      { field: "name", header: "Name" },

    ];
    this.statuses = [
      { label: 'INSTOCK', value: 'instock' },
      { label: 'LOWSTOCK', value: 'lowstock' },
      { label: 'OUTOFSTOCK', value: 'outofstock' }
    ];
    this.exportColumns = this.cols.map(col => (col.header));

  }
  loadprogramData() {

    this._service.getAllprogram().subscribe((response:any) => 
    {
      debugger

      //console.log('Shift data received:', response);
      this.programResponse = response;
      this.progamRequest.id=response.id;
   },
   (error) => {
      console.error('Error fetching program data:', error);
   }
);
}
openNew() {
  this.isEditing = true; // Add mode
  this.loadProgramtype();
    this.addprogramForm.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true; 
}

ValidationAddFormControl(){
this.addprogramForm = this.fb.group({
  name: ['', Validators.required], 
programTypeId:[null, Validators.required]
 
});
}
loadProgramtype(){

  this._programtypeService.getAllprogramtype().subscribe((response:any) => 
    {
      debugger
      //console.log('Shift data received:', response)
      this.programtypeResponse = response;
   }
);
}
hideDialog() {
  this.addDialog = false;
  this.submitted = false;
}
isDeleted(response:any) {
    this.deleteDialog = true;
    this.programId=response.id;
    
  }
editDetails(){

  if (this.addprogramForm.valid) {

   this.progamRequest=this.addprogramForm.value;
   this.progamRequest.id=this.programId;
    this._service.updateProgram(this.progamRequest).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'program Added', life: 3000 });
      this.loadprogramData();  // Refresh the list
      this.hideDialog();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'program could not be added', life: 3000 });
    });
  }
  this.addprogramForm.markAllAsTouched();
}
saveDetails() {
  this.submitted = true;
  if (this.addprogramForm.valid) {
   
    this._service.addProgram(this.addprogramForm.value).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'program Added', life: 3000 });
      this.loadprogramData();  // Refresh the list
      this.hideDialog();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'program could not be added', life: 3000 });
    });
  }
  this.addprogramForm.markAllAsTouched();
}
  
showEditModal(response:any){
    this.addDialog=true;
    this.submitted=true;
    this.isEditing=false;
    this.loadProgramtype();
    debugger
    this.addprogramForm.patchValue(response);
    this.programId=response.id;
  }

  confirmDelete() {
 
    this._service.deleteProgram(this.programId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'program Deleted', life: 3000 });
      this.loadprogramData();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'program could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
  }
  findIndexById(id: string): number {
      let index = -1;
      for (let i = 0; i < this.programResponse.length; i++) {
          if (this.programResponse[i].id === id) {
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
      const worksheet = xlsx.utils.json_to_sheet(this.programResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "program");
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
        const dataRows = this.programResponse.map(program => [
          program.id,
          program.name,
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
        doc.save('program.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this._service.getAllProgramByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.programResponse = data.data;
    })
  }

}
