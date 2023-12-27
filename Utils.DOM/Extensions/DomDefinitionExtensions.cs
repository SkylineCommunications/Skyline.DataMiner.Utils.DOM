namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Extension methods for <see cref="DomDefinition"/>.
	/// </summary>
	public static class DomDefinitionExtensions
	{
		/// <summary>
		/// Gets a <see cref="DomDefinition"/> by its ID.
		/// </summary>
		/// <param name="helper">The <see cref="DomDefinitionCrudHelperComponent"/> instance.</param>
		/// <param name="id">The ID of the <see cref="DomDefinition"/> to retrieve.</param>
		/// <returns>The <see cref="DomDefinition"/> with the specified ID, or null if not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> is null.</exception>
		public static DomDefinition GetByID(this DomDefinitionCrudHelperComponent helper, Guid id)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			var filter = DomDefinitionExposers.Id.Equal(id);

			return helper.Read(filter).SingleOrDefault();
		}

		/// <summary>
		/// Gets a <see cref="DomDefinition"/> by its name.
		/// </summary>
		/// <param name="helper">The <see cref="DomDefinitionCrudHelperComponent"/> instance.</param>
		/// <param name="name">The name of the <see cref="DomDefinition"/> to retrieve.</param>
		/// <returns>The <see cref="DomDefinition"/> with the specified name, or null if not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
		public static DomDefinition GetByName(this DomDefinitionCrudHelperComponent helper, string name)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			var filter = DomDefinitionExposers.Name.Equal(name);

			return helper.Read(filter).SingleOrDefault();
		}
	}
}