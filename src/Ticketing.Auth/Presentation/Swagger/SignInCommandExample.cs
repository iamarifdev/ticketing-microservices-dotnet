using Swashbuckle.AspNetCore.Filters;
using Ticketing.Auth.Application.Commands;

namespace Ticketing.Auth.Presentation.Swagger;

public class SignInCommandExample : IExamplesProvider<SignInCommand>
{
    public SignInCommand GetExamples()
    {
        return new SignInCommand("test@test.com", "123456");
    }
}
