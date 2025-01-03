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
  "Permissions.ApplicationForms.Export"
] as const;
export type Permission = typeof Permission[number];
