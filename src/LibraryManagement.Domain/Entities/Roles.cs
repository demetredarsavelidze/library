namespace LibraryManagement.Domain.Entities;

public static class Roles
{
    public const string Admin = "admin";
    public const string User = "user";

    public static readonly string[] Supported = [Admin, User];
}
