using Shared.Kernel;
using UserManagement.Domain.Validation.City;
using UserManagement.Domain.Validation.Emails;
using UserManagement.Domain.ValueObjects;
using UserManagement.Domain.ValueObjects.Emails;

namespace Tests.UserManagement.Domain.ValueObjects;

public sealed class EmailTests
{
    [Fact]
    public void Email_WhenCreatedWithValidValue_ShouldReturnSuccess()
    {
        const string validEmail = "test@example.com";
        IValidationRule<Email>[] rules = [new AlwaysValidRule()];

        Result<Email> result = EmailFactory.Create(validEmail, rules);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(validEmail);
    }

    [Fact]
    public void Email_WhenCreatedWithEmptyString_ShouldReturnFailure()
    {
        const string emptyEmail = "";
        IValidationRule<Email>[] rules = [new AlwaysInvalidRule()];

        Result<Email> result = EmailFactory.Create(emptyEmail, rules);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Email_WhenCreatedWithNull_ShouldReturnFailure()
    {
        IValidationRule<Email>[] rules = [new AlwaysInvalidRule()];

        Result<Email> result = EmailFactory.Create(null!, rules);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Email_WhenComparedWithSameValue_ShouldBeEqual()
    {
        const string emailValue = "test@example.com";
        IValidationRule<Email>[] rules = [new AlwaysValidRule()];
        Result<Email> result1 = EmailFactory.Create(emailValue, rules);
        Result<Email> result2 = EmailFactory.Create(emailValue, rules);

        result1.Value.Should().Be(result2.Value);
    }

    [Fact]
    public void Email_WhenComparedWithDifferentValue_ShouldNotBeEqual()
    {
        IValidationRule<Email>[] rules = [new AlwaysValidRule()];
        Result<Email> result1 = EmailFactory.Create("test1@example.com", rules);
        Result<Email> result2 = EmailFactory.Create("test2@example.com", rules);

        result1.Value.Should().NotBe(result2.Value);
    }

    [Fact]
    public void Email_WhenCreatedWithMultipleValidRules_ShouldReturnSuccess()
    {
        const string validEmail = "test@example.com";
        IValidationRule<Email>[] rules =
        [
            new NotEmptyRule<Email>(e => e.Value),
            new DomainNotEmptyRule(),
        ];

        Result<Email> result = EmailFactory.Create(validEmail, rules);

        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(validEmail);
    }

    [Fact]
    public void Email_WhenCreatedWithOneInvalidRule_ShouldReturnFailure()
    {
        const string email = "test@example.com";
        IValidationRule<Email>[] rules = [new AlwaysValidRule(), new AlwaysInvalidRule()];

        Result<Email> result = EmailFactory.Create(email, rules);

        result.IsFailure.Should().BeTrue();
    }

    private sealed class AlwaysValidRule : IValidationRule<Email>
    {
        public Result Validate(Email email) => ResultFactory.Success();
    }

    private sealed class AlwaysInvalidRule : IValidationRule<Email>
    {
        public Result Validate(Email email)
        {
            Error error = ErrorFactory.Create("Email.Invalid", "Email is invalid");
            return ResultFactory.Failure(error);
        }
    }
}
