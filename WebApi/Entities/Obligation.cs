namespace WebApi.Entities;

public class Obligation : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<Client> Clients { get; set; } = new();
}