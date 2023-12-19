namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;

	using Skyline.DataMiner.Net.Sections;

	public static class SectionExtensions
	{
		public static T GetFieldValue<T>(this Section section, string name, DomCache cache)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (cache == null)
			{
				throw new ArgumentNullException(nameof(cache));
			}

			var definition = cache.GetSectionDefinitionById(section.SectionDefinitionID.Id);
			var fieldDescriptor = definition.GetFieldDescriptorByName(name);

			var value = section.GetValue<T>(fieldDescriptor.ID);

			return value.GetValue();
		}

		public static void SetFieldValue<T>(this Section section, string name, T value, DomCache cache)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (cache == null)
			{
				throw new ArgumentNullException(nameof(cache));
			}

			var definition = cache.GetSectionDefinitionById(section.SectionDefinitionID.Id);
			var fieldDescriptor = definition.GetFieldDescriptorByName(name);

			section.SetFieldValue(fieldDescriptor.ID, value);
		}

		public static void SetFieldValue<T>(this Section section, FieldDescriptorID fieldDescriptorId, T value)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (fieldDescriptorId == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorId));
			}

			if (!Equals(value, default))
			{
				var fieldValue = new FieldValue(fieldDescriptorId, ValueWrapperFactory.Create(value));
				section.AddOrReplaceFieldValue(fieldValue);
			}
			else
			{
				section.RemoveFieldValueById(fieldDescriptorId);
			}
		}
	}
}
