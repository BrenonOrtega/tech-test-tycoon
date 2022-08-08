using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application;

public interface ICommand
{
    Result Validate();
}
