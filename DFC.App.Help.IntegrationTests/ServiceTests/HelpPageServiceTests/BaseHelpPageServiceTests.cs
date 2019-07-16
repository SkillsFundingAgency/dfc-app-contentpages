using NUnit.Framework;

namespace DFC.App.Help.IntegrationTests.ServiceTests.HelpPageServiceTests
{
    [TestFixture]
    public abstract class BaseHelpPageServiceTests : BaseServiceTests
    {
        protected const string ValidNameValue = "unit_tests";
        protected const string ValidAlternativeNameValue = "unit_tests_alternative";
    }
}
