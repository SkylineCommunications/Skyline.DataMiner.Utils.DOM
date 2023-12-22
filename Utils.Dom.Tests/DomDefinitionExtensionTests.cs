namespace Utils.Dom.Tests
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
			var mock = TestData.DomHelper;

			var definition = mock.DomDefinitions.GetByID(TestData.Definition1.ID.Id);

			definition.Should().Be(TestData.Definition1);
		}

		[TestMethod]
		public void DomDefinition_GetByName()
		{
			var mock = TestData.DomHelper;

			var definition = mock.DomDefinitions.GetByName(TestData.Definition1.Name);

			definition.Should().Be(TestData.Definition1);
		}
	}
}
