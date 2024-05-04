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
        public const string RoleClaims = nameof(RoleClaims);
        public const string ApplicationForms = nameof(ApplicationForms);
        public const string Brands = nameof(Brands);
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
