import { Component, OnInit } from '@angular/core';
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

  productDialog: boolean = false;

  deleteProductDialog: boolean = false;

  deleteProductsDialog: boolean = false;

  faculties: Faculty[] = [];

  product: Product = {};

  selectedProducts: Product[] = [];

  submitted: boolean = false;

  cols: any[] = [];

  exportColumns: any[] = [];

  statuses: any[] = [];

  searchValue: string | undefined;
  totalRecords: number = 0;

  rowsPerPageOptions = [5, 10, 20];

  constructor(private facultyService: FacultyService, private messageService: MessageService) { }

  ngOnInit() {
    // this.loadFacultyData();

    this.cols = [
      { field: 'id', header: 'Faculty ID' },
      { field: "name", header: "Name" },

    ];

    this.statuses = [
      { label: 'INSTOCK', value: 'instock' },
      { label: 'LOWSTOCK', value: 'lowstock' },
      { label: 'OUTOFSTOCK', value: 'outofstock' }
    ];
    this.exportColumns = this.cols.map(col => (col.header));

  }
  loadFacultyData() {
    this.facultyService.getAllFaculty().subscribe((data) => {
      this.faculties = data
      console.log(data)
    });
  }
  openNew() {
    this.product = {};
    this.submitted = false;
    this.productDialog = true;
  }

  deleteSelectedProducts() {
    this.deleteProductsDialog = true;
  }

  editProduct(product: Product) {
    this.product = { ...product };
    this.productDialog = true;
  }

  deleteProduct(product: Product) {
    this.deleteProductDialog = true;
    this.product = { ...product };
  }

  confirmDeleteSelected() {
    this.deleteProductsDialog = false;
    // this.faculties = this.faculties.filter(val => !this.selectedProducts.includes(val));
    this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Products Deleted', life: 3000 });
    this.selectedProducts = [];
  }

  confirmDelete() {
    this.deleteProductDialog = false;
    // this.faculties = this.faculties.filter(val => val.id !== this.product.id);
    this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Product Deleted', life: 3000 });
    this.product = {};
  }

  hideDialog() {
    this.productDialog = false;
    this.submitted = false;
  }

  saveProduct() {
    this.submitted = true;

    if (this.product.name?.trim()) {
      if (this.product.id) {
        // @ts-ignore
        this.product.inventoryStatus = this.product.inventoryStatus.value ? this.product.inventoryStatus.value : this.product.inventoryStatus;
        // this.faculties[this.findIndexById(this.product.id)] = this.product;
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Product Updated', life: 3000 });
      } else {
        this.product.id = this.createId();
        this.product.code = this.createId();
        this.product.image = 'product-placeholder.svg';
        // @ts-ignore
        this.product.inventoryStatus = this.product.inventoryStatus ? this.product.inventoryStatus.value : 'INSTOCK';
        // this.faculties.push(this.product);
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Product Created', life: 3000 });
      }

      this.faculties = [...this.faculties];
      this.productDialog = false;
      this.product = {};
    }
  }

  // findIndexById(id: string): number {
  //     let index = -1;
  //     for (let i = 0; i < this.faculties.length; i++) {
  //         if (this.faculties[i].id === id) {
  //             index = i;
  //             break;
  //         }
  //     }
  //
  //     return index;
  // }

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
    this.facultyService.getAllFacultyByFilter(event).subscribe((data) => {
      this.totalRecords = data.total;
      this.faculties = data.data;
    })
  }
}
