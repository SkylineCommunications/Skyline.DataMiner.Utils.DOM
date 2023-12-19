namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	public static class DomInstanceExtensions
	{
		public static DomInstance GetByID(this DomInstanceCrudHelperComponent helper, Guid id)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			var filter = DomInstanceExposers.Id.Equal(id);

			return helper.Read(filter).SingleOrDefault();
		}

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

			var definition = cache.GetSectionDefinitionByName(definitionName);

			if (definition == null)
			{
				throw new ArgumentException($"Couldn't find section definition with name '{definitionName}'", nameof(definitionName));
			}

			return instance.GetSectionsWithDefinition(definition);
		}
	}
}