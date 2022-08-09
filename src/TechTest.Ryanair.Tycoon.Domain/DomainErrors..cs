using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain;

public class DomainErrors : BaseError
{
    public static readonly Error OverlappingActivities = Create("OVERLAPING_ACTIVITIES", "One or more requested activities overlaps each other");
    public static readonly Error AlreadyWorksInActivity = Create("WORKER_ALREADY_SCHEDULED", "The worker already works in that activity");
    public static readonly Error InconsistentWorkerInActivity = Create("INCONSISTENT_WORKER_ACTIVITY", "The worker added to the activity cannot work in it either for rest time or because is already working in another activity");
    public static readonly Error AddingActivityToNullWorker = Create("ADDING_ACTIVITY_TO_NULL_WORKER", "Cannot assign activities to a null worker");
    public static readonly Error TryWorkingInInvalidActivity  = Create("WORKER_INVALID_ACTIVITY", "Worker cannot work in an invalid activity.");
    public static readonly Error InvalidActivityAssignment = Create("INVALID_ACTIVITY_ASSIGNMENT", "Cannot Assign more than one worker to component activity.");

}
