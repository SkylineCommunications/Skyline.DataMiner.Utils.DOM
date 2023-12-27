namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.Sections.Fields;
	using Skyline.DataMiner.Net.Apps.Sections.Sections;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Extension methods for <see cref="ISectionContainer"/>.
	/// </summary>
	public static class SectionContainerExtensions
	{
		/// <summary>
		/// Gets the value of a field within a section by name.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="container">The <see cref="ISectionContainer"/> instance.</param>
		/// <param name="sectionName">The name of the section.</param>
		/// <param name="fieldName">The name of the field.</param>
		/// <param name="cache">The <see cref="DomCache"/> instance.</param>
		/// <returns>The value of the specified field in the specified section, or the default value for the type if not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="container"/> or <paramref name="cache"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="sectionName"/> or <paramref name="fieldName"/> is null or whitespace.</exception>
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

		/// <summary>
		/// Sets the value of a field within a section by name.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="container">The <see cref="ISectionContainer"/> instance.</param>
		/// <param name="sectionName">The name of the section.</param>
		/// <param name="fieldName">The name of the field.</param>
		/// <param name="value">The value to set for the field.</param>
		/// <param name="cache">The <see cref="DomCache"/> instance.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="container"/>, <paramref name="cache"/>, or <paramref name="value"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="sectionName"/> or <paramref name="fieldName"/> is null or whitespace.</exception>
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

		/// <summary>
		/// Removes the value of a field within a section by definition.
		/// </summary>
		/// <param name="container">The <see cref="ISectionContainer"/> instance.</param>
		/// <param name="sectionDefinition">The <see cref="SectionDefinition"/> instance.</param>
		/// <param name="fieldDescriptor">The <see cref="FieldDescriptor"/> instance.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="container"/>, <paramref name="sectionDefinition"/>, or <paramref name="fieldDescriptor"/> is null.</exception>
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

		/// <summary>
		/// Removes the value of a field within a section by definition ID.
		/// </summary>
		/// <param name="container">The <see cref="ISectionContainer"/> instance.</param>
		/// <param name="sectionDefinitionID">The ID of the section definition.</param>
		/// <param name="fieldDescriptorID">The ID of the field descriptor.</param>
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

		/// <summary>
		/// Gets or initializes a list field value within a section by definition.
		/// </summary>
		/// <typeparam name="T">The type of the list elements.</typeparam>
		/// <param name="container">The <see cref="ISectionContainer"/> instance.</param>
		/// <param name="sectionDefinition">The definition of the section.</param>
		/// <param name="fieldDescriptor">The descriptor of the field.</param>
		/// <returns>The list field value.</returns>
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

		/// <summary>
		/// Gets or initializes a list field value within a section by definition ID.
		/// </summary>
		/// <typeparam name="T">The type of the list elements.</typeparam>
		/// <param name="container">The <see cref="ISectionContainer"/> instance.</param>
		/// <param name="sectionDefinitionID">The ID of the section definition.</param>
		/// <param name="fieldDescriptorID">The ID of the field descriptor.</param>
		/// <returns>The list field value.</returns>
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