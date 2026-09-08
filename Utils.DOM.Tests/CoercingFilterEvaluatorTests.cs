namespace Skyline.DataMiner.Utils.DOM.Tests
{
	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Utils.DOM.Extensions;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	[TestClass]
	public class CoercingFilterEvaluatorTests
	{
		[TestMethod]
		public void Read_FieldValueFilter_MatchesWhenLiteralTypeDiffersFromStoredValue()
		{
			// arrange - Instance1 stores "Field 2" as an int (123); the field descriptor is a long.
			var helper = DomHelperMock.Create();
			helper.SetDefinitions(new[] { TestData.Definition1 });
			helper.SetSectionDefinitions(new[] { TestData.SectionDefinition1, TestData.SectionDefinition2 });
			helper.SetInstances(new[] { TestData.Instance1, TestData.Instance2 });

			var field2Id = TestData.SectionDefinition1.GetFieldDescriptorByName("Field 2").ID;

			// filter with a long literal while the stored value is an int
			var filter = DomInstanceExposers.FieldValues.DomInstanceField(field2Id).Equal(123L);

			// act
			var result = helper.DomInstances.Read(filter);

			// assert - a real DataMiner Agent coerces the numeric types, so the instance matches
			result.Should().ContainSingle().Which.ID.Should().Be(TestData.Instance1.ID);
		}

		[TestMethod]
		public void Read_FieldValueFilter_DoesNotMatchWhenNumericValuesDiffer()
		{
			// arrange
			var helper = DomHelperMock.Create();
			helper.SetDefinitions(new[] { TestData.Definition1 });
			helper.SetSectionDefinitions(new[] { TestData.SectionDefinition1, TestData.SectionDefinition2 });
			helper.SetInstances(new[] { TestData.Instance1, TestData.Instance2 });

			var field2Id = TestData.SectionDefinition1.GetFieldDescriptorByName("Field 2").ID;

			// no instance stores 999
			var filter = DomInstanceExposers.FieldValues.DomInstanceField(field2Id).Equal(999L);

			// act
			var result = helper.DomInstances.Read(filter);

			// assert
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void Count_FieldValueFilter_CountsMatchesThroughCoercion()
		{
			// arrange
			var helper = DomHelperMock.Create();
			helper.SetDefinitions(new[] { TestData.Definition1 });
			helper.SetSectionDefinitions(new[] { TestData.SectionDefinition1, TestData.SectionDefinition2 });
			helper.SetInstances(new[] { TestData.Instance1, TestData.Instance2 });

			var field2Id = TestData.SectionDefinition1.GetFieldDescriptorByName("Field 2").ID;
			var filter = DomInstanceExposers.FieldValues.DomInstanceField(field2Id).Equal(123L);

			// act
			var count = helper.DomInstances.Count(filter);

			// assert
			count.Should().Be(1);
		}
	}
}
