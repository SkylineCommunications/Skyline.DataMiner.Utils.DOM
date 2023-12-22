namespace Utils.Dom.Tests
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class DomInstanceExtensionTests
	{
		[TestMethod]
		public void DomInstance_GetByID()
		{
			var mock = TestData.DomHelper;

			var instance = mock.DomInstances.GetByID(TestData.Instance1.ID.Id);

			instance.Should().Be(TestData.Instance1);
		}

		[TestMethod]
		public void DomInstance_ReadAll()
		{
			var mock = TestData.DomHelper;

			var instances = mock.DomInstances.ReadAll(TestData.Definition1);

			instances.Should().BeEquivalentTo(new[] { TestData.Instance1, TestData.Instance2 });
		}

		[TestMethod]
		public void DomInstance_GetSectionsWithDefinition()
		{
			var sections = TestData.Instance2.GetSectionsWithDefinition(TestData.SectionDefinition2);

			sections.Select(x => x.ID).Should().BeEquivalentTo(new[]
			{
				new SectionID(Guid.Parse("c4b62b86-ce85-46b4-9532-6197af7e6f15")),
				new SectionID(Guid.Parse("6b274338-e23f-47dc-8ca1-61a96c97449f")),
			});
		}

		[TestMethod]
		public void DomInstance_GetSectionsWithDefinitionName()
		{
			var cache = new DomCache(TestData.DomHelper);
			var sections = TestData.Instance2.GetSectionsWithDefinition("Section Definition 2", cache);

			sections.Select(x => x.ID).Should().BeEquivalentTo(new[]
			{
				new SectionID(Guid.Parse("c4b62b86-ce85-46b4-9532-6197af7e6f15")),
				new SectionID(Guid.Parse("6b274338-e23f-47dc-8ca1-61a96c97449f")),
			});
		}

		[TestMethod]
		public void DomInstance_GetFieldValue()
		{
			var cache = new DomCache(TestData.DomHelper);

			var value = TestData.Instance2.GetFieldValue<int>("Section Definition 1", "Field 2", cache);

			value.Should().Be(456);
		}

		[TestMethod]
		public void DomInstance_SetFieldValue()
		{
			var cache = new DomCache(TestData.DomHelper);

			// clone the instance to avoid interference with other tests
			var instance = TestData.Instance2.Clone() as DomInstance;

			instance.SetFieldValue("Section Definition 1", "Field 2", 666, cache);

			var value = instance.GetFieldValue<int>("Section Definition 1", "Field 2", cache);
			value.Should().Be(666);
		}
	}
}
