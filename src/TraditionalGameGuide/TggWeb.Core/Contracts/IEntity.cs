

namespace TggWeb.Core.Contracts
{
    public interface IEntity
    {
        int Id { get; set; }
        string UrlSlug { get; set; }
    }
}
