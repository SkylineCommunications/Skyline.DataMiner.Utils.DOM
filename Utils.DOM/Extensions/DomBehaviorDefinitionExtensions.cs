namespace Skyline.DataMiner.Utils.DOM.Extensions
{
    using System;
    using System.Linq;

    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.CrudHelperComponents;
    using Skyline.DataMiner.Net.Messages.SLDataGateway;

	/// <summary>
	/// Extension methods for <see cref="DomBehaviorDefinition" />.
	/// </summary>
	public static class DomBehaviorDefinitionExtensions
    {
		/// <summary>
		/// Gets a <see cref="DomBehaviorDefinition"/> by its ID.
		/// </summary>
		/// <param name="helper">The <see cref="DomBehaviorDefinitionCrudHelperComponent"/> instance.</param>
		/// <param name="id">The ID of the <see cref="DomBehaviorDefinition"/> to retrieve.</param>
		/// <returns>The <see cref="DomBehaviorDefinition"/> with the specified ID, or null if not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> is null.</exception>
		public static DomBehaviorDefinition GetById(this DomBehaviorDefinitionCrudHelperComponent helper, Guid id)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            var filter = DomBehaviorDefinitionExposers.Id.Equal(id);

            return helper.Read(filter).SingleOrDefault();
        }

		/// <summary>
		/// Gets a <see cref="DomBehaviorDefinition"/> by its name.
		/// </summary>
		/// <param name="helper">The <see cref="DomBehaviorDefinitionCrudHelperComponent"/> instance.</param>
		/// <param name="name">The name of the <see cref="DomBehaviorDefinition"/> to retrieve.</param>
		/// <returns>The <see cref="DomBehaviorDefinition"/> with the specified name, or null if not found.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> is null.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
		public static DomBehaviorDefinition GetByName(this DomBehaviorDefinitionCrudHelperComponent helper, string name)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            var filter = DomBehaviorDefinitionExposers.Name.Equal(name);

            return helper.Read(filter).SingleOrDefault();
        }
    }
}
