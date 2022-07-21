using Microsoft.AspNetCore.Authorization;

namespace Chapter08.Authorization;

public class MaintenanceTimeRequirement : IAuthorizationRequirement
{
    public TimeOnly StartTime { get; init; }

    public TimeOnly EndTime { get; init; }
}
