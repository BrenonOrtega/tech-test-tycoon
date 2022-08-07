using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain;

namespace TechTest.Ryanair.Tycoon.Application;

public class ApplicationErrors : BaseError
{
    public static readonly Error NullCommand = Create("NULL_COMMAND_ERROR", "Provided command data is null, please verify and try again.");
    public static readonly Error InvalidCommand = Create("INVALID_COMMAND", "One or more validations failed, please verify command fields and try again.");
}
