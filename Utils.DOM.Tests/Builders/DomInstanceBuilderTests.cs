namespace Utils.DOM.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM;
	using Skyline.DataMiner.Utils.DOM.Builders;
    using Skyline.DataMiner.Utils.DOM.Extensions;

    [TestClass]
    public class DomInstanceBuilderTests
    {
        [TestMethod]
        public void DomInstanceBuilder_WithID()
        {
            var id = new DomInstanceId(Guid.NewGuid());

            var instance = new DomInstanceBuilder()
                .WithID(id)
                .Build();

            instance.ID.Should().Be(id);
        }

        [TestMethod]
        public void DomInstanceBuilder_WithDefinition()
        {
            var definitionId = new DomDefinitionId(Guid.NewGuid());

            var instance = new DomInstanceBuilder()
                .WithDefinition(definitionId)
                .Build();

            instance.DomDefinitionId.Should().Be(definitionId);
        }

        [TestMethod]
        public void DomInstanceBuilder_WithStatusId()
        {
            var instance = new DomInstanceBuilder()
                .WithStatusID("My status")
                .Build();

            instance.StatusId.Should().Be("My status");
        }

        [TestMethod]
        public void DomInstanceBuilder_WithStatusId_Null()
        {
            var instance = new DomInstanceBuilder()
                .WithStatusID(null)
                .Build();

            instance.StatusId.Should().BeNull();
        }

        [TestMethod]
        public void DomInstanceBuilder_AddSection()
        {
            var sectionId = Guid.NewGuid();
            var sectionDefId = new SectionDefinitionID(Guid.NewGuid());

            var instance = new DomInstanceBuilder()
                .AddSection(sectionDefId, builder =>
                    builder.WithID(sectionId))
                .Build();

            instance.Sections.Should().BeEquivalentTo(
                new[] { new Section(sectionDefId) { ID = new SectionID(sectionId) } });
        }

        [TestMethod]
        public void DomInstanceBuilder_WithFieldValue1()
        {
            var sectionDefId = new SectionDefinitionID(Guid.NewGuid());
            var fieldDescId = new FieldDescriptorID(Guid.NewGuid());

            var instance = new DomInstanceBuilder()
                .WithFieldValue(sectionDefId, fieldDescId, 123)
                .Build();

            instance.GetFieldValue<int>(sectionDefId, fieldDescId).GetValue().Should().Be(123);
        }

        [TestMethod]
        public void DomInstanceBuilder_WithFieldValue2()
        {
            var sectionDefId = new SectionDefinitionID(Guid.NewGuid());
            var fieldDescId = new FieldDescriptorID(Guid.NewGuid());

            var sectionDef = new CustomSectionDefinition { ID = sectionDefId };
            var fieldDesc = new FieldDescriptor { ID = fieldDescId };

            var instance = new DomInstanceBuilder()
                .WithFieldValue(sectionDef, fieldDesc, 123)
                .Build();

            instance.GetFieldValue<int>(sectionDef, fieldDesc).GetValue().Should().Be(123);
        }

		[TestMethod]
		public void DomInstanceBuilder_WithFieldValue3()
		{
			var cache = new DomCache(TestData.DomHelper);

			var instance = new DomInstanceBuilder()
				.WithFieldValue("Section Definition 1", "Field 1", "my value", cache)
				.Build();

			instance.GetFieldValue<string>("Section Definition 1", "Field 1", cache).Should().Be("my value");
		}
	}
}
