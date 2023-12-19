namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.Sections.Fields;
	using Skyline.DataMiner.Net.Apps.Sections.Sections;
	using Skyline.DataMiner.Net.Sections;

	public static class SectionContainerExtensions
	{
		public static T GetFieldValue<T>(this ISectionContainer container, string sectionName, string fieldName, DomCache cache)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (String.IsNullOrWhiteSpace(sectionName))
			{
				throw new ArgumentException($"'{nameof(sectionName)}' cannot be null or whitespace.", nameof(sectionName));
			}

			if (String.IsNullOrWhiteSpace(fieldName))
			{
				throw new ArgumentException($"'{nameof(fieldName)}' cannot be null or whitespace.", nameof(fieldName));
			}

			if (cache == null)
			{
				throw new ArgumentNullException(nameof(cache));
			}

			var section = cache.GetSectionDefinitionByName(sectionName);
			var field = section.GetFieldDescriptorByName(fieldName);

			var valueWrapper = container.GetFieldValue<T>(section, field);

			return valueWrapper != null ? valueWrapper.Value : default;
		}

		public static void SetFieldValue<T>(this ISectionContainer container, string sectionName, string fieldName, T value, DomCache cache)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (String.IsNullOrWhiteSpace(sectionName))
			{
				throw new ArgumentException($"'{nameof(sectionName)}' cannot be null or whitespace.", nameof(sectionName));
			}

			if (String.IsNullOrWhiteSpace(fieldName))
			{
				throw new ArgumentException($"'{nameof(fieldName)}' cannot be null or whitespace.", nameof(fieldName));
			}

			if (cache == null)
			{
				throw new ArgumentNullException(nameof(cache));
			}

			var section = cache.GetSectionDefinitionByName(sectionName);
			var field = section.GetFieldDescriptorByName(fieldName);

			if (!Equals(value, default))
			{
				container.AddOrUpdateFieldValue(section, field, value);
			}
			else
			{
				container.RemoveFieldValue(section, field);
			}
		}

		public static void RemoveFieldValue(this ISectionContainer container, SectionDefinition sectionDefinition, FieldDescriptor fieldDescriptor)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (sectionDefinition == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinition));
			}

			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			container.RemoveFieldValue(sectionDefinition.GetID(), fieldDescriptor.ID);
		}

		public static void RemoveFieldValue(this ISectionContainer container, SectionDefinitionID sectionDefinitionID, FieldDescriptorID fieldDescriptorID)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (sectionDefinitionID == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionID));
			}

			if (fieldDescriptorID == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorID));
			}

			container.GetSections()
				.FirstOrDefault(s => s.SectionDefinitionID.Equals(sectionDefinitionID))
				?.RemoveFieldValueById(fieldDescriptorID);
		}

		public static ListValueWrapper<T> GetOrInitializeListFieldValue<T>(this ISectionContainer container, SectionDefinition sectionDefinition, FieldDescriptor fieldDescriptor)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (sectionDefinition == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinition));
			}

			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			return container.GetOrInitializeListFieldValue<T>(sectionDefinition.GetID(), fieldDescriptor.ID);
		}

		public static ListValueWrapper<T> GetOrInitializeListFieldValue<T>(this ISectionContainer container, SectionDefinitionID sectionDefinitionID, FieldDescriptorID fieldDescriptorID)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			if (sectionDefinitionID == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionID));
			}

			if (fieldDescriptorID == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorID));
			}

			var listValueWrapper = container.GetListFieldValue<T>(sectionDefinitionID, fieldDescriptorID);

			if (listValueWrapper == null)
			{
				container.AddOrUpdateListFieldValue(sectionDefinitionID, fieldDescriptorID, new List<T>());
				listValueWrapper = container.GetListFieldValue<T>(sectionDefinitionID, fieldDescriptorID);
			}

			return listValueWrapper;
		}
	}
}