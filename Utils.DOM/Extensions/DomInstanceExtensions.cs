namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Extension methods for <see cref="DomInstance"/>.
	/// </summary>
	public static class DomInstanceExtensions
	{
		/// <summary>
		/// Gets a <see cref="DomInstance"/> by ID.
		/// </summary>
		/// <param name="helper">The <see cref="DomInstanceCrudHelperComponent"/>.</param>
		/// <param name="id">The ID of the <see cref="DomInstance"/>.</param>
		/// <returns>The <see cref="DomInstance"/> with the specified ID, or null if not found.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="helper"/> is null.</exception>
		public static DomInstance GetByID(this DomInstanceCrudHelperComponent helper, Guid id)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			var filter = DomInstanceExposers.Id.Equal(id);

			return helper.Read(filter).SingleOrDefault();
		}

		/// <summary>
		/// Reads all <see cref="DomInstance"/> objects with the specified <see cref="DomDefinitionId"/>.
		/// </summary>
		/// <param name="helper">The <see cref="DomInstanceCrudHelperComponent"/>.</param>
		/// <param name="definitionId">The <see cref="DomDefinitionId"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="DomInstance"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="helper"/> or <paramref name="definitionId"/> is null.</exception>
		public static IEnumerable<DomInstance> ReadAll(this DomInstanceCrudHelperComponent helper, DomDefinitionId definitionId)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (definitionId == null)
			{
				throw new ArgumentNullException(nameof(definitionId));
			}

			var filter = DomInstanceExposers.DomDefinitionId.Equal(definitionId.Id);

			return helper.Read(filter);
		}

		/// <summary>
		/// Reads all <see cref="DomInstance"/> objects with the specified <see cref="DomDefinition"/>.
		/// </summary>
		/// <param name="helper">The <see cref="DomInstanceCrudHelperComponent"/>.</param>
		/// <param name="definition">The <see cref="DomDefinition"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="DomInstance"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="helper"/> or <paramref name="definition"/> is null.</exception>
		public static IEnumerable<DomInstance> ReadAll(this DomInstanceCrudHelperComponent helper, DomDefinition definition)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			return helper.ReadAll(definition.ID);
		}

		/// <summary>
		/// Gets sections with the specified <see cref="SectionDefinitionID"/> associated with the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="instance">The <see cref="DomInstance"/>.</param>
		/// <param name="definitionId">The <see cref="SectionDefinitionID"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Section"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> or <paramref name="definitionId"/> is null.</exception>
		public static IEnumerable<Section> GetSectionsWithDefinition(this DomInstance instance, SectionDefinitionID definitionId)
		{
			if (instance == null)
			{
				throw new ArgumentNullException(nameof(instance));
			}

			if (definitionId == null)
			{
				throw new ArgumentNullException(nameof(definitionId));
			}

			return instance.Sections.Where(x => x.SectionDefinitionID.Equals(definitionId));
		}

		/// <summary>
		/// Gets sections with the specified <see cref="SectionDefinition"/> associated with the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="instance">The <see cref="DomInstance"/>.</param>
		/// <param name="definition">The <see cref="SectionDefinition"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Section"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> or <paramref name="definition"/> is null.</exception>
		public static IEnumerable<Section> GetSectionsWithDefinition(this DomInstance instance, SectionDefinition definition)
		{
			if (instance == null)
			{
				throw new ArgumentNullException(nameof(instance));
			}

			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			var definitionId = definition.GetID();
			return instance.GetSectionsWithDefinition(definitionId);
		}

		/// <summary>
		/// Gets sections with the specified <paramref name="definitionName"/> associated with the <see cref="DomInstance"/>.
		/// </summary>
		/// <param name="instance">The <see cref="DomInstance"/>.</param>
		/// <param name="definitionName">The name of the section definition.</param>
		/// <param name="cache">The <see cref="DomCache"/>.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Section"/> objects.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="instance"/> or <paramref name="cache"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="definitionName"/> is null or whitespace.</exception>
		/// <exception cref="ArgumentException">Could not find section definition with the specified <paramref name="definitionName"/>.</exception>
		public static IEnumerable<Section> GetSectionsWithDefinition(this DomInstance instance, string definitionName, DomCache cache)
		{
			if (instance == null)
			{
				throw new ArgumentNullException(nameof(instance));
			}

			if (String.IsNullOrWhiteSpace(definitionName))
			{
				throw new ArgumentException($"'{nameof(definitionName)}' cannot be null or whitespace.", nameof(definitionName));
			}

			if (cache == null)
			{
				throw new ArgumentNullException(nameof(cache));
			}

			var definition = cache.GetSectionDefinitionByName(definitionName)
				?? throw new ArgumentException($"Couldn't find section definition with name '{definitionName}'", nameof(definitionName));

			return instance.GetSectionsWithDefinition(definition);
		}
	}
}