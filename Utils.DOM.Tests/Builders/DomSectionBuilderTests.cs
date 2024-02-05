namespace Utils.DOM.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM;
	using Skyline.DataMiner.Utils.DOM.Builders;
    using Skyline.DataMiner.Utils.DOM.Extensions;

    [TestClass]
    public class DomSectionBuilderTests
    {
        [TestMethod]
        public void DomSectionBuilder_WithID()
        {
            var id = new SectionID(Guid.NewGuid());

            var section = new DomSectionBuilder()
                .WithID(id)
                .Build();

            section.ID.Should().Be(id);
        }

        [TestMethod]
        public void DomSectionBuilder_WithSectionDefinitionID()
        {
            var id = new SectionDefinitionID(Guid.NewGuid());

            var section = new DomSectionBuilder()
                .WithSectionDefinitionID(id)
                .Build();

            section.SectionDefinitionID.Should().Be(id);
        }

        [TestMethod]
        public void DomSectionBuilder_WithFieldValue1()
        {
            var fieldDescId = new FieldDescriptorID(Guid.NewGuid());

            var section = new DomSectionBuilder()
                .WithFieldValue(fieldDescId, 123)
                .Build();

            section.GetFieldValue<int>(fieldDescId).Should().Be(123);
        }

        [TestMethod]
        public void DomSectionBuilder_WithFieldValue2()
        {
            var fieldDescId = new FieldDescriptorID(Guid.NewGuid());
            var fieldDesc = new FieldDescriptor { ID = fieldDescId };

            var section = new DomSectionBuilder()
                .WithFieldValue(fieldDesc, 123)
                .Build();

            section.GetFieldValue<int>(fieldDesc).Should().Be(123);
        }

		[TestMethod]
		public void DomSectionBuilder_WithFieldValue3()
		{
			var cache = new DomCache(TestData.DomHelper);

			var section = new DomSectionBuilder(TestData.SectionDefinition1)
				.WithFieldValue("Field 1", "my value", cache)
				.Build();

			section.GetFieldValue<string>("Field 1", cache).Should().Be("my value");
		}
	}
}
