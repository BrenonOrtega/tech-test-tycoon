using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain.FluentApi.Activity
{
    public class WorksInResult : Result<IActivityWorker>
    {
        public WorksInResult(IActivityWorker worker) : base(worker) { }

        public WorksInResult(Error error) : base(error) { }
    }
}