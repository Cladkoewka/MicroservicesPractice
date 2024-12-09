namespace MessageContracts;

public record PaymentProcessed
{
    public Guid OrderId { get; init; }
    public bool IsSuccess { get; init; }
    public string Reason { get; init; } = default!;
}