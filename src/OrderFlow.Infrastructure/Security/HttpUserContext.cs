using Microsoft.AspNetCore.Http;
using OrderFlow.Application.Interfaces;

namespace OrderFlow.Infrastructure.Security;

public class HttpUserContext(IHttpContextAccessor httpContextAccessor): IUserContext
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;
    public string? IpAddress => httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
}
