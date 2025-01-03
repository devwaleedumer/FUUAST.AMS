namespace AMS.MODELS.Identity.User;

public record CreateAdminUserRequest(string Email, string UserName,string role);
public record CreateAdminUserResponse(int Id);