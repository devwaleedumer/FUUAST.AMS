using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SHARED.Constants.Authorization
{
    public static class AMSAction
    {
        public const string View = nameof(View);
        public const string Search = nameof(Search);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string Export = nameof(Export);
        public const string Generate = nameof(Generate);
        public const string Clean = nameof(Clean);
    }

    public static class AMSResource
    {
        public const string Dashboard = nameof(Dashboard);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string Shift = nameof(Shift);
        public const string Faculty = nameof(Faculty);
        public const string Program = nameof(Program);
        public const string Department = nameof(Department);
        public const string Session = nameof(Session);
        public const string AcademicYear = nameof(AcademicYear);
        public const string ProgramType = nameof(ProgramType);
        public const string AppicantDetail = nameof(AppicantDetail);
        public const string RoleClaims = nameof(RoleClaims);
        public const string ApplicationForms = nameof(ApplicationForms);
    }

    public static class AMSPermissions
    {
        private static readonly AMSPermission[] _all = new AMSPermission[]
        {
        new("View Dashboard", AMSAction.View, AMSResource.Dashboard),
        new("View Users", AMSAction.View, AMSResource.Users),
        new("Search Users", AMSAction.Search, AMSResource.Users),
        new("Create Users", AMSAction.Create, AMSResource.Users),
        new("Update Users", AMSAction.Update, AMSResource.Users),
        new("Delete Users", AMSAction.Delete, AMSResource.Users),
        new("Export Users", AMSAction.Export, AMSResource.Users),
        new("View UserRoles", AMSAction.View, AMSResource.UserRoles),
        new("Update UserRoles", AMSAction.Update, AMSResource.UserRoles),
        new("View Roles", AMSAction.View, AMSResource.Roles),
        new("Create Roles", AMSAction.Create, AMSResource.Roles),
        new("Update Roles", AMSAction.Update, AMSResource.Roles),
        new("Delete Roles", AMSAction.Delete, AMSResource.Roles), 
        new("View Shift", AMSAction.View, AMSResource.Shift),
        new("Create Shift", AMSAction.Create, AMSResource.Shift),
        new("Update Shift", AMSAction.Update, AMSResource.Shift),
        new("Delete Shift", AMSAction.Delete, AMSResource.Shift), 
        new("View Faculty", AMSAction.View, AMSResource.Faculty),
        new("Create Faculty", AMSAction.Create, AMSResource.Faculty),
        new("Update Faculty", AMSAction.Update, AMSResource.Faculty),
        new("Delete Faculty", AMSAction.Delete, AMSResource.Faculty), 
        new("View Program", AMSAction.View, AMSResource.Program),
        new("Create Program", AMSAction.Create, AMSResource.Program),
        new("Update Program", AMSAction.Update, AMSResource.Program),
        new("Delete Program", AMSAction.Delete, AMSResource.Program), 
        new("View Department", AMSAction.View, AMSResource.Department),
        new("Create Department", AMSAction.Create, AMSResource.Department),
        new("Update Department", AMSAction.Update, AMSResource.Department),
        new("Delete Department", AMSAction.Delete, AMSResource.Department),
        new("View Session", AMSAction.View, AMSResource.Session),
        new("Create Session", AMSAction.Create, AMSResource.Session),
        new("Update Session", AMSAction.Update, AMSResource.Session),
        new("Delete Session", AMSAction.Delete, AMSResource.Session),
        new("View AcademicYear", AMSAction.View, AMSResource.AcademicYear),
        new("Create AcademicYear", AMSAction.Create, AMSResource.AcademicYear),
        new("Update AcademicYear", AMSAction.Update, AMSResource.AcademicYear),
        new("Delete AcademicYear", AMSAction.Delete, AMSResource.AcademicYear),
        new("View ProgramType", AMSAction.View, AMSResource.ProgramType),
        new("Create ProgramType", AMSAction.Create, AMSResource.ProgramType),
        new("Update ProgramType", AMSAction.Update, AMSResource.ProgramType),
        new("Delete ProgramType", AMSAction.Delete, AMSResource.ProgramType),
        new("View AppicantDetail", AMSAction.View, AMSResource.AppicantDetail),
        new("Create AppicantDetail", AMSAction.Create, AMSResource.AppicantDetail),
        new("Update AppicantDetail", AMSAction.Update, AMSResource.AppicantDetail),
        new("Delete AppicantDetail", AMSAction.Delete, AMSResource.AppicantDetail),
        new("View RoleClaims", AMSAction.View, AMSResource.RoleClaims),
        new("Update RoleClaims", AMSAction.Update, AMSResource.RoleClaims),
        new("View ApplicationForms", AMSAction.View, AMSResource.ApplicationForms, IsBasic: true),
        new("Search ApplicationForms", AMSAction.Search, AMSResource.ApplicationForms),
        new("Create ApplicationForms", AMSAction.Create, AMSResource.ApplicationForms),
        new("Update ApplicationForms", AMSAction.Update, AMSResource.ApplicationForms,IsBasic: true),
        new("Delete ApplicationForms", AMSAction.Delete, AMSResource.ApplicationForms),
        new("Export ApplicationForms", AMSAction.Export, AMSResource.ApplicationForms),

   
        };

        public static IReadOnlyList<AMSPermission> All { get; } = new ReadOnlyCollection<AMSPermission>(_all);
        public static IReadOnlyList<AMSPermission> Root { get; } = new ReadOnlyCollection<AMSPermission>(_all.Where(p => p.IsRoot).ToArray());
        public static IReadOnlyList<AMSPermission> Admin { get; } = new ReadOnlyCollection<AMSPermission>(_all.Where(p => !p.IsRoot).ToArray());
        public static IReadOnlyList<AMSPermission> Basic { get; } = new ReadOnlyCollection<AMSPermission>(_all.Where(p => p.IsBasic).ToArray());
    }

    public record AMSPermission(string Description, string Action, string Resource, bool IsBasic = false, bool IsRoot = false)
    {
        public string Name => NameFor(Action, Resource);
        public static string NameFor(string action, string resource) => $"Permissions.{resource}.{action}";
    }
}
