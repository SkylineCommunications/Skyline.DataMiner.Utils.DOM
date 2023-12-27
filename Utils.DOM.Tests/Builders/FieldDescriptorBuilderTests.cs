namespace Utils.DOM.Tests
{
    using System;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Skyline.DataMiner.Net.Sections;
    using Skyline.DataMiner.Utils.DOM.Builders;

    [TestClass]
    public class FieldDescriptorBuilderTests
    {
        [TestMethod]
        public void FieldDescriptorBuilder_WithID()
        {
            var id = new FieldDescriptorID(Guid.NewGuid());

            var fieldDescriptor = new FieldDescriptorBuilder()
                .WithID(id)
                .Build();

            fieldDescriptor.ID.Should().Be(id);
        }

        [TestMethod]
        public void FieldDescriptorBuilder_WithName()
        {
            var name = "My name";

            var fieldDescriptor = new FieldDescriptorBuilder()
                .WithName(name)
                .Build();

            fieldDescriptor.Name.Should().Be(name);
        }

        [TestMethod]
        public void FieldDescriptorBuilder_WithType()
        {
            var fieldDescriptor = new FieldDescriptorBuilder()
                .WithType(typeof(long))
                .Build();

            fieldDescriptor.FieldType.Should().Be(typeof(long));
        }

        [TestMethod]
        public void FieldDescriptorBuilder_WithDefaultValue()
        {
            var defaultValue = new ValueWrapper<string>("My value");

            var fieldDescriptor = new FieldDescriptorBuilder()
                .WithDefaultValue(defaultValue)
                .Build();

            fieldDescriptor.DefaultValue.Should().Be(defaultValue);
        }

        [TestMethod]
        public void FieldDescriptorBuilder_WithIsOptional()
        {
            var fieldDescriptor = new FieldDescriptorBuilder()
                .WithIsOptional(true)
                .Build();

            fieldDescriptor.IsOptional.Should().Be(true);
        }
    }
}
