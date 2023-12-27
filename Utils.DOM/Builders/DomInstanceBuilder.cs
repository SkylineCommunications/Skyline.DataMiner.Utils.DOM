namespace Skyline.DataMiner.Utils.DOM.Builders
{
    using System;

    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Sections;
    using Skyline.DataMiner.Utils.DOM.Extensions;

	/// <summary>
	/// Represents a builder for creating instances of <see cref="DomInstance"/>.
	/// </summary>
	/// <typeparam name="T">The type of the derived builder class.</typeparam>
	public class DomInstanceBuilder<T> where T : DomInstanceBuilder<T>
    {
        private readonly DomInstance _instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder{T}"/> class.
		/// </summary>
		public DomInstanceBuilder()
        {
            _instance = new DomInstance();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder{T}"/> class with a specified <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="definition">The DOM definition associated with the instance.</param>
		public DomInstanceBuilder(DomDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            _instance = new DomInstance { DomDefinitionId = definition.ID };
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder{T}"/> class with a specified <see cref="DomDefinitionId"/>.
		/// </summary>
		/// <param name="definitionID">The ID of the DOM definition associated with the instance.</param>
		public DomInstanceBuilder(DomDefinitionId definitionID)
        {
            if (definitionID == null)
            {
                throw new ArgumentNullException(nameof(definitionID));
            }

            _instance = new DomInstance { DomDefinitionId = definitionID };
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder{T}"/> class with a specified <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="instance">The existing DOM instance to be used in the builder.</param>
		public DomInstanceBuilder(DomInstance instance)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

		/// <summary>
		/// Builds the <see cref="DomInstance"/>.
		/// </summary>
		/// <returns>The built <see cref="DomInstance"/>.</returns>
		public DomInstance Build()
        {
            return _instance;
        }

		/// <summary>
		/// Sets the ID of the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(DomInstanceId id)
        {
            _instance.ID = id ?? throw new ArgumentNullException(nameof(id));

            return (T)this;
        }

		/// <summary>
		/// Sets the ID of the <see cref="DomInstance"/> using a GUID.
		/// </summary>
		/// <param name="id">The GUID to set as the ID.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(Guid id)
        {
            return WithID(new DomInstanceId(id));
        }

		/// <summary>
		/// Sets the DOM definition ID of the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="definitionId">The DOM definition ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithDefinition(DomDefinitionId definitionId)
        {
            _instance.DomDefinitionId = definitionId ?? throw new ArgumentNullException(nameof(definitionId));

            return (T)this;
        }

		/// <summary>
		/// Sets the DOM definition of the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="definition">The DOM definition to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithDefinition(DomDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            return WithDefinition(definition.ID);
        }

		/// <summary>
		/// Sets the status ID of the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="statusId">The status ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithStatusID(string statusId)
        {
            _instance.StatusId = statusId;

            return (T)this;
        }

		/// <summary>
		/// Adds a section to the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="section">The section to add.</param>
		/// <returns>The builder instance.</returns>
		public T AddSection(Section section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            _instance.Sections.Add(section);

            return (T)this;
        }

		/// <summary>
		/// Adds a section to the <see cref="DomInstance"/> using a section builder.
		/// </summary>
		/// <typeparam name="TSection">The type of the section builder.</typeparam>
		/// <param name="sectionBuilder">The section builder to add.</param>
		/// <returns>The builder instance.</returns>
		public T AddSection<TSection>(DomSectionBuilder<TSection> sectionBuilder)
            where TSection : DomSectionBuilder<TSection>
        {
            if (sectionBuilder == null)
            {
                throw new ArgumentNullException(nameof(sectionBuilder));
            }

            return AddSection(sectionBuilder.Build());
        }

		/// <summary>
		/// Adds a section to the <see cref="DomInstance"/> using a section definition ID and a builder action.
		/// </summary>
		/// <param name="sectionDefinitionID">The section definition ID to create a section with.</param>
		/// <param name="builder">The builder action to configure the section.</param>
		/// <returns>The builder instance.</returns>
		public T AddSection(SectionDefinitionID sectionDefinitionID, Action<DomSectionBuilder> builder)
        {
            if (sectionDefinitionID == null)
            {
                throw new ArgumentNullException(nameof(sectionDefinitionID));
            }

            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var sectionBuilder = new DomSectionBuilder(sectionDefinitionID);
            builder(sectionBuilder);

            AddSection(sectionBuilder);

            return (T)this;
        }

		/// <summary>
		/// Sets the field value for a specific section definition ID and field descriptor ID in the <see cref="DomInstance"/>.
		/// </summary>
		/// <typeparam name="TValue">The type of the field value.</typeparam>
		/// <param name="sectionDefinitionID">The section definition ID.</param>
		/// <param name="fieldDescriptorID">The field descriptor ID.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithFieldValue<TValue>(SectionDefinitionID sectionDefinitionID, FieldDescriptorID fieldDescriptorID, TValue value)
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

		/// <summary>
		/// Sets the field value for a specific section definition and field descriptor in the <see cref="DomInstance"/>.
		/// </summary>
		/// <typeparam name="TValue">The type of the field value.</typeparam>
		/// <param name="sectionDefinition">The section definition.</param>
		/// <param name="fieldDescriptor">The field descriptor.</param>
		/// <param name="value">The value to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithFieldValue<TValue>(SectionDefinition sectionDefinition, FieldDescriptor fieldDescriptor, TValue value)
        {
            if (sectionDefinition == null)
            {
                throw new ArgumentNullException(nameof(sectionDefinition));
            }

            if (fieldDescriptor == null)
            {
                throw new ArgumentNullException(nameof(fieldDescriptor));
            }

            return WithFieldValue(sectionDefinition.GetID(), fieldDescriptor.ID, value);
        }
    }

	/// <summary>
	/// Represents a builder for creating instances of <see cref="DomInstance"/>.
	/// </summary>
	public class DomInstanceBuilder : DomInstanceBuilder<DomInstanceBuilder>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder"/> class.
		/// </summary>
		public DomInstanceBuilder()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder"/> class with a specified <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="definition">The DOM definition associated with the instance.</param>
		public DomInstanceBuilder(DomDefinition definition) : base(definition)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder"/> class with a specified <see cref="DomDefinitionId"/>.
		/// </summary>
		/// <param name="definitionID">The ID of the DOM definition associated with the instance.</param>
		public DomInstanceBuilder(DomDefinitionId definitionID) : base(definitionID)
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceBuilder"/> class with a specified <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="instance">The existing DOM instance to be used in the builder.</param>
		public DomInstanceBuilder(DomInstance instance) : base(instance)
        {
        }
    }
}