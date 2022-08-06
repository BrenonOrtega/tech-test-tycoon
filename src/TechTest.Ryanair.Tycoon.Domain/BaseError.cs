using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Domain
{
    public class BaseError
    {
        public static Error Create(string code, string reason) => Error.Create(code, reason);
    }
}