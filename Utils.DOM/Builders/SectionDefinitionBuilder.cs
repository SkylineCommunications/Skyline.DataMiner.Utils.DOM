namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;

	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Generic builder class for creating instances of the <see cref="SectionDefinition"/> class.
	/// </summary>
	/// <typeparam name="T">The type of the builder class.</typeparam>
	public class SectionDefinitionBuilder<T> where T : SectionDefinitionBuilder<T>
	{
		private readonly CustomSectionDefinition _definition;

		/// <summary>
		/// Initializes a new instance of the <see cref="SectionDefinitionBuilder{T}"/> class.
		/// </summary>
		public SectionDefinitionBuilder()
		{
			_definition = new CustomSectionDefinition();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SectionDefinitionBuilder{T}"/> class with a specified section definition.
		/// </summary>
		/// <param name="definition">The existing section definition to be used in the builder.</param>
		public SectionDefinitionBuilder(SectionDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			if (!(definition is CustomSectionDefinition custom))
			{
				throw new NotSupportedException();
			}

			_definition = custom;
		}

		/// <summary>
		/// Builds the section definition.
		/// </summary>
		/// <returns>The constructed <see cref="SectionDefinition"/>.</returns>
		public SectionDefinition Build()
		{
			return _definition;
		}

		/// <summary>
		/// Sets the ID for the section definition.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(SectionDefinitionID id)
		{
			_definition.ID = id ?? throw new ArgumentNullException(nameof(id));

			return (T)this;
		}

		/// <summary>
		/// Sets the ID for the section definition using a GUID.
		/// </summary>
		/// <param name="id">The GUID to set as the ID.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(Guid id)
		{
			return WithID(new SectionDefinitionID(id));
		}

		/// <summary>
		/// Sets the name for the section definition.
		/// </summary>
		/// <param name="name">The name to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			_definition.Name = name;

			return (T)this;
		}

		/// <summary>
		/// Sets the reservation link info for the section definition.
		/// </summary>
		/// <param name="reservationLinkInfo">The reservation link info to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithReservationLinkInfo(ReservationLinkInfo reservationLinkInfo)
		{
			_definition.ReservationLinkInfo = reservationLinkInfo;

			return (T)this;
		}

		/// <summary>
		/// Adds a field descriptor to the section definition.
		/// </summary>
		/// <param name="fieldDescriptor">The field descriptor to add.</param>
		/// <returns>The builder instance.</returns>
		public T AddFieldDescriptor(FieldDescriptor fieldDescriptor)
		{
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			_definition.AddOrReplaceFieldDescriptor(fieldDescriptor);

			return (T)this;
		}

		/// <summary>
		/// Adds a field descriptor to the section definition using a builder.
		/// </summary>
		/// <typeparam name="TFieldDescriptor">The type of the field descriptor builder.</typeparam>
		/// <param name="fieldDescriptorBuilder">The field descriptor builder instance.</param>
		/// <returns>The builder instance.</returns>
		public T AddFieldDescriptor<TFieldDescriptor>(FieldDescriptorBuilder<TFieldDescriptor> fieldDescriptorBuilder)
			where TFieldDescriptor : FieldDescriptorBuilder<TFieldDescriptor>
		{
			if (fieldDescriptorBuilder == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorBuilder));
			}

			return AddFieldDescriptor(fieldDescriptorBuilder.Build());
		}

		/// <summary>
		/// Adds a field descriptor to the section definition using a builder action.
		/// </summary>
		/// <param name="builder">The builder action for creating a field descriptor.</param>
		/// <returns>The builder instance.</returns>
		public T AddFieldDescriptor(Action<FieldDescriptorBuilder> builder)
		{
			if (builder == null)
			{
				throw new ArgumentNullException(nameof(builder));
			}

			var fieldDescriptorBuilder = new FieldDescriptorBuilder();
			builder(fieldDescriptorBuilder);

			return AddFieldDescriptor(fieldDescriptorBuilder);
		}
	}

	/// <summary>
	/// Builder class for creating instances of the <see cref="CustomSectionDefinition"/> class.
	/// </summary>
	public class SectionDefinitionBuilder : SectionDefinitionBuilder<SectionDefinitionBuilder>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SectionDefinitionBuilder"/> class.
		/// </summary>
		public SectionDefinitionBuilder()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SectionDefinitionBuilder"/> class with a specified section definition.
		/// </summary>
		/// <param name="definition">The existing section definition to be used in the builder.</param>
		public SectionDefinitionBuilder(SectionDefinition definition) : base(definition)
		{
		}
	}
}