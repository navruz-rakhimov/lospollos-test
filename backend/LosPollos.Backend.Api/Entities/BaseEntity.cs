namespace LosPollos.Backend.Api.Entities;

public class BaseEntity<TKey> where TKey : struct
{
    public TKey Id { get; set; }
}