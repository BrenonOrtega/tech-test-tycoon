namespace TechTest.Ryanair.Tycoon.Application.CreateWorker
{
    public class CreatedWorkerResponse
    {
        public CreatedWorkerResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; init; }

        public static readonly CreatedWorkerResponse Null = new(Guid.Empty);
    }
}