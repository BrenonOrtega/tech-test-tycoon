﻿namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;

public class CreatedActivityResponse
{
    public CreatedActivityResponse(Guid id, DateTime startDate, DateTime finishDate, string restPeriod, string type)
    {
        Id = id;
        StartDate = startDate;
        FinishDate = finishDate;
        RestPeriod = restPeriod;
        Type = type;
    }

    public Guid Id { get; }
    public DateTime FinishDate { get; }
    public DateTime StartDate { get; }
    public string RestPeriod { get; }
    public string Type { get; }

    public static readonly CreatedActivityResponse Null = new(Guid.Empty, DateTime.MinValue, DateTime.MaxValue, "NO_TIME", "None");
}