using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain;

namespace TechTest.Ryanair.Tycoon.Infra
{
    public class InfrastructureErrors : BaseError
    {
        public static readonly Error EntityNotFound = Create("ENTITY_NOT_FOUND", "Did not found an entity with given Id");
        public static readonly Error InvalidEntityType = Create("INVALID_ENTITY", "Did not found an id for provided entity.");
        public static readonly Error NullEntity = Create("NULL_ENTITY", "Provided entity was null");
        public static readonly Error InvalidUpdateCommand = Create("INVALID_UPDATE_COMMAND", "Update entity data was null or invalid.");
    }
}
