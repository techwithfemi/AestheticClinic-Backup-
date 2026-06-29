// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

using AestheticEMR.Core.Models.Account;
using System.Collections.ObjectModel;

namespace AestheticEMR.Core.Services.Account
{
    public static class ApplicationPermissions
    {
        /************* USER PERMISSIONS *************/

        public const string UsersPermissionGroupName = "User Permissions";

        public static readonly ApplicationPermission ViewUsers = new(
            "View Users",
            "users.view",
            UsersPermissionGroupName,
            "Permission to view other users account details");

        public static readonly ApplicationPermission ManageUsers = new(
            "Manage Users",
            "users.manage",
            UsersPermissionGroupName,
            "Permission to create, delete and modify other users account details");

        /************* ROLE PERMISSIONS *************/

        public const string RolesPermissionGroupName = "Role Permissions";

        public static readonly ApplicationPermission ViewRoles = new(
            "View Roles",
            "roles.view",
            RolesPermissionGroupName,
            "Permission to view available roles");

        public static readonly ApplicationPermission ManageRoles = new(
            "Manage Roles",
            "roles.manage",
            RolesPermissionGroupName,
            "Permission to create, delete and modify roles");

        public static readonly ApplicationPermission AssignRoles = new(
            "Assign Roles",
            "roles.assign",
            RolesPermissionGroupName,
            "Permission to assign roles to users");

        /************* MANAGEMENT PERMISSIONS *************/

        public const string ManagementPermissionGroupName = "Management Permissions";

        public static readonly ApplicationPermission ViewAuditLogs = new(
            "View Audit Logs",
            "management.audit.view",
            ManagementPermissionGroupName,
            "Permission to view audit trail reports");

        public static readonly ApplicationPermission ViewAccounting = new(
            "View Accounting",
            "accounting.view",
            ManagementPermissionGroupName,
            "Permission to view accounting records (journal entries, expenses, incomes, etc.)");

        public static readonly ApplicationPermission ManageAccounting = new(
            "Manage Accounting",
            "accounting.manage",
            ManagementPermissionGroupName,
            "Permission to create, edit and delete accounting records");

        public static readonly ApplicationPermission ViewEmployees = new(
            "View Employees",
            "employees.view",
            ManagementPermissionGroupName,
            "Permission to view employee records");

        public static readonly ApplicationPermission ManageEmployees = new(
            "Manage Employees",
            "employees.manage",
            ManagementPermissionGroupName,
            "Permission to create, edit and delete employee records");

        /************* ALL PERMISSIONS *************/

        public static readonly ReadOnlyCollection<ApplicationPermission> AllPermissions =
            new List<ApplicationPermission> {
                ViewUsers, ManageUsers,
                ViewRoles, ManageRoles, AssignRoles,
                ViewAuditLogs,
                ViewAccounting, ManageAccounting,
                ViewEmployees, ManageEmployees
            }.AsReadOnly();

        /************* HELPER METHODS *************/

        public static ApplicationPermission? GetPermissionByName(string? permissionName)
        {
            return AllPermissions.SingleOrDefault(p => p.Name == permissionName);
        }

        public static ApplicationPermission? GetPermissionByValue(string? permissionValue)
        {
            return AllPermissions.SingleOrDefault(p => p.Value == permissionValue);
        }

        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
        }

        public static string[] GetAdministrativePermissionValues()
        {
            return [ManageUsers, ManageRoles, AssignRoles, ViewAuditLogs];
        }
    }
}
