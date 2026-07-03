// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

export type PermissionNames =
  'View Users' | 'Manage Users' |
  'View Roles' | 'Manage Roles' | 'Assign Roles' |
  'View Accounting' | 'Manage Accounting';

export type PermissionValues =
  'users.view' | 'users.manage' |
  'roles.view' | 'roles.manage' | 'roles.assign' |
  'accounting.view' | 'accounting.manage';

export interface Permission {
  name: PermissionNames;
  value: PermissionValues;
  groupName: string;
  description: string;
}

export class Permissions {
  public static readonly viewUsers: PermissionValues = 'users.view';
  public static readonly manageUsers: PermissionValues = 'users.manage';

  public static readonly viewRoles: PermissionValues = 'roles.view';
  public static readonly manageRoles: PermissionValues = 'roles.manage';
  public static readonly assignRoles: PermissionValues = 'roles.assign';

  public static readonly viewAccounting: PermissionValues = 'accounting.view';
  public static readonly manageAccounting: PermissionValues = 'accounting.manage';
}
