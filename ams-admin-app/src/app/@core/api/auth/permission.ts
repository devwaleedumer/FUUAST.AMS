export const Permission = [
  "Permissions.Dashboard.View",
  "Permissions.Users.View",
  "Permissions.Users.Search",
  "Permissions.Users.Create",
  "Permissions.Users.Update",
  "Permissions.Users.Delete",
  "Permissions.Users.Export",
  "Permissions.UserRoles.View",
  "Permissions.UserRoles.Update",
  "Permissions.Roles.View",
  "Permissions.Roles.Create",
  "Permissions.Roles.Update",
  "Permissions.Roles.Delete",
  "Permissions.RoleClaims.View",
  "Permissions.RoleClaims.Update",
  "Permissions.ApplicationForms.View",
  "Permissions.ApplicationForms.Search",
  "Permissions.ApplicationForms.Create",
  "Permissions.ApplicationForms.Update",
  "Permissions.ApplicationForms.Delete",
  "Permissions.ApplicationForms.Export",
  "Permissions.Shift.Create",
  "Permissions.Shift.View",
  "Permissions.Shift.Update",
  "Permissions.Shift.Delete",
  "Permissions.Faculty.View",
  "Permissions.Faculty.Create",
  "Permissions.Faculty.Update",
  "Permissions.Faculty.Delete",
  "Permissions.Program.View",
  "Permissions.Program.Create",
  "Permissions.Program.Update",
  "Permissions.Program.Delete",
  "Permissions.Department.View",
  "Permissions.Department.Create",
  "Permissions.Department.Update",
  "Permissions.Department.Delete",
  "Permissions.Session.View",
  "Permissions.Session.Create",
  "Permissions.Session.Update",
  "Permissions.Session.Delete",
  "Permissions.AcademicYear.View",
  "Permissions.AcademicYear.Create",
  "Permissions.AcademicYear.Update",
  "Permissions.AcademicYear.Delete",
  "Permissions.ProgramType.View",
  "Permissions.ProgramType.Create",
  "Permissions.ProgramType.Update",
  "Permissions.ProgramType.Delete",
  "Permissions.AppicantDetail.Delete",
  "Permissions.AppicantDetail.Update",
  "Permissions.AppicantDetail.View",
  "Permissions.AppicantDetail.Create",





] as const;
export type Permission = typeof Permission[number];
