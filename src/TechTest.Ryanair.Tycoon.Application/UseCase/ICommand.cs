using Awarean.Sdk.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTest.Ryanair.Tycoon.Application.UseCase
{
    public interface ICommand
    {
        Result Validate();
    }
}
