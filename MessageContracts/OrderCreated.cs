namespace MessageContracts;

public record OrderCreated
{
    public Guid OrderId { get; init; }
    public string CustomerName { get; init; } = default!;
    public decimal Amount { get; init; }
}