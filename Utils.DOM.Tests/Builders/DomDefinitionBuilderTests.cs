namespace Utils.DOM.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions;
    using Skyline.DataMiner.Net.Sections;
    using Skyline.DataMiner.Utils.DOM.Builders;

    [TestClass]
    public class DomDefinitionBuilderTests
    {
        [TestMethod]
        public void DomDefinitionBuilder_WithID()
        {
            var id = new DomDefinitionId(Guid.NewGuid());

            var definition = new DomDefinitionBuilder()
                .WithID(id)
                .Build();

            definition.ID.Should().Be(id);
        }

        [TestMethod]
        public void DomDefinitionBuilder_WithName()
        {
            var name = "My name";

            var definition = new DomDefinitionBuilder()
                .WithName(name)
                .Build();

            definition.Name.Should().Be(name);
        }

        [TestMethod]
        public void DomDefinitionBuilder_WithDomBehaviorDefinition()
        {
            var behaviorDefinitionId = new DomBehaviorDefinitionId(Guid.NewGuid());

            var definition = new DomDefinitionBuilder()
                .WithDomBehaviorDefinition(behaviorDefinitionId)
                .Build();

            definition.DomBehaviorDefinitionId.Should().Be(behaviorDefinitionId);
        }

        [TestMethod]
        public void DomDefinitionBuilder_AddSectionDefinitionLink()
        {
            var sectionDefinitionID1 = new SectionDefinitionID(Guid.NewGuid());
            var sectionDefinitionID2 = new SectionDefinitionID(Guid.NewGuid());

            var definition = new DomDefinitionBuilder()
                .AddSectionDefinitionLink(sectionDefinitionID1)
                .AddSectionDefinitionLink(new SectionDefinitionLink(sectionDefinitionID2) { AllowMultipleSections = true })
                .Build();

            definition.SectionDefinitionLinks
                .Should()
                .BeEquivalentTo(
                    new[]
                    {
                        new SectionDefinitionLink(sectionDefinitionID1),
                        new SectionDefinitionLink(sectionDefinitionID2) {AllowMultipleSections = true},
                    });
        }
    }
}
