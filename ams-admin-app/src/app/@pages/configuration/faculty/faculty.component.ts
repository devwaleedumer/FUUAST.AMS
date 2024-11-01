import {Component, OnInit, ViewChild} from '@angular/core';
import { MessageService } from 'primeng/api';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import { Product } from '../../../@core/api/dashboard/product';
import { ProductService } from '../../../@core/services/dashboard/product.service';
import { FacultyService } from '../../../@core/services/faculty/faculty.service';
import { Faculty } from '../../../@core/api/configuration/faculty/faculty';
import autoTable from 'jspdf-autotable';

@Component({
  templateUrl: 'faculty.component.html',
  providers: [MessageService,]
})
export class FacultyComponent implements OnInit {

  // Reference to the PrimeNG table
  @ViewChild('dt') table!: Table;

  facultyDialog: boolean = false;

  editFacultyDialog: boolean = false;

  deleteFacultyDialog: boolean = false;

  deleteFacultiesDialog: boolean = false;

  faculties: Faculty[] = [];

  faculty: Partial<Faculty> = {};

  selectedFaculty: Partial<Faculty> = {};

  submitted: boolean = false;

  cols: any[] = [];

  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];
  isLoading : boolean = false;

  constructor(private facultyService: FacultyService, private messageService: MessageService) { }

  ngOnInit() {
    this.cols = [
      { field: 'id', header: 'Faculty ID' },
      { field: "name", header: "Name" },

    ];
    this.exportColumns = this.cols.map(col => (col.header));
  }
  openNew() {
    this.faculty = {};
    this.submitted = false;
    this.facultyDialog = true;
  }
  deleteSelectedFaculty() {
    this.deleteFacultiesDialog = true;
  }

  showEditFacultyModal(faculty: Faculty) {
    this.faculty = {...faculty} ;
    this.submitted = false;
    this.editFacultyDialog = true;
  }
  editFaculty() {
    if (this.faculty.name?.trim()) {
      this.facultyService.updateFaculty(this.faculty,this.faculty.id!)
        .subscribe((data) => {
          const index  = this.faculties.findIndex(f => f.id == data.id)
          this.faculties[index].name = data.name;
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Faculty Updated', life: 3000 });
        });
      this.submitted = true;
  }
      this.editFacultyDialog = false;
      this.faculty = {};
  }
  deleteFaculty(faculty: Faculty) {
    this.deleteFacultyDialog = true;
    this.faculty = { ...faculty };
  }
  confirmDelete() {
    this.deleteFacultyDialog = false;
    if (this.faculty.id) {
       this.facultyService.deleteFaculty(this.faculty.id).subscribe( {next: (data) => {
         this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Faculty Deleted', life: 3000 });
           this.table.reset()
           this.faculty = {};
       }
         ,error: (error: any) => {
         console.log(error)
           this.messageService.add({ severity: 'error', summary: 'Error', detail: error ??  `Faculty couldn't be deleted right now` });
           this.faculty = {};
         }
       })
    }
  }

  hideDialog() {
    this.facultyDialog = false;
    this.submitted = false;
  }
  hideEditDialogue() {
    this.editFacultyDialog = false;
    this.submitted = false;
  }
  saveFaculty() {
    this.submitted = true;
    if (this.faculty.name?.trim()) {
      this.facultyService.createFaculty(this.faculty)
                         .subscribe((data) =>
                         {
                           this.faculties = [...this.faculties,data]
                           this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Faculty Created', life: 3000 });
                         });
      this.facultyDialog = false;
      this.faculty = {};
    }
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
      const worksheet = xlsx.utils.json_to_sheet(this.faculties);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "faculties");
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
        const dataRows = this.faculties.map(faculty => [
          faculty.id,
          faculty.name,
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
        doc.save('faculties.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this.isLoading = true;
    this.facultyService.getAllFacultyByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.faculties = data.data;
      this.isLoading = false;
    })
  }
}
