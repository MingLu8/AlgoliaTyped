using Algolia.Search.Clients;
using Xunit.Extensions.AssemblyFixture;

[assembly: TestFramework(AssemblyFixtureFramework.TypeName, AssemblyFixtureFramework.AssemblyName)]
namespace AlgoliaTypedIntegrationTests
{
    public class AlgoliaFixture : IDisposable
    {
        public SearchClient SearchClient { get; private set; }
        public AlgoliaFixture()
        {
            var applicationId = "FQYQ8IBGN5";
            var apiKey = "7a36900fc5bbf76a0857484917812787";
            SearchClient = new SearchClient(applicationId, apiKey);
        }

        public void Dispose()
        {
            
        }
    }
}