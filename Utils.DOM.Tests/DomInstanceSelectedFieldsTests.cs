namespace Skyline.DataMiner.Utils.DOM.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.ManagerStore.Select;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class DomInstanceSelectedFieldsTests
	{
		private static readonly FieldDescriptorID Field1Id = TestData.SectionDefinition1.GetFieldDescriptorByName("Field 1").ID;
		private static readonly FieldDescriptorID Field2Id = TestData.SectionDefinition1.GetFieldDescriptorByName("Field 2").ID;
		private static readonly FieldDescriptorID Section2Field1Id = TestData.SectionDefinition2.GetFieldDescriptorByName("Field 1").ID;

		[TestMethod]
		public void DomInstances_Read_SelectedFields_BaseProperties()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>()
				.Add(DomInstanceExposers.DomDefinitionId)
				.Add(DomInstanceExposers.StatusId);

			var filter = DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id);

			// act
			var results = testData.DomHelper.DomInstances.Read(filter, selectedFields);

			// assert
			results.Should().ContainSingle();

			var result = results.Single();
			result.Id.Should().Be(TestData.Instance1.ID);
			result.GetValue(DomInstanceExposers.DomDefinitionId).Should().Be(TestData.Definition1.ID.Id);
			result.GetValue(DomInstanceExposers.StatusId).Should().Be(String.Empty);
		}

		[TestMethod]
		public void DomInstances_Read_SelectedFields_FieldDescriptorValues()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>()
				.Add(Field1Id)
				.Add(Field2Id);

			var filter = DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id);

			// act
			var results = testData.DomHelper.DomInstances.Read(filter, selectedFields);

			// assert
			var result = results.Should().ContainSingle().Subject;
			result.Id.Should().Be(TestData.Instance1.ID);
			result.GetValue<string>(Field1Id).Should().Be("Value 1");

			// the value is returned with the type it was stored with, just like when reading the full instance
			result.GetValue<int>(Field2Id).Should().Be(123);
			result.GetValues<string>(Field1Id).Should().BeEquivalentTo(new[] { "Value 1" });
		}

		[TestMethod]
		public void DomInstances_Read_SelectedFields_MultipleSections()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>().Add(Field1Id);

			// Instance2 has 3 sections with a value for this field descriptor
			var filter = DomInstanceExposers.Id.Equal(TestData.Instance2.ID.Id);

			// act
			var result = testData.DomHelper.DomInstances.Read(filter, selectedFields).Single();

			// assert
			result.GetValues<string>(Field1Id).Should().BeEquivalentTo(new[] { "Value 2", "Value 2", "Value 2" });
			result.Invoking(x => x.GetValue<string>(Field1Id)).Should().Throw<InvalidOperationException>();
			result.TryGetValue<string>(Field1Id, out _).Should().BeFalse();
		}

		[TestMethod]
		public void DomInstances_Read_SelectedFields_WithoutValue()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>().Add(Section2Field1Id);

			// Instance1 doesn't have a section for SectionDefinition2
			var filter = DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id);

			// act
			var result = testData.DomHelper.DomInstances.Read(filter, selectedFields).Single();

			// assert
			result.GetValue<string>(Section2Field1Id).Should().BeNull();
			result.GetValues<string>(Section2Field1Id).Should().BeNull();
			result.TryGetValue<string>(Section2Field1Id, out _).Should().BeFalse();
			result.TryGetValues<string>(Section2Field1Id, out _).Should().BeFalse();
		}

		[TestMethod]
		public void DomInstances_Read_SelectedFields_AllInstances()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>().Add(Field1Id);

			// act
			var results = testData.DomHelper.DomInstances.Read(new TRUEFilterElement<DomInstance>(), selectedFields);

			// assert
			results.Should().HaveCount(2);
			results.Select(x => x.Id).Should().BeEquivalentTo(new[] { TestData.Instance1.ID, TestData.Instance2.ID });
			results.Single(x => x.Id.Equals(TestData.Instance1.ID)).GetValues<string>(Field1Id).Should().BeEquivalentTo(new[] { "Value 1" });
			results.Single(x => x.Id.Equals(TestData.Instance2.ID)).GetValues<string>(Field1Id).Should().HaveCount(3);
		}

		[TestMethod]
		public void DomInstances_Read_SelectedFields_UnsupportedExposer()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>().Add(DomInstanceExposers.FullObject);

			// act & assert
			testData.DomHelper.DomInstances
				.Invoking(x => x.Read(new TRUEFilterElement<DomInstance>(), selectedFields))
				.Should().Throw<InvalidOperationException>();
		}

		[TestMethod]
		public void DomInstances_PreparePaging_SelectedFields()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>().Add(Field1Id);

			// act
			var pagingHelper = testData.DomHelper.DomInstances.PreparePaging(new TRUEFilterElement<DomInstance>(), selectedFields, 1);

			var pages = new List<List<PartialObject<DomInstance, DomInstanceId>>>();
			while (pagingHelper.MoveToNextPage())
			{
				pages.Add(pagingHelper.GetCurrentPage());
			}

			// assert
			pages.Should().HaveCount(2);
			pages.SelectMany(x => x).Select(x => x.Id)
				.Should().BeEquivalentTo(new[] { TestData.Instance1.ID, TestData.Instance2.ID });
			pages.SelectMany(x => x).Should().OnlyContain(x => x.GetValues<string>(Field1Id) != null);
		}

		[TestMethod]
		public void DomInstances_ReadPaged_SelectedFields()
		{
			// arrange
			var testData = new TestData();
			var selectedFields = new SelectedFields<DomInstance>().Add(Field1Id);

			// act
			var pages = testData.DomHelper.DomInstances
				.ReadPaged(new TRUEFilterElement<DomInstance>(), selectedFields, 1)
				.ToList();

			// assert
			pages.Should().HaveCount(2);
			pages.SelectMany(x => x).Select(x => x.Id)
				.Should().BeEquivalentTo(new[] { TestData.Instance1.ID, TestData.Instance2.ID });
		}
	}
}
