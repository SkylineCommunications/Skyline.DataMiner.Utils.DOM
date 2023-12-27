namespace Utils.DOM.Tests.Extensions
{
	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class SectionDefinitionExtensionTests
	{
		[TestMethod]
		public void SectionDefinition_GetByID()
		{
			var mock = TestData.DomHelper;

			var definition = mock.SectionDefinitions.GetByID(TestData.SectionDefinition1.GetID().Id);

			definition.Should().Be(TestData.SectionDefinition1);
		}

		[TestMethod]
		public void SectionDefinition_GetByName()
		{
			var mock = TestData.DomHelper;

			var definition = mock.SectionDefinitions.GetByName(TestData.SectionDefinition1.GetName());

			definition.Should().Be(TestData.SectionDefinition1);
		}

		[TestMethod]
		public void SectionDefinition_GetFieldDescriptorByName()
		{
			var fieldDescriptor = TestData.SectionDefinition1.GetFieldDescriptorByName("Field 1");

			fieldDescriptor.Should().BeAssignableTo<FieldDescriptor>();
			fieldDescriptor.Name.Should().Be("Field 1");
		}
	}
}
