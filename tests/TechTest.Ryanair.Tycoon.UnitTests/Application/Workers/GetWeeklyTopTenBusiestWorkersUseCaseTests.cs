using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTest.Ryanair.Tycoon.Application.Extensions;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;
using TechTest.Ryanair.Tycoon.Infra.Extensions;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application.Workers;

public class GetWeeklyTopTenBusiestWorkersUseCaseTests
{
    [Fact]
    public void Getting_Top_Ten_Weekly_Should_Build_Correct_Get_Busiest_Workers_Command()
    {
        var command = new GetWeeklyTopTenBusiestWorkersCommand();

        command.FinalDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10));
        command.StartDate.Should().BeCloseTo(DateTime.Now.AddDays(-7), TimeSpan.FromMilliseconds(10));
        command.Count.Should().Be(10);
    }
}
