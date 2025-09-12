namespace OrderFlow.Application.Interfaces;

public interface IUserContext
{
    string? UserId { get; }
    string? IpAddress { get; }
}
