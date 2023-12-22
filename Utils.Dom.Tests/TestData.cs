namespace Utils.Dom.Tests
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Builders;
	using Skyline.DataMiner.Utils.DOM.Extensions;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	public static class TestData
	{
		static TestData()
		{
			DomHelper.SetDefinitions(new[] { Definition1 });
			DomHelper.SetSectionDefinitions(new[] { SectionDefinition1, SectionDefinition2 });
			DomHelper.SetInstances(new[] { Instance1, Instance2 });
		}

		public static DomHelperMock DomHelper { get; } = DomHelperMock.Create();

		public static DomDefinition Definition1 { get; } = new DomDefinitionBuilder()
			.WithID(Guid.Parse("fc13f9fb-02d4-4158-9186-d72317be5aa7"))
			.WithName("Definition 1")
			.Build();

		public static SectionDefinition SectionDefinition1 { get; } = new SectionDefinitionBuilder()
				.WithID(Guid.Parse("82d20f2e-dc5f-4115-88f4-28cc2b1747e2"))
				.WithName("Section Definition 1")
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(Guid.Parse("8308cf6e-f797-4998-a413-5c336e005790"))
					.WithName("Field 1")
					.WithType(typeof(string)))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(Guid.Parse("fb36b3ea-bf5c-4f89-b42e-a7e27cf916c1"))
					.WithName("Field 2")
					.WithType(typeof(long)))
				.Build();

		public static SectionDefinition SectionDefinition2 { get; } = new SectionDefinitionBuilder()
				.WithID(Guid.Parse("5d6fec2d-ba88-4c62-82f9-54d895034cfd"))
				.WithName("Section Definition 2")
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(Guid.Parse("a2fe2aa7-abf6-4669-9889-96d232ac16da"))
					.WithName("Field 1")
					.WithType(typeof(string)))
				.AddFieldDescriptor(new FieldDescriptorBuilder()
					.WithID(Guid.Parse("920b3fd3-491d-4f1f-9ed3-421ed73bc903"))
					.WithName("Field 2")
					.WithType(typeof(long)))
				.Build();

		public static DomInstance Instance1 { get; } = new DomInstanceBuilder()
				.WithDefinition(Definition1)
				.WithID(Guid.Parse("33e63ef0-4a92-449b-a4c0-2dce260408f5"))
				.AddSection(new DomSectionBuilder(SectionDefinition1)
					.WithFieldValue(SectionDefinition1.GetFieldDescriptorByName("Field 1"), "Value 1")
					.WithFieldValue(SectionDefinition1.GetFieldDescriptorByName("Field 2"), 123))
				.Build();

		public static DomInstance Instance2 { get; } = new DomInstanceBuilder()
				.WithDefinition(Definition1)
				.WithID(Guid.Parse("c3a576d5-e03c-452f-a731-b7ada4d155b5"))
				.AddSection(new DomSectionBuilder(SectionDefinition1)
					.WithFieldValue(SectionDefinition1.GetFieldDescriptorByName("Field 1"), "Value 2")
					.WithFieldValue(SectionDefinition1.GetFieldDescriptorByName("Field 2"), 456))
				.Build();

	}
}
