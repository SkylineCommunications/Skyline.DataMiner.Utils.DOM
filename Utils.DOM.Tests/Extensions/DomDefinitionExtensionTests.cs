namespace Skyline.DataMiner.Utils.DOM.Tests.Extensions
{
	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class DomDefinitionExtensionTests
	{
		[TestMethod]
		public void DomDefinition_GetByID()
		{
			var testData = new TestData();
			var mock = testData.DomHelper;

			var definition = mock.DomDefinitions.GetByID(TestData.Definition1.ID.Id);

			definition.Should().Be(TestData.Definition1);
		}

		[TestMethod]
		public void DomDefinition_GetByName()
		{
			var testData = new TestData();
			var mock = testData.DomHelper;

			var definition = mock.DomDefinitions.GetByName(TestData.Definition1.Name);

			definition.Should().Be(TestData.Definition1);
		}
	}
}
