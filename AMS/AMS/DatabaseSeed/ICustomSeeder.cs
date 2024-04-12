namespace AMS.DatabaseSeed
{
    public interface ICustomSeeder
    {
        Task InitializeAsync(CancellationToken cancellationToken);
    }
}
