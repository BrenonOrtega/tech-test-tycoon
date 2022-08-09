using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain;

namespace TechTest.Ryanair.Tycoon.Application;

public class ApplicationErrors : BaseError
{
    public static readonly Error NullCommand = Create("NULL_COMMAND_ERROR", "Provided command data is null, please verify and try again.");
    public static readonly Error InvalidCommand = Create("INVALID_COMMAND", "One or more validations failed, please verify command fields and try again.");
    public static readonly Error WorkerNotFound = Create("WORKER_NOT_FOUND", "The Provided worker was not found.");
    public static readonly Error InvalidScheduleActivityCommand = Create("INVALID_SCHEDULE_COMMAND", "Provided worker Id or activity is invalid");
    public static readonly Error InvalidGuid = Create("INVALID_GET_WORKER_COMMAND", "Provided worker Id is invalid");
}

