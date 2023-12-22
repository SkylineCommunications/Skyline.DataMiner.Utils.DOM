namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;

	using Skyline.DataMiner.Net.Sections;

	public class SectionDefinitionBuilder<T> where T : SectionDefinitionBuilder<T>
	{
		private readonly CustomSectionDefinition _definition;

		public SectionDefinitionBuilder()
		{
			_definition = new CustomSectionDefinition();
		}

		public SectionDefinitionBuilder(SectionDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			if (!(definition is CustomSectionDefinition custom))
			{
				throw new NotSupportedException();
			}

			_definition = custom;
		}

		public SectionDefinition Build()
		{
			return _definition;
		}

		public T WithID(Guid id)
		{
			_definition.ID = new SectionDefinitionID(id);

			return (T)this;
		}

		public T WithName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			_definition.Name = name;

			return (T)this;
		}

		public T AddFieldDescriptor(FieldDescriptor fieldDescriptor)
		{
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			_definition.AddOrReplaceFieldDescriptor(fieldDescriptor);

			return (T)this;
		}

		public T AddFieldDescriptor<TFieldDescriptor>(FieldDescriptorBuilder<TFieldDescriptor> fieldDescriptorBuilder) where TFieldDescriptor : FieldDescriptorBuilder<TFieldDescriptor>
		{
			if (fieldDescriptorBuilder == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorBuilder));
			}

			return AddFieldDescriptor(fieldDescriptorBuilder.Build());
		}
	}

	public class SectionDefinitionBuilder : SectionDefinitionBuilder<SectionDefinitionBuilder>
	{
		public SectionDefinitionBuilder()
		{
		}

		public SectionDefinitionBuilder(SectionDefinition definition) : base(definition)
		{
		}
	}
}