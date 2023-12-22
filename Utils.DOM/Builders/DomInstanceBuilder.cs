namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	public class DomInstanceBuilder<T> where T : DomInstanceBuilder<T>
	{
		private readonly DomInstance _instance;

		public DomInstanceBuilder()
		{
			_instance = new DomInstance();
		}

		public DomInstanceBuilder(DomDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			_instance = new DomInstance { DomDefinitionId = definition.ID };
		}

		public DomInstanceBuilder(DomDefinitionId definitionID)
		{
			if (definitionID == null)
			{
				throw new ArgumentNullException(nameof(definitionID));
			}

			_instance = new DomInstance { DomDefinitionId = definitionID };
		}

		public DomInstanceBuilder(DomInstance instance)
		{
			_instance = instance ?? throw new ArgumentNullException(nameof(instance));
		}

		public DomInstance Build()
		{
			return _instance;
		}

		public T WithID(Guid id)
		{
			_instance.ID = new DomInstanceId(id);

			return (T)this;
		}

		public T WithDefinition(DomDefinitionId definitionId)
		{
			if (definitionId == null)
			{
				throw new ArgumentNullException(nameof(definitionId));
			}

			_instance.DomDefinitionId = definitionId;

			return (T)this;
		}

		public T WithDefinition(DomDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			return WithDefinition(definition.ID);
		}

		public T AddSection(Section section)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			_instance.Sections.Add(section);

			return (T)this;
		}

		public T AddSection<TSection>(DomSectionBuilder<TSection> sectionBuilder) where TSection : DomSectionBuilder<TSection>
		{
			if (sectionBuilder == null)
			{
				throw new ArgumentNullException(nameof(sectionBuilder));
			}

			return AddSection(sectionBuilder.Build());
		}

		public T WithFieldValue(SectionDefinitionID sectionDefinitionID, FieldDescriptorID fieldDescriptorID, object value)
		{
			if (sectionDefinitionID == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionID));
			}

			if (fieldDescriptorID == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorID));
			}

			if (!Equals(value, default))
			{
				_instance.AddOrUpdateFieldValue(sectionDefinitionID, fieldDescriptorID, value);
			}
			else
			{
				_instance.RemoveFieldValue(sectionDefinitionID, fieldDescriptorID);
			}

			return (T)this;
		}

		public T WithFieldValue(SectionDefinition sectionDefinition, FieldDescriptor fieldDescriptor, object value)
		{
			if (sectionDefinition == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinition));
			}

			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			if (!Equals(value, default))
			{
				_instance.AddOrUpdateFieldValue(sectionDefinition, fieldDescriptor, value);
			}
			else
			{
				_instance.RemoveFieldValue(sectionDefinition, fieldDescriptor);
			}

			return (T)this;
		}
	}

	public class DomInstanceBuilder : DomInstanceBuilder<DomInstanceBuilder>
	{
		public DomInstanceBuilder()
		{
		}

		public DomInstanceBuilder(DomDefinition definition) : base(definition)
		{
		}

		public DomInstanceBuilder(DomDefinitionId definitionID) : base(definitionID)
		{
		}

		public DomInstanceBuilder(DomInstance instance) : base(instance)
		{
		}
	}
}