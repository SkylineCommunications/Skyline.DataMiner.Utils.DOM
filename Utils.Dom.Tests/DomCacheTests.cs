namespace Utils.Dom.Tests
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class DomCacheTests
	{
		[TestMethod]
		public void DomCache_GetInstanceById()
		{
			var cache = new DomCache(TestData.DomHelper);

			var instance = cache.GetInstanceById(TestData.Instance1.ID.Id);

			instance.Should().Be(TestData.Instance1);
		}

		[TestMethod]
		public void DomCache_GetInstancesById()
		{
			var cache = new DomCache(TestData.DomHelper);

			var instances = cache.GetInstancesById(new[] { TestData.Instance1.ID.Id, TestData.Instance2.ID.Id });

			var expectedResult = new Dictionary<Guid, DomInstance>
			{
				{ TestData.Instance1.ID.Id, TestData.Instance1 },
				{ TestData.Instance2.ID.Id, TestData.Instance2 }
			};

			instances.Should().Equal(expectedResult);
		}

		[TestMethod]
		public void DomCache_GetInstancesByFilter()
		{
			var cache = new DomCache(TestData.DomHelper);

			var filter = DomInstanceExposers.FieldValues
				.DomInstanceField(TestData.SectionDefinition1.GetFieldDescriptorByName("Field 1"))
				.Equal("Value 1");

			var instances = cache.GetInstances(filter);

			instances.Should().Equal(new[] { TestData.Instance1 });
		}

		[TestMethod]
		public void DomCache_GetInstancesByDefinition()
		{
			var cache = new DomCache(TestData.DomHelper);

			var instances = cache.GetInstancesByDefinition("Definition 1");

			instances.Should().BeEquivalentTo(new[] { TestData.Instance1, TestData.Instance2 });
		}

		[TestMethod]
		public void DomCache_GetDefinitionById()
		{
			var cache = new DomCache(TestData.DomHelper);

			var definition = cache.GetDefinitionById(TestData.Definition1.ID.Id);

			definition.Should().Be(TestData.Definition1);
		}

		[TestMethod]
		public void DomCache_GetDefinitionByName()
		{
			var cache = new DomCache(TestData.DomHelper);

			var definition = cache.GetDefinitionByName(TestData.Definition1.Name);

			definition.Should().Be(TestData.Definition1);
		}

		[TestMethod]
		public void DomCache_GetSectionDefinitionById()
		{
			var cache = new DomCache(TestData.DomHelper);

			var sectionDefinition = cache.GetSectionDefinitionById(TestData.SectionDefinition1.GetID().Id);

			sectionDefinition.Should().Be(TestData.SectionDefinition1);
		}

		[TestMethod]
		public void DomCache_GetSectionDefinitionByName()
		{
			var cache = new DomCache(TestData.DomHelper);

			var sectionDefinition = cache.GetSectionDefinitionByName(TestData.SectionDefinition1.GetName());

			sectionDefinition.Should().Be(TestData.SectionDefinition1);
		}

		[TestMethod]
		public void DomCache_GetFieldDescriptor()
		{
			var cache = new DomCache(TestData.DomHelper);

			var fieldDescriptor = cache.GetFieldDescriptor(TestData.SectionDefinition1.GetID(), TestData.SectionDefinition1.GetFieldDescriptorByName("Field 1").ID);

			fieldDescriptor.Should().BeAssignableTo<FieldDescriptor>();
			fieldDescriptor.Name.Should().Be("Field 1");
		}

		[TestMethod]
		public void DomCache_GetFieldDescriptorByName()
		{
			var cache = new DomCache(TestData.DomHelper);

			var fieldDescriptor = cache.GetFieldDescriptorByName("Section Definition 1", "Field 1");

			fieldDescriptor.Should().BeAssignableTo<FieldDescriptor>();
			fieldDescriptor.Name.Should().Be("Field 1");
		}
	}
}
