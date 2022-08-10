using Awarean.Sdk.Result;
using System.ComponentModel.DataAnnotations;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Api.Requests
{
    public class ScheduleActivityRequest
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime? FinishDate { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public List<string> Workers { get; set; }
        public Guid? Id { get; set; }

        public Result<ScheduleActivityCommand> ToCommand()
        {
            var activity = CreateActivity();
            if (activity.IsFailed)
                return Result.Fail<ScheduleActivityCommand>(activity.Error);

            return Result.Success(new ScheduleActivityCommand(activity.Value, UseIds()));
        }

        private Guid[] UseIds()
        {
            if (Workers is null)
                return Array.Empty<Guid>();

            return Workers.Select(x => Guid.TryParse(x, out var guid) ? guid : Guid.Empty).ToArray();
        }

        // Code Smell, Should Remove it to use case or an Service for API to construct components.
        private Result<TimedActivity> CreateActivity()
        {
            var id = Id ?? Guid.NewGuid();

            if (Type == "Component")
                CORRIGIR CASO DE USO PARA SÓ RECEBER DADOS DE ATIVIDADES EXISTENTES
                return Result.Success<TimedActivity>(new BuildComponentActivity(id, (DateTime)StartDate, (DateTime)FinishDate));

            if (Type == "Machine")
                    return Result.Success<TimedActivity>(new BuildMachineActivity(id, (DateTime)StartDate, (DateTime)FinishDate));

            return Result.Fail<TimedActivity>("INVALID_ACTIVITY_TYPE", $"Activity type for type {Type} does not exist.");
        }
    }
}