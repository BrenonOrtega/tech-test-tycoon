using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain;

public class DomainErrors : BaseError
{
    public static readonly Error OverlappingActivities = Create("OVERLAPING_ACTIVITIES", "One or more requested activities overlaps each other");
    public static readonly Error AlreadyWorksInActivity = Create("WORKER_ALREADY_SCHEDULED", "The worker already works in that activity");
}
