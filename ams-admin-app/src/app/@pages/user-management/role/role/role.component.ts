import { Component } from '@angular/core';
import { RoleService } from '../../../../@core/services/role/role.service';
import { Role } from '../../../../@core/api/auth/role';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';

function mapPermissions(permissionArray: string[]) {
  return permissionArray.map((permission) => {
    const parts = permission.split(".");
    const module = parts[1];
    const action = parts[2];

    // Generate a readable name
    const name = `${action.charAt(0).toUpperCase() + action.slice(1)} ${module.charAt(0).toUpperCase() + module.slice(1)
      }`.replace(/([A-Z])/g, " $1").trim();

    // Generate a description
    const description = `Can ${action.toLowerCase()} ${module.charAt(0).toUpperCase() + module.slice(1)
      }`;

    return {
      id: permission,
      name: name,
      description: description,
      module: module,
    };
  });
}

interface Permission {
  id: string;
  name: string;
  description: string;
  module: string;
}

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrl: './role.component.scss'
})
export class RoleComponent {
  selectedRole!: Role;
  searchQuery: string = '';
  selectedModule: string = '';

  permissions: Permission[] = []

  roles: Role[] = [];

  modules: string[] = [];

  filteredPermissions: Permission[] = [];

  submitted: boolean = false;
  isLoading: boolean = false;
  userProfilePlaceHolder: string = "assets/images/public.png"
  roleFormGroup: FormGroup;
  isEditing: boolean = false;
  deleteDialog: boolean = false;
  addDialog: boolean = false;
  roleId!: number
  constructor(private rolesService: RoleService, private fb: FormBuilder, private messageService: MessageService) {
    this.roleFormGroup = this.fb.group({
      id: [null],
      name: ['', Validators.required],
      description: ['', Validators.required],
    })
  }
  ngOnInit(): void {
    this.loadAllRoles();
    this.loadAllPermissions();
  }
  // Http
  loadAllRoles() {
    this.rolesService.getAllRolesWithPermissions().subscribe((response) => {
      this.roles = response;
      this.selectedRole = this.roles[0];
    })
  }
  loadAllPermissions() {
    this.rolesService.getAllPermission().subscribe((response) => {
      this.permissions = mapPermissions(response);
      this.modules = [...new Set<string>(this.permissions.map<string>(x => x.module))]
      this.filteredPermissions = [...this.permissions];
    })
  }
  selectRole(role: Role): void {
    this.selectedRole = role;
  }
  isPermissionEnabled(permissionId: string): boolean {
    return this.selectedRole.permissions.includes(permissionId);
  }
  togglePermission(permissionId: string): void {
    const index = this.selectedRole.permissions.indexOf(permissionId);
    if (index === -1) {
      this.selectedRole.permissions.push(permissionId);
    } else {
      this.selectedRole.permissions.splice(index, 1);
    }
  }
  filterPermissions(): void {
    this.filteredPermissions = this.permissions.filter(permission => {
      const matchesSearch = permission.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
        permission.description.toLowerCase().includes(this.searchQuery.toLowerCase());
      //  const matchesModule = !this.selectedModule || permission.module === this.selectedModule;
      return matchesSearch;
    });
  }
  onSearch(): void {
    this.filterPermissions();
  }
  onModuleChange(): void {
    this.filterPermissions();
  }
  saveChanges(): void {
    // Implement save logic here
    console.log('Saving changes for role:', this.selectedRole);
    this.rolesService.updateRolePermissions(this.selectedRole.id, { id: this.selectedRole.id, permissions: this.selectedRole.permissions }).subscribe(() => {
      this.loadAllRoles();
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Role Permissions Updated', life: 3000 });

    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || error.detail || 'Role permissions could not be updated', life: 3000 });
    });
  }
  // Edit, Delete, Add
  openNew() {
    this.isEditing = true; // Add mode
    this.roleFormGroup.reset(); // Reset the form for new entry
    this.submitted = false; // Reset submitted flag
    this.addDialog = true;
  }
  filterText(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    const filteredValue = inputElement.value.replace(/[^a-zA-Z\s]/g, ''); // Allow only letters and spaces
    inputElement.value = filteredValue;
    this.roleFormGroup.get('userName')?.setValue(filteredValue, { emitEvent: true }); // Update the form control value
  }
  hideDialog() {
    this.addDialog = false;
    this.submitted = false;
  }
  isDeleted(response: any) {
    this.deleteDialog = true;
    this.roleId = response.id;
  }
  editDetails() {
    this.submitted = true;
    if (this.roleFormGroup.valid) {
      this.roleFormGroup.setValue({ id: this.selectedRole.id, ...this.roleFormGroup.value })
      this.rolesService.updateRole(this.roleFormGroup.value, this.selectedRole.id).subscribe(() => {
        this.loadAllRoles();
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: 'Role Updated', life: 3000 });
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || error.detail || 'Role could not be Updated', life: 3000 });
      });
    }
    this.roleFormGroup.markAllAsTouched();
  }
  saveDetails() {
    debugger
    this.submitted = true;
    // alert(JSON.stringify(this.roleFormGroup.errors))
    if (this.roleFormGroup.valid) {
      this.rolesService.createRole(this.roleFormGroup.value).subscribe((response) => {
        this.loadAllRoles();
        this.messageService.add({ severity: 'success', summary: 'Successful', detail: response.message || 'Role Created', life: 3000 });
        this.hideDialog();
      }, error => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || error.detail || 'Role could not be added', life: 3000 });
      });
    }
    this.roleFormGroup.markAllAsTouched();
  }
  confirmDelete() {

    this.rolesService.deleteRole(this.roleId).subscribe((res) => {
      this.messageService.add({ severity: 'success', summary: 'Successful', detail: res.message || res.detail || 'Role Deleted', life: 3000 });
      this.loadAllRoles();
    }, error => {
      this.messageService.add({ severity: 'error', summary: 'Error', detail: error.message || error.detail || 'Role could not be deleted', life: 3000 });
    });
    this.deleteDialog = false;
  }
  showEditModal(response: any) {
    this.addDialog = true;
    this.submitted = true;
    this.isEditing = false;
    debugger
    this.roleFormGroup.patchValue({
      ...response
    });
    this.roleId = response.id;
  }
}
