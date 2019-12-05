using NUnit.Framework;

namespace DFC.App.ContentPages.PageService.IntegrationTests.ServiceTests.ContentPageServiceTests
{
    [TestFixture]
    public abstract class BaseContentPageServiceTests : BaseServiceTests
    {
        protected const string ValidNameValue = "unit_tests";
        protected const string ValidAlternativeNameValue = "unit_tests_alternative";
    }
}
