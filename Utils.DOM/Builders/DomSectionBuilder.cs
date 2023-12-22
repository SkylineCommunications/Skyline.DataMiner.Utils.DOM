namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;

	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	public class DomSectionBuilder<T> where T : DomSectionBuilder<T>
	{
		private readonly Section _section;

		public DomSectionBuilder()
		{
			_section = new Section();
		}

		public DomSectionBuilder(SectionDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			_section = new Section(definition);
		}

		public DomSectionBuilder(SectionDefinitionID definitionID)
		{
			if (definitionID == null)
			{
				throw new ArgumentNullException(nameof(definitionID));
			}

			_section = new Section(definitionID);
		}

		public DomSectionBuilder(Section section)
		{
			_section = section ?? throw new ArgumentNullException(nameof(section));
		}

		public Section Build()
		{
			return _section;
		}

		public T WithID(Guid id)
		{
			_section.ID = new SectionID(id);

			return (T)this;
		}

		public T WithSectionDefinitionID(SectionDefinitionID sectionDefinitionID)
		{
			if (sectionDefinitionID == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionID));
			}

			_section.SectionDefinitionID = sectionDefinitionID;

			return (T)this;
		}

		public T WithFieldValue(FieldDescriptorID fieldDescriptorID, object value)
		{
			if (fieldDescriptorID == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorID));
			}

			_section.SetFieldValue(fieldDescriptorID, value);

			return (T)this;
		}

		public T WithFieldValue(FieldDescriptor fieldDescriptor, object value)
		{
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			_section.SetFieldValue(fieldDescriptor.ID, value);

			return (T)this;
		}
	}

	public class DomSectionBuilder : DomSectionBuilder<DomSectionBuilder>
	{
		public DomSectionBuilder()
		{
		}

		public DomSectionBuilder(SectionDefinition definition) : base(definition)
		{
		}

		public DomSectionBuilder(SectionDefinitionID definitionID) : base(definitionID)
		{
		}

		public DomSectionBuilder(Section section) : base(section)
		{
		}
	}
}