namespace Utils.DOM.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Net.Sections;
    using Skyline.DataMiner.Utils.DOM.Builders;

    [TestClass]
    public class SectionDefinitionBuilderTests
    {
        [TestMethod]
        public void SectionDefinitionBuilder_WithID()
        {
            var id = new SectionDefinitionID(Guid.NewGuid());

            var definition = new SectionDefinitionBuilder()
                .WithID(id)
                .Build();

            definition.GetID().Should().Be(id);
        }

        [TestMethod]
        public void SectionDefinitionBuilder_WithName()
        {
            var name = "My name";

            var definition = new SectionDefinitionBuilder()
                .WithName(name)
                .Build();

            definition.GetName().Should().Be(name);
        }

        [TestMethod]
        public void SectionDefinitionBuilder_WithReservationLinkInfo()
        {
            var reservationLinkInfo = new ReservationLinkInfo();

            var definition = new SectionDefinitionBuilder()
                .WithReservationLinkInfo(reservationLinkInfo)
                .Build();

            definition.GetReservationLinkInfo().Should().Be(reservationLinkInfo);
        }

        [TestMethod]
        public void SectionDefinitionBuilder_AddFieldDescriptor()
        {
            var fieldDescriptorId1 = new FieldDescriptorID(Guid.NewGuid());
            var fieldDescriptorId2 = new FieldDescriptorID(Guid.NewGuid());
            var fieldDescriptorId3 = new FieldDescriptorID(Guid.NewGuid());

            var definition = new SectionDefinitionBuilder()
                .AddFieldDescriptor(new FieldDescriptor { ID = fieldDescriptorId1 })
                .AddFieldDescriptor(new FieldDescriptorBuilder()
                    .WithID(fieldDescriptorId2))
                .AddFieldDescriptor(builder =>
                    builder.WithID(fieldDescriptorId3))
                .Build();

            definition.GetAllFieldDescriptors().Should()
                .BeEquivalentTo(new[]
                {
                    new FieldDescriptor{ ID = fieldDescriptorId1 },
                    new FieldDescriptor{ ID = fieldDescriptorId2 },
                    new FieldDescriptor{ ID = fieldDescriptorId3 },
                });
        }
    }
}
