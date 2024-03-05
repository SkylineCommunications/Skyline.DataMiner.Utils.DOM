namespace Skyline.DataMiner.Utils.DOM.Builders
{
    using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Status;
	using Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions;
    using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Represents a builder for creating instances of <see cref="DomBehaviorDefinition"/>.
	/// </summary>
	/// <typeparam name="T">The type of the derived builder class.</typeparam>
	public class DomBehaviorDefinitionBuilder<T> where T : DomBehaviorDefinitionBuilder<T>
    {
		/// <summary>
		/// The <see cref="DomBehaviorDefinition"/> instance being built by the builder.
		/// </summary>
		protected readonly DomBehaviorDefinition _definition;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomBehaviorDefinitionBuilder{T}"/> class.
		/// </summary>
		public DomBehaviorDefinitionBuilder()
        {
            _definition = new DomBehaviorDefinition();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomBehaviorDefinitionBuilder{T}"/> class with a specified <see cref="DomBehaviorDefinition"/>.
		/// </summary>
		/// <param name="definition">The existing DOM behavior definition to be used in the builder.</param>
		public DomBehaviorDefinitionBuilder(DomBehaviorDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

		/// <summary>
		/// Builds the <see cref="DomBehaviorDefinition"/>.
		/// </summary>
		/// <returns>The built <see cref="DomBehaviorDefinition"/>.</returns>
		public DomBehaviorDefinition Build()
        {
            return _definition;
        }

		/// <summary>
		/// Sets the ID of the <see cref="DomBehaviorDefinition"/>.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(DomBehaviorDefinitionId id)
        {
            _definition.ID = id ?? throw new ArgumentNullException(nameof(id));

            return (T)this;
        }

		/// <summary>
		/// Sets the ID of the <see cref="DomBehaviorDefinition"/> using a GUID.
		/// </summary>
		/// <param name="id">The GUID to set as the ID.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(Guid id)
        {
            return WithID(new DomBehaviorDefinitionId(id));
        }

		/// <summary>
		/// Sets the name of the <see cref="DomBehaviorDefinition"/>.
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
		/// Sets the initial status ID.
		/// </summary>
		/// <param name="initialStatusId">The initial status to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithInitialStatusId(string initialStatusId)
		{
			_definition.InitialStatusId = initialStatusId;

			return (T)this;
		}

		/// <summary>
		/// Sets the statuses.
		/// </summary>
		/// <param name="statuses">The statuses to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithStatuses(IEnumerable<DomStatus> statuses)
		{
			if (statuses == null)
			{
				throw new ArgumentNullException(nameof(statuses));
			}

			_definition.Statuses = statuses.ToList();

			return (T)this;
		}

		/// <summary>
		/// Sets the status transitions.
		/// </summary>
		/// <param name="transitions">The transitions to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithStatusTransitions(IEnumerable<DomStatusTransition> transitions)
		{
			if (transitions == null)
			{
				throw new ArgumentNullException(nameof(transitions));
			}

			_definition.StatusTransitions = transitions.ToList();

			return (T)this;
		}
	}

	/// <summary>
	/// Represents a builder for creating instances of <see cref="DomBehaviorDefinition"/>.
	/// </summary>
	public class DomBehaviorDefinitionBuilder : DomBehaviorDefinitionBuilder<DomBehaviorDefinitionBuilder>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="DomBehaviorDefinitionBuilder"/> class.
		/// </summary>
		public DomBehaviorDefinitionBuilder()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="DomBehaviorDefinitionBuilder"/> class with a specified <see cref="DomBehaviorDefinition"/>.
		/// </summary>
		/// <param name="definition">The existing DOM behavior definition to be used in the builder.</param>
		public DomBehaviorDefinitionBuilder(DomBehaviorDefinition definition) : base(definition)
        {
        }
    }
}