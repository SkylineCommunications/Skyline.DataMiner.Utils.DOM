namespace Skyline.DataMiner.Utils.DOM.Builders
{
    using System;

    using Skyline.DataMiner.Net.Sections;
    using Skyline.DataMiner.Utils.DOM.Extensions;

	/// <summary>
	/// Generic builder class for creating instances of the <see cref="Section"/> class.
	/// </summary>
	/// <typeparam name="T">The type of the derived builder class.</typeparam>
	public class DomSectionBuilder<T> where T : DomSectionBuilder<T>
    {
		/// <summary>
		/// The <see cref="Section"/> instance being built by the builder.
		/// </summary>
		protected readonly Section _section;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder{T}"/> class.
		/// </summary>
		public DomSectionBuilder()
        {
            _section = new Section();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder{T}"/> class with a specified section definition.
		/// </summary>
		/// <param name="definition">The section definition.</param>
		public DomSectionBuilder(SectionDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            _section = new Section(definition);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder{T}"/> class with a specified section definition ID.
		/// </summary>
		/// <param name="definitionID">The ID of the section definition.</param>
		public DomSectionBuilder(SectionDefinitionID definitionID)
        {
            if (definitionID == null)
            {
                throw new ArgumentNullException(nameof(definitionID));
            }

            _section = new Section(definitionID);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder{T}"/> class with a specified section.
		/// </summary>
		/// <param name="section">The existing section to be used in the builder.</param>
		public DomSectionBuilder(Section section)
        {
            _section = section ?? throw new ArgumentNullException(nameof(section));
        }

		/// <summary>
		/// Builds the configured <see cref="Section"/> instance.
		/// </summary>
		/// <returns>The constructed <see cref="Section"/>.</returns>
		public Section Build()
        {
            return _section;
        }

		/// <summary>
		/// Sets the ID of the section.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(SectionID id)
        {
            _section.ID = id ?? throw new ArgumentNullException(nameof(id));

            return (T)this;
        }

		/// <summary>
		/// Sets the ID of the section using a <see cref="Guid"/>.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(Guid id)
        {
            return WithID(new SectionID(id));
        }

		/// <summary>
		/// Sets the section definition ID for the section.
		/// </summary>
		/// <param name="sectionDefinitionID">The section definition ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithSectionDefinitionID(SectionDefinitionID sectionDefinitionID)
        {
            _section.SectionDefinitionID = sectionDefinitionID ?? throw new ArgumentNullException(nameof(sectionDefinitionID));

            return (T)this;
        }

		/// <summary>
		/// Sets the field value for a specific field descriptor ID.
		/// </summary>
		/// <typeparam name="TValue">The type of the field value.</typeparam>
		/// <param name="fieldDescriptorID">The field descriptor ID.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithFieldValue<TValue>(FieldDescriptorID fieldDescriptorID, TValue value)
        {
            if (fieldDescriptorID == null)
            {
                throw new ArgumentNullException(nameof(fieldDescriptorID));
            }

            _section.SetFieldValue(fieldDescriptorID, value);

            return (T)this;
        }

		/// <summary>
		/// Sets the field value for a specific field descriptor.
		/// </summary>
		/// <typeparam name="TValue">The type of the field value.</typeparam>
		/// <param name="fieldDescriptor">The field descriptor.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithFieldValue<TValue>(FieldDescriptor fieldDescriptor, TValue value)
        {
            if (fieldDescriptor == null)
            {
                throw new ArgumentNullException(nameof(fieldDescriptor));
            }

            _section.SetFieldValue(fieldDescriptor.ID, value);

            return (T)this;
        }
    }

	/// <summary>
	/// Represents a builder for creating instances of the <see cref="Section"/> class.
	/// </summary>
	public class DomSectionBuilder : DomSectionBuilder<DomSectionBuilder>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder"/> class.
		/// </summary>
		public DomSectionBuilder()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder"/> class with a specified section definition.
		/// </summary>
		/// <param name="definition">The section definition.</param>
		public DomSectionBuilder(SectionDefinition definition) : base(definition)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder"/> class with a specified section definition ID.
		/// </summary>
		/// <param name="definitionID">The ID of the section definition.</param>
		public DomSectionBuilder(SectionDefinitionID definitionID) : base(definitionID)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSectionBuilder"/> class with a specified section.
		/// </summary>
		/// <param name="section">The existing section to be used in the builder.</param>
		public DomSectionBuilder(Section section) : base(section)
        {
        }
    }
}