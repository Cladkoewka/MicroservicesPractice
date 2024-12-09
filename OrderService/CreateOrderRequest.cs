namespace OrderService;

public record CreateOrderRequest(string CustomerName, decimal Amount);