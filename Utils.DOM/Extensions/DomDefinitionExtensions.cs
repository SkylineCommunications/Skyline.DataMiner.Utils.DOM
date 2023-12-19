namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	public static class DomDefinitionExtensions
	{
		public static DomDefinition GetByID(this DomDefinitionCrudHelperComponent helper, Guid id)
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			var filter = DomDefinitionExposers.Id.Equal(id);

			return helper.Read(filter).SingleOrDefault();
		}

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