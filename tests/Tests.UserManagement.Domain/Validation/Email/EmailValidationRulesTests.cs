using AwesomeAssertions;
using Shared.Kernel;
using UserManagement.Domain.Validation.Email;

namespace Tests.UserManagement.Domain.Validation.Email;

public sealed class EmailValidationRulesTests
{
    [Theory]
    [InlineData("email@here.com", true)]
    [InlineData("weirder-email@here.and.there.com", true)]
    [InlineData("!def!xyz%abc@example.com", true)]
    [InlineData("example@valid-----hyphens.com", true)]
    [InlineData("example@valid-with-hyphens.com", true)]
    [InlineData("test@domain.with.idn.tld.उदाहरण.परीक्षा", true)]
    [InlineData("a@atm.aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", true)]
    [InlineData("a@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.atm", true)]
    [InlineData("a@aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.bbbbbbbbbb.atm", true)]
    [InlineData("a@atm.aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", false)] // 64 chars label
    [InlineData("", false)]
    [InlineData("abc", false)]
    [InlineData("abc@", false)]
    [InlineData("abc@bar", true)]
    [InlineData("a @x.cz", false)]
    [InlineData("abc@.com", false)]
    [InlineData("something@@somewhere.com", false)]
    [InlineData("example@invalid-.com", false)]
    [InlineData("example@-invalid.com", false)]
    [InlineData("example@invalid.com-", false)]
    [InlineData("example@inv-.alid-.com", false)]
    [InlineData("example@inv-.-alid.com", false)]
    [InlineData("test@example.com\n\n<script src=\"x.js\">", false)]
    [InlineData("trailingdot@shouldfail.com.", false)]
    [InlineData("trailingdot@should..fail.com", false)]
    [InlineData("a@b.com\n", false)]
    [InlineData("a\n@b.com", false)]
    [InlineData("John.Doe@exam_ple.com", false)]
    // RFC 5321 tests
    [InlineData("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa@mail.com", false)] // 65 char local part
    public void EmailRules_WhenCombined_ShouldValidateCorrectly(string email, bool expectedValid)
    {
        IValidationRule<string>[] rules =
        [
            new EmailNotEmptyRule(),
            new ExactlyOneAtSymbolRule(),
            new DomainNotEmptyRule(),
            new LocalPartMaxLengthRule(),
            new DomainPartMaxLengthRule(),
            new NoWhitespaceRule(),
            new NoNewlineCharactersRule(),
            new NoTrailingDotRule(),
            new NoUnderscoreInDomainRule(),
            new ValidDomainHyphensRule(),
            new DomainLabelMaxLengthRule(),
            new DomainNotStartWithDotRule(),
            new NoConsecutiveDotsInDomainRule()
        ];

        RuleComposer<string> composedRule = RuleComposerFactory.Create(rules);
        Result result = composedRule.Validate(email);

        result.IsSuccess.Should().Be(expectedValid);
    }
}
