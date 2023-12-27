namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Extension methods for <see cref="SectionDefinition"/>.
	/// </summary>
	public static class SectionDefinitionExtensions
	{
		/// <summary>
		/// Gets a <see cref="SectionDefinition"/> by ID.
		/// </summary>
		/// <param name="helper">The <see cref="SectionDefinitionCrudHelperComponent"/>.</param>
		/// <param name="id">The ID of the <see cref="SectionDefinition"/>.</param>
		/// <returns>The <see cref="SectionDefinition"/> with the specified ID, or null if not found.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="helper"/> is null.</exception>
		public static SectionDefinition GetByID(this SectionDefinitionCrudHelperComponent helper, Guid id)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			var filter = SectionDefinitionExposers.ID.Equal(id);

			return helper.Read(filter).SingleOrDefault();
		}

		/// <summary>
		/// Gets a <see cref="SectionDefinition"/> by name.
		/// </summary>
		/// <param name="helper">The <see cref="SectionDefinitionCrudHelperComponent"/>.</param>
		/// <param name="name">The name of the <see cref="SectionDefinition"/>.</param>
		/// <returns>The <see cref="SectionDefinition"/> with the specified name, or null if not found.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="helper"/> is null.</exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="name"/> is null or whitespace.
		/// </exception>
		public static SectionDefinition GetByName(this SectionDefinitionCrudHelperComponent helper, string name)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			var filter = SectionDefinitionExposers.Name.Equal(name);

			return helper.Read(filter).SingleOrDefault();
		}

		/// <summary>
		/// Gets a <see cref="FieldDescriptor"/> by name within the specified <see cref="SectionDefinition"/>.
		/// </summary>
		/// <param name="definition">The <see cref="SectionDefinition"/>.</param>
		/// <param name="name">The name of the <see cref="FieldDescriptor"/>.</param>
		/// <returns>The <see cref="FieldDescriptor"/> with the specified name.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="definition"/> is null.</exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="name"/> is null or whitespace, or the field descriptor doesn't exist in the definition.
		/// </exception>
		public static FieldDescriptor GetFieldDescriptorByName(this SectionDefinition definition, string name)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			var fieldDescriptor = definition.GetAllFieldDescriptors().SingleOrDefault(x => String.Equals(x.Name, name))
				?? throw new ArgumentException($"Field descriptor with name '{name}' doesn't exist in definition '{definition.GetName()}'.", nameof(name));

			return fieldDescriptor;
		}
	}
}