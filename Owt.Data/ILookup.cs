namespace Owt.Data
{
    public interface ILookup : IDeletable
    {
        int Id { get; }

        string Name { get; }
    }
}