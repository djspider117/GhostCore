namespace GhostCore
{
    public interface IIdentifiable
    {
        int Id { get; set; }
    }
    public interface INamed
    {
        string Name { get; set; }
    }

    public interface INamedIdentifiable : IIdentifiable, INamed
    {
    }
}
