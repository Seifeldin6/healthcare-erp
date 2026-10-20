using Xunit;

namespace Erp.Tests.Fixtures;

[CollectionDefinition("IntegrationTests", DisableParallelization = true)]
public class TestCollection : ICollectionFixture<ErpTestWebApplicationFactory>
{
}
