using Microsoft.AspNetCore.Authorization;

namespace Chapter08.Authorization;

public class MaintenanceTimeAuthorizationHandler : AuthorizationHandler<MaintenanceTimeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MaintenanceTimeRequirement requirement)
    {
        var isAuthorized = true;
        if (!context.User.IsInRole("Administrator"))
        {
            var time = TimeOnly.FromDateTime(DateTime.Now);
            if (time >= requirement.StartTime && time < requirement.EndTime)
            {
                isAuthorized = false;
            }
        }

        if (isAuthorized)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}

