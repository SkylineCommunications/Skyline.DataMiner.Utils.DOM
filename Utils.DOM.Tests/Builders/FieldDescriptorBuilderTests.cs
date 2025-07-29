namespace Skyline.DataMiner.Utils.DOM.Tests.Builders
{
	using System;
	using System.Collections.Generic;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.GenericEnums;
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

		[TestMethod]
		public void GenericEnumFieldDescriptorBuilder_WithAllowMultiple()
		{
			var fieldDescriptor = new GenericEnumFieldDescriptorBuilder()
				.WithEnumType(GenericEnumFieldDescriptorBuilder.EnumType.Int)
				.WithAllowMultiple(true)
				.Build();

			fieldDescriptor.FieldType.Should().Be(typeof(List<GenericEnum<int>>));
		}

		[TestMethod]
		public void GenericEnumFieldDescriptorBuilder_WithEnumName()
		{
			var enumName = "MyEnum";
			var fieldDescriptor = new GenericEnumFieldDescriptorBuilder()
				.WithEnumName(enumName)
				.Build();

			fieldDescriptor.GenericEnumInstance.EnumName.Should().Be(enumName);
		}

		[TestMethod]
		public void GenericEnumFieldDescriptorBuilder_WithEnumType()
		{
			var fieldDescriptor = new GenericEnumFieldDescriptorBuilder()
				.WithEnumType(GenericEnumFieldDescriptorBuilder.EnumType.String)
				.Build();

			fieldDescriptor.FieldType.Should().Be(typeof(GenericEnum<string>));
		}

		[TestMethod]
		public void GenericEnumFieldDescriptorBuilder_WithEnumValue()
		{
			var enumValue1 = new GenericEnumEntry<string>("Display1", "Value1");
			var enumValue2 = new GenericEnumEntry<string>("Display2", "Value2");
			var enumValue3 = new GenericEnumEntry<string>("Display3", "Value3");

			var fieldDescriptor = new GenericEnumFieldDescriptorBuilder()
				.WithEnumType(GenericEnumFieldDescriptorBuilder.EnumType.String)
				.AddEnumValue(enumValue1)
				.AddEnumValue(enumValue2)
				.AddEnumValue(enumValue3)
				.Build();

			fieldDescriptor.FieldType.Should().Be(typeof(GenericEnum<string>));
			fieldDescriptor.GenericEnumInstance.Should().NotBeNull();
			fieldDescriptor.GenericEnumInstance.Entries.Should().HaveCount(3);
			fieldDescriptor.GenericEnumInstance.Entries.Should().Contain(new[] { enumValue1, enumValue2, enumValue3 });
		}

		[TestMethod]
		public void DomInstanceFieldDescriptor_WithAllowMultiple()
		{
			var fieldDescriptor = new DomInstanceFieldDescriptorBuilder()
				.WithAllowMultiple(true)
				.Build();

			fieldDescriptor.FieldType.Should().Be(typeof(List<Guid>));
		}

		[TestMethod]
		public void DomInstanceFieldDescriptor_WithModule()
		{
			var moduleName = "my_module";
			var fieldDescriptor = new DomInstanceFieldDescriptorBuilder()
				.WithModule(moduleName)
				.Build();

			fieldDescriptor.ModuleId.Should().Be(moduleName);
		}

		[TestMethod]
		public void DomInstanceFieldDescriptor_WithDefinition()
		{
			var moduleName = "my_module";
			var definitionId1 = new Skyline.DataMiner.Net.Apps.DataMinerObjectModel.DomDefinitionId(Guid.NewGuid())
			{
				ModuleId = moduleName,
			};
			var definitionId2 = new Skyline.DataMiner.Net.Apps.DataMinerObjectModel.DomDefinitionId(Guid.NewGuid())
			{
				ModuleId = "my_other_module",
			};

			var fieldDescriptor = new DomInstanceFieldDescriptorBuilder()
				.WithModule(moduleName)
				.AddDomDefinition(definitionId1)
				.AddDomDefinition(definitionId2)
				.AddDomDefinition(new Skyline.DataMiner.Net.Apps.DataMinerObjectModel.DomDefinitionId(Guid.NewGuid()))
				.Build();

			fieldDescriptor.ModuleId.Should().Be(moduleName);
			fieldDescriptor.DomDefinitionIds.Should().NotBeNull();
			fieldDescriptor.DomDefinitionIds.Should().HaveCount(2);
			fieldDescriptor.DomDefinitionIds.Should().Contain(definitionId1);
		}
	}
}
