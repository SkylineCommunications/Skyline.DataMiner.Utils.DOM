namespace Skyline.DataMiner.Utils.DOM.Extensions
{
    using System;
    using System.Linq;

    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.CrudHelperComponents;
    using Skyline.DataMiner.Net.Messages.SLDataGateway;

    public static class DomBehaviorDefinitionExtensions
    {
        public static DomBehaviorDefinition GetById(this DomBehaviorDefinitionCrudHelperComponent helper, Guid id)
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            var filter = DomBehaviorDefinitionExposers.Id.Equal(id);

            return helper.Read(filter).SingleOrDefault();
        }

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
