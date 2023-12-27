namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;

	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Extension methods for <see cref="Section"/>.
	/// </summary>
	public static class SectionExtensions
	{
		/// <summary>
		/// Gets the value of a field within the <see cref="Section"/> by its <see cref="FieldDescriptorID"/>.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="section">The <see cref="Section"/>.</param>
		/// <param name="fieldDescriptorID">The <see cref="FieldDescriptorID"/>.</param>
		/// <returns>The value of the field.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="section"/> or <paramref name="fieldDescriptorID"/> is null.</exception>
		public static T GetFieldValue<T>(this Section section, FieldDescriptorID fieldDescriptorID)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (fieldDescriptorID == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorID));
			}

			var value = section.GetValue<T>(fieldDescriptorID);

			return value.GetValue();
		}

		/// <summary>
		/// Gets the value of a field within the <see cref="Section"/> by its <see cref="FieldDescriptor"/>.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="section">The <see cref="Section"/>.</param>
		/// <param name="fieldDescriptor">The <see cref="FieldDescriptor"/>.</param>
		/// <returns>The value of the field.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="section"/> or <paramref name="fieldDescriptor"/> is null.</exception>
		public static T GetFieldValue<T>(this Section section, FieldDescriptor fieldDescriptor)
		{
			if (section == null)
			{
				throw new ArgumentNullException(nameof(section));
			}

			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			return section.GetFieldValue<T>(fieldDescriptor.ID);
		}

		/// <summary>
		/// Gets the value of a field within the <see cref="Section"/> by field name and <see cref="DomCache"/>.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="section">The <see cref="Section"/>.</param>
		/// <param name="name">The name of the field.</param>
		/// <param name="cache">The <see cref="DomCache"/>.</param>
		/// <returns>The value of the field.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="section"/> or <paramref name="cache"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is null or whitespace.</exception>
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

			return section.GetFieldValue<T>(fieldDescriptor);
		}

		/// <summary>
		/// Sets the value of a field within the <see cref="Section"/> by field name and <see cref="DomCache"/>.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="section">The <see cref="Section"/>.</param>
		/// <param name="name">The name of the field.</param>
		/// <param name="value">The new value for the field.</param>
		/// <param name="cache">The <see cref="DomCache"/>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="section"/>, <paramref name="cache"/>, or <paramref name="name"/> is null.</exception>
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

		/// <summary>
		/// Sets the value of a field within the <see cref="Section"/> by its <see cref="FieldDescriptorID"/>.
		/// </summary>
		/// <typeparam name="T">The type of the field value.</typeparam>
		/// <param name="section">The <see cref="Section"/>.</param>
		/// <param name="fieldDescriptorId">The <see cref="FieldDescriptorID"/>.</param>
		/// <param name="value">The new value for the field.</param>
		/// <exception cref="ArgumentNullException"><paramref name="section"/> or <paramref name="fieldDescriptorId"/> is null.</exception>
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
