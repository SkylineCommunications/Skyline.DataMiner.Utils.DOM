namespace Skyline.DataMiner.Utils.DOM.Builders
{
    using System;

    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions;
    using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Represents a builder for creating instances of <see cref="DomDefinition"/>.
	/// </summary>
	/// <typeparam name="T">The type of the derived builder class.</typeparam>
	public class DomDefinitionBuilder<T> where T : DomDefinitionBuilder<T>
    {
		/// <summary>
		/// The <see cref="DomDefinition"/> instance being built by the builder.
		/// </summary>
		protected readonly DomDefinition _definition;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomDefinitionBuilder{T}"/> class.
		/// </summary>
		public DomDefinitionBuilder()
        {
            _definition = new DomDefinition();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomDefinitionBuilder{T}"/> class with a specified <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="definition">The existing DOM definition to be used in the builder.</param>
		public DomDefinitionBuilder(DomDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

		/// <summary>
		/// Builds the <see cref="DomDefinition"/>.
		/// </summary>
		/// <returns>The built <see cref="DomDefinition"/>.</returns>
		public DomDefinition Build()
        {
            return _definition;
        }

		/// <summary>
		/// Sets the ID of the <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(DomDefinitionId id)
        {
            _definition.ID = id ?? throw new ArgumentNullException(nameof(id));

            return (T)this;
        }

		/// <summary>
		/// Sets the ID of the <see cref="DomDefinition"/> using a GUID.
		/// </summary>
		/// <param name="id">The GUID to set as the ID.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(Guid id)
        {
            return WithID(new DomDefinitionId(id));
        }

		/// <summary>
		/// Sets the name of the <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="name">The name to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithName(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            _definition.Name = name;

            return (T)this;
        }

		/// <summary>
		/// Sets the DOM behavior definition ID of the <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="domBehaviorDefinitionId">The DOM behavior definition ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithDomBehaviorDefinition(DomBehaviorDefinitionId domBehaviorDefinitionId)
        {
            _definition.DomBehaviorDefinitionId = domBehaviorDefinitionId ?? throw new ArgumentNullException(nameof(domBehaviorDefinitionId));

            return (T)this;
        }

		/// <summary>
		/// Adds a section definition link to the <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="sectionDefinitionLink">The section definition link to add.</param>
		/// <returns>The builder instance.</returns>
		public T AddSectionDefinitionLink(SectionDefinitionLink sectionDefinitionLink)
        {
            if (sectionDefinitionLink == null)
            {
                throw new ArgumentNullException(nameof(sectionDefinitionLink));
            }

            _definition.SectionDefinitionLinks.Add(sectionDefinitionLink);

            return (T)this;
        }

		/// <summary>
		/// Adds a section definition link to the <see cref="DomDefinition"/> using a section definition ID.
		/// </summary>
		/// <param name="sectionDefinitionID">The section definition ID to create a link with and add.</param>
		/// <returns>The builder instance.</returns>
		public T AddSectionDefinitionLink(SectionDefinitionID sectionDefinitionID)
        {
            if (sectionDefinitionID == null)
            {
                throw new ArgumentNullException(nameof(sectionDefinitionID));
            }

            var sectionDefinitionLink = new SectionDefinitionLink(sectionDefinitionID);

            return AddSectionDefinitionLink(sectionDefinitionLink);
        }
    }

	/// <summary>
	/// Represents a builder for creating instances of <see cref="DomDefinition"/>.
	/// </summary>
	public class DomDefinitionBuilder : DomDefinitionBuilder<DomDefinitionBuilder>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DomDefinitionBuilder"/> class.
		/// </summary>
		public DomDefinitionBuilder()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomDefinitionBuilder"/> class with a specified <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="definition">The existing DOM definition to be used in the builder.</param>
		public DomDefinitionBuilder(DomDefinition definition) : base(definition)
        {
        }
    }
}