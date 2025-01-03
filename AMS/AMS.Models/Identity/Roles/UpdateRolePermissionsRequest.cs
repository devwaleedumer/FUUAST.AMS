


namespace AMS.MODELS.Identity.Roles
{
    public class UpdateRolePermissionsRequest
    {
        public int Id { get; set; } = default!;
        public List<string> Permissions { get; set; } = default!;
    }

   
}
