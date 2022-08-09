﻿using TechTest.Ryanair.Tycoon.Application.Dtos;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

public class FoundActivityResponse
{
    public FoundActivityResponse(ActivityDto activityDto) => Activity = activityDto;

    public ActivityDto Activity { get; }

    public static readonly FoundActivityResponse Null = new(ActivityDto.Null);
}