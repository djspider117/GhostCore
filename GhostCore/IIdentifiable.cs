namespace GhostCore
{
    public interface IIdentifiable
    {
        int Id { get; set; }
    }
    public interface INamed
    {
        string Name { get; }
    }

    public interface INamedIdentified : IIdentifiable, INamed
    {
    }
}
