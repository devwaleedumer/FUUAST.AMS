import { Component, ViewChild } from '@angular/core';
import { Table, TableLazyLoadEvent } from 'primeng/table';
import autoTable from 'jspdf-autotable';
import { User, UserRequest } from '../../../@core/api/user-management/user/user';
import { MessageService } from 'primeng/api';
import { UserService } from '../../../@core/services/user/user.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Option } from '../../../@core/api/option';
import { RoleService } from '../../../@core/services/role/role.service';
import { Role } from '../../../@core/api/auth/role';
import { AccountService } from '../../../@core/services/account/account.service';
import { AuthService } from '../../../@core/utilities/auth-service.service';
import { RolesAndPermissionService } from '../../../@core/utilities/roles-and-permission.service';
import { ToggleRequest } from '../../../@core/api/user-management/toogle/togglerequest';
//import { ToggleRequest } from '../../../@core/api/user-management/toogle/togglerequest';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent {
  @ViewChild('dt') table!: Table;
  cols: any[] = [];
  exportColumns: any[] = [];
  statuses: any[] = [];
  totalRecords: number = 0;
  rowsPerPageOptions = [5, 10, 20];
  first: number;
  rows: number;
  searchValue: string | undefined;
  users: User[] = [];
  submitted: boolean = false;
  isLoading: boolean = false;
  userProfilePlaceHolder: string = "assets/images/public.png"
  userFormGroup: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  userId: number;
  userRequest!: UserRequest;
  roles: Option[] = []
  statusLoader: boolean = false;
  toggleRequest: ToggleRequest;
  userdetail: any;

  constructor(private _service: UserService, private _accountservice: AccountService, private _roleService: RoleService, private messageService: MessageService, private fb: FormBuilder, public _auth: AuthService, public _permission: RolesAndPermissionService) {
    this.toggleRequest = new ToggleRequest();

    this.userFormGroup = this.fb.group({
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      role: ['', // Initial value
        Validators.required],
    })
    this.userId = 0;
    this.first = 0;
    this.rows = 10;
  }
  ngOnInit() {
    this.userdetail = this._auth.User;
    this.cols = [
      { field: 'id', header: 'UserId' },
      { field: "username", header: "Username" },
      { field: "email", header: "Email" },
      { field: "role", header: "Role" },
      { field: "isActive", header: "Active" },
      { field: "isEmailConfirmed", header: "Email Confirmed" },
    ];
    this.exportColumns = this.cols.map(col => (col.header));
  }

  loadRoles() {
    debugger
    this._roleService.getAllRoles().subscribe((response: any) => {
      this.roles = response.map((role: Role) => ({ value: role.id, label: role.name }));
    }
    );
  }
  openNew() {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Users.Create")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to create users.', life: 3000 });
      return;
    }
    this.loadRoles();
    this.isEditing = true; // Add mode
    this.userFormGroup.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true;
  }
  hideDialog() {
    this.addDialog = false;
    this.submitted = false;
  }
  isDeleted(user: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Users.Delete")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to delete users.', life: 3000 });
      return;
    }
    this.deleteDialog = true;
    this.userId = user.id;
  }
  showEditModal(user: any) {
    if (!this._permission.hasRequiredPermission(this.userdetail, "Permissions.Users.Update")) {
      this.messageService.add({ severity: 'error', summary: 'Not Successful', detail: 'You do not have permission to update users.', life: 3000 });
      return;
    }
    this.addDialog = true;
    this.submitted = true;
    this.isEditing = false;
    this.loadRoles();
    debugger
    this.userFormGroup.patchValue(user);
    this.userId = user.id;
  }
  editDetails() {
    if (this.userFormGroup.valid) {
      this.userRequest = this.userFormGroup.value;
      this.userRequest.id = this.userId;
      this._service.updateUser(this.userRequest).subscribe(() => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'User Updated', life: 3000 });
        this.getFilterData({ first: this.first, last: this.rows });  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'User could not be Updated', life: 3000 });
      });
    }
    this.userFormGroup.markAllAsTouched();
  }
  filterText(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const filteredValue = inputElement.value.replace(/[^a-zA-Z\s]/g, ''); // Allow only letters and spaces
    inputElement.value = filteredValue;
    this.userFormGroup.get('userName')?.setValue(filteredValue, { emitEvent: true }); // Update the form control value
  }
  saveDetails() {
    this.submitted = true;
    if (this.userFormGroup.valid) {
      this._service.createUser(this.userFormGroup.value).subscribe((response) => {
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: response.message || 'User Created', life: 3000 });
        this.getFilterData({ first: this.first, last: this.rows });  // Refresh the list
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'User could not be added', life: 3000 });
      });
    }
    this.userFormGroup.markAllAsTouched();
  }
  confirmDelete() {
    this._service.deleteUser(this.userId).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'User Deleted', life: 3000 });
      this.getFilterData({ first: this.first, last: this.rows });  // Refresh the list
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'User could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
  }
  onGlobalFilter(table: Table, event: Event) {
    table.filterGlobal((event.target as HTMLInputElement).value, 'contains');
  }
  exportExcel() {
    import("xlsx").then(xlsx => {
      const worksheet = xlsx.utils.json_to_sheet(this.users);
      const workbook = { Sheets: { data: worksheet }, SheetNames: ["data"] };
      const excelBuffer: any = xlsx.write(workbook, {
        bookType: "xlsx",
        type: "array"
      });
      this.saveAsExcelFile(excelBuffer, "users");
    });
  }
  toggleUserStatus(user: User) {
    debugger
    this.toggleRequest.ActivateUser = !user.isActive;
    this.toggleRequest.UserId = user.id;
    this._accountservice.toggleUserStatus(this.toggleRequest).subscribe(() => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'User Status Updated', life: 3000 });
      this.getFilterData({ first: this.first, last: this.rows });  // Refresh the list
      this.statusLoader = false
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: 'User status could not be updated', life: 3000 });
      this.statusLoader = false
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
        const dataRows = this.users.map(user => [
          user.id,
          user.userName,

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
        doc.save('users.pdf');
      })
    });
  }
  getFilterData(event: TableLazyLoadEvent) {
    this.isLoading = true;
    this._service.getAllUsersByFilter(event).subscribe((data) => {
      debugger
      this.totalRecords = data.total;

      this.users = data.data;
      this.isLoading = false;
    })
  }
}
