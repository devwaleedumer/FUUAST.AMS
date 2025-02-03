import { Component } from '@angular/core';
import { ApplicantmanagementService } from '../../../@core/services/applicant_management/applicantmanagement.service';
import { ApplicantInfoRequest } from '../../../@core/api/applicant_management/applicantmanagement';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import autoTable from 'jspdf-autotable';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UpdateApplicantRequest } from '../../../@core/api/applicant_management/updateapplicantmanagement';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-applicant-detail',
  templateUrl: './applicant-detail.component.html',
  styleUrl: './applicant-detail.component.scss'
})
export class ApplicantDetailComponent {
  applicantFormgroup!: FormGroup
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  submitted: boolean = false;
  imageUrl: string = "";
  applicantInfoResponse: any[] = [];
  updateApplicantRequest: UpdateApplicantRequest;
  applicantInfoRequest: ApplicantInfoRequest
  cols: any[] = [];
  applicantId: number | undefined;
  exportColumns: any[] = [];
  totalRecords: number = 0;
  programType: string = '';
  statusOptions = [
    { label: 'All', value: 0 },
    { label: 'Pending', value: 1 },
    { label: 'Approved', value: 2 },
    { label: 'Rejected', value: 3 }
  ];


  constructor(private _applicantmanagementservice: ApplicantmanagementService, private fb: FormBuilder, private messageService: MessageService) {
    this.applicantInfoRequest = new ApplicantInfoRequest();
    this.updateApplicantRequest = new UpdateApplicantRequest();
  }
  ngOnInit() {
    this.getApplicantInfo();
    this.validateFormgroup();

    this.cols = [
      { field: "name", header: "Name" },
      { field: "fatherName", header: "FatherName" },
      { field: "cnic", header: "Cnic" },
      { field: "email", header: "Email" },
      { field: "verificationStatusId", header: "Status" },
    ];
    this.exportColumns = this.cols.map(col => (col.header));
  }

  validateFormgroup() {
    this.applicantFormgroup = this.fb.group({
      userName: ['', Validators.required],
      fatherName: [''],
      email: ['', [Validators.required, Validators.email]],
      dob: ['', Validators.required],
      cnic: ['', Validators.required],
      matricDegreeName: [''],
      matricBoard: [''],
      matricYear: [''],
      matricSubject: [''],
      matricRollNo: [''],
      matricTotalMarks: [''],
      matricObtainedMarks: [''],
      intermediateDegreeName: [''],
      intermediateBoard: [''],
      intermediateYear: [''],
      intermediateSubject: [''],
      intermediateRollNo: [''],
      intermediateTotalMarks: [''],
      intermediateObtainedMarks: [''],
      bachleorDegreeName: [''],
      bachleorBoard: [''],
      bachleorYear: [''],
      bachleorSubject: [''],
      bachleorRollNo: [''],
      bachleorTotalMarks: [''],
      bachleorObtainedMarks: [''],
    });
  }
  OnSearch() {
    debugger
    this.applicantInfoResponse = [];
    this._applicantmanagementservice.getAllApplicantDetail(this.applicantInfoRequest).subscribe((response: any) => {
      {
        debugger
        this.applicantInfoResponse = response;

        this.imageUrl = response.documentUrl
      }
    });
  }
  getApplicantInfo() {
    this.applicantInfoResponse = [];
    //this.filteredUserDetails = [];
    this._applicantmanagementservice.getAllApplicantDetail(this.applicantInfoRequest).subscribe((response: any) => {
      {

        this.applicantInfoResponse = response;

      }
    });
  }
  showEditModal(response: any) {
    debugger
    const backendBaseUrl = 'https://localhost:7081';
    this.programType = response.programTypeName;

    this.addDialog = true;
    this.submitted = true;
    this.isEditing = false;
    this.imageUrl = `${response.documentUrl}`;
    const dob = new Date(response.dob).toISOString().split('T')[0];
    this.applicantFormgroup.patchValue({
      ...response,
      dob: dob,

    });
    const degrees = response.degrees || [];

    const matricDegree = Array.isArray(degrees) ? degrees.find(d => d.degreeLevelId === 1) : null;
    if (matricDegree) {
      this.applicantFormgroup.patchValue({
        matricDegreeName: matricDegree.degreeName,
        matricBoard: matricDegree.boardOrUniversityName,
        matricYear: matricDegree.passingYear,
        matricSubject: matricDegree.subject,
        matricRollNo: matricDegree.rollNo,
        matricTotalMarks: matricDegree.totalMarks,
        matricObtainedMarks: matricDegree.obtainedMarks,
      });
    }
    const intermediateDegree = Array.isArray(degrees) ? degrees.find(d => d.degreeLevelId === 2) : null;
    if (intermediateDegree) {
      this.applicantFormgroup.patchValue({
        intermediateDegreeName: intermediateDegree.degreeName,
        intermediateBoard: intermediateDegree.boardOrUniversityName,
        intermediateYear: intermediateDegree.passingYear,
        intermediateSubject: intermediateDegree.subject,
        intermediateRollNo: intermediateDegree.rollNo,
        intermediateTotalMarks: intermediateDegree.totalMarks,
        intermediateObtainedMarks: intermediateDegree.obtainedMarks,
      });
    }
    const bachleorDegree = Array.isArray(degrees) ? degrees.find(d => d.degreeLevelId === 3 || d.degreeLevelId === 4) : null;
    if (bachleorDegree) {
      this.applicantFormgroup.patchValue({
        bachleorDegreeName: bachleorDegree.degreeName,
        bachleorBoard: bachleorDegree.boardOrUniversityName,
        bachleorYear: bachleorDegree.passingYear,
        bachleorSubject: bachleorDegree.subject,
        bachleorRollNo: bachleorDegree.rollNo,
        bachleorTotalMarks: bachleorDegree.totalMarks,
        bachleorObtainedMarks: bachleorDegree.obtainedMarks,
      });
    }
    this.applicantId = response.applicantId;
    this.applicantFormgroup.disable();

  }

  isDeleted(response: any) {
    this.deleteDialog = true;

  }
  confirmDelete() {
  }
  hideDialog() {
    this.addDialog = false;
    this.submitted = false;
  }
  OnReset() {
    this.applicantInfoRequest = new ApplicantInfoRequest();
    this.getApplicantInfo();
  }
  update(action: string) {
    debugger
    this.updateApplicantRequest.verificationStatusEid = action;
    this.updateApplicantRequest.applicantId = this.applicantId;
    this._applicantmanagementservice.updateApplicantDetail(this.updateApplicantRequest).subscribe(() => {
      const successMessage = action === 'APPROVED'
        ? 'Applicant approved successfully'
        : 'Applicant rejected successfully';
      this.messageService.add({
        severity: 'success',
        summary: 'Successful',
        detail: successMessage,
        life: 3000
      });
      this.getApplicantInfo();
      this.hideDialog();
    }, error => {
      const errorMessage = action === 'APPROVED'
        ? 'Error approving applicant'
        : 'Error rejecting applicant';

      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: errorMessage,
        life: 3000
      });
    });
  }



  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
  exportExcel() {
    import("xlsx").then(xlsx => {
      const worksheet = xlsx.utils.json_to_sheet(this.applicantInfoResponse);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "applicantmanagement");
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
        const dataRows = this.applicantInfoResponse.map(applicant => [
          applicant.fullName,
          applicant.fatherName,
          applicant.cnic,
          applicant.email,
          applicant.verificationStatusEid === 1 ? 'Pending' :
            applicant.verificationStatusEid === 2 ? 'Approved' :
              applicant.verificationStatusEid === 3 ? 'Rejected' : 'Unknown'


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
        doc.save('applicant.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    //   this._service.getAllProgramByFilter(event).subscribe((data) => {
    //     this.totalRecords = data.total;
    //     this.programResponse = data.data;
    // })
    // }

  }
}
