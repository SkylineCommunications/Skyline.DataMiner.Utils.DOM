namespace Skyline.DataMiner.Utils.DOM
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
    using Skyline.DataMiner.Net.Messages;
    using Skyline.DataMiner.Net.Messages.SLDataGateway;
    using Skyline.DataMiner.Net.Sections;
    using Skyline.DataMiner.Utils.DOM.Extensions;

    public class DomCache
	{
		private readonly ConcurrentDictionary<Guid, DomInstance> _instancesById = new ConcurrentDictionary<Guid, DomInstance>();
		private readonly ConcurrentDictionary<DomDefinitionId, ICollection<DomInstance>> _instancesByDefinition = new ConcurrentDictionary<DomDefinitionId, ICollection<DomInstance>>();
		private readonly ConcurrentDictionary<Guid, DomDefinition> _definitionsById = new ConcurrentDictionary<Guid, DomDefinition>();
		private readonly ConcurrentDictionary<string, DomDefinition> _definitionsByName = new ConcurrentDictionary<string, DomDefinition>();
		private readonly ConcurrentDictionary<Guid, SectionDefinition> _sectionDefinitionsById = new ConcurrentDictionary<Guid, SectionDefinition>();
		private readonly ConcurrentDictionary<string, SectionDefinition> _sectionDefinitionsByName = new ConcurrentDictionary<string, SectionDefinition>();
		private readonly ConcurrentDictionary<Guid, DomBehaviorDefinition> _behaviorDefinitionsById = new ConcurrentDictionary<Guid, DomBehaviorDefinition>();
		private readonly ConcurrentDictionary<string, DomBehaviorDefinition> _behaviorDefinitonsByName = new ConcurrentDictionary<string, DomBehaviorDefinition>();

		public DomCache(DomHelper helper)
		{
			Helper = helper ?? throw new ArgumentNullException(nameof(helper));
		}

		public DomCache(Func<DMSMessage[], DMSMessage[]> messageHandler, string moduleId)
		{
			if (messageHandler == null)
			{
				throw new ArgumentNullException(nameof(messageHandler));
			}

			if (String.IsNullOrEmpty(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or empty.", nameof(moduleId));
			}

			Helper = new DomHelper(messageHandler, moduleId);
		}

		public DomHelper Helper { get; }

		public DomInstance GetInstanceById(Guid id)
		{
			return _instancesById.GetOrAdd(id, Helper.DomInstances.GetByID);
		}

		public IDictionary<Guid, DomInstance> GetInstancesById(IEnumerable<Guid> ids)
		{
			if (ids == null)
			{
				throw new ArgumentNullException(nameof(ids));
			}

			var result = new Dictionary<Guid, DomInstance>();
			var idsToRetrieve = new List<Guid>();

			foreach (var id in ids.Where(x => x != Guid.Empty).Distinct())
			{
				if (_instancesById.TryGetValue(id, out var instance))
					result[id] = instance;
				else
					idsToRetrieve.Add(id);
			}

			if (idsToRetrieve.Count > 0)
			{
				var instances = Net.Tools.RetrieveBigOrFilter(
					idsToRetrieve,
					x => DomInstanceExposers.Id.Equal(x),
					x => Helper.DomInstances.Read(x));

				foreach (var instance in instances)
				{
					result[instance.ID.Id] = instance;
					_instancesById[instance.ID.Id] = instance;
				}
			}

			return result;
		}

		public ICollection<DomInstance> GetInstances(FilterElement<DomInstance> filter)
		{
			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter));
			}

			var result = new List<DomInstance>();

			var instances = Helper.DomInstances.Read(filter);

			foreach (var instance in instances)
			{
				result.Add(instance);
				_instancesById[instance.ID.Id] = instance;
			}

			return result;
		}

		public ICollection<DomInstance> GetInstancesByDefinition(string definitionName)
		{
			if (String.IsNullOrWhiteSpace(definitionName))
			{
				throw new ArgumentException($"'{nameof(definitionName)}' cannot be null or whitespace.", nameof(definitionName));
			}

			var definition = GetDefinitionByName(definitionName);

			if (definition == null)
			{
				throw new ArgumentException($"Couldn't find definition with name '{definitionName}'");
			}

			return GetInstancesByDefinition(definition);
		}

		public ICollection<DomInstance> GetInstancesByDefinition(DomDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			return GetInstancesByDefinition(definition.ID);
		}

		public ICollection<DomInstance> GetInstancesByDefinition(DomDefinitionId definitionID)
		{
			if (definitionID == null)
			{
				throw new ArgumentNullException(nameof(definitionID));
			}

			return _instancesByDefinition.GetOrAdd(
				definitionID,
				d =>
				{
					var instances = Helper.DomInstances.ReadAll(definitionID).ToList();

					foreach (var instance in instances)
					{
						_instancesById[instance.ID.Id] = instance;
					}

					return instances;
				});
		}

		public DomDefinition GetDefinitionById(Guid id)
		{
			var definition = _definitionsById.GetOrAdd(id, Helper.DomDefinitions.GetByID);

			if (definition != null)
			{
				_definitionsByName.TryAdd(definition.Name, definition);
			}

			return definition;
		}

		public DomDefinition GetDefinitionByName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			var definition = _definitionsByName.GetOrAdd(name, Helper.DomDefinitions.GetByName);

			if (definition != null)
			{
				_definitionsById.TryAdd(definition.ID.Id, definition);
			}

			return definition;
		}

		public SectionDefinition GetSectionDefinitionById(Guid id)
		{
			var definition = _sectionDefinitionsById.GetOrAdd(id, Helper.SectionDefinitions.GetByID);

			if (definition != null)
			{
				_sectionDefinitionsByName.TryAdd(definition.GetName(), definition);
			}

			return definition;
		}

		public SectionDefinition GetSectionDefinitionByName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			var definition = _sectionDefinitionsByName.GetOrAdd(name, Helper.SectionDefinitions.GetByName);

			if (definition != null)
			{
				_sectionDefinitionsById.TryAdd(definition.GetID().Id, definition);
			}

			return definition;
		}

		public FieldDescriptor GetFieldDescriptorByName(string sectionDefinitionName, string fieldName)
		{
			if (String.IsNullOrWhiteSpace(sectionDefinitionName))
			{
				throw new ArgumentException($"'{nameof(sectionDefinitionName)}' cannot be null or whitespace.", nameof(sectionDefinitionName));
			}

			if (String.IsNullOrWhiteSpace(fieldName))
			{
				throw new ArgumentException($"'{nameof(fieldName)}' cannot be null or whitespace.", nameof(fieldName));
			}

			return GetSectionDefinitionByName(sectionDefinitionName).GetFieldDescriptorByName(fieldName);
		}

		public FieldDescriptor GetFieldDescriptor(SectionDefinitionID sectionDefinitionID, FieldDescriptorID fieldDescriptorID)
		{
			if (sectionDefinitionID == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionID));
			}

			if (fieldDescriptorID == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptorID));
			}

			return GetSectionDefinitionById(sectionDefinitionID.Id).GetFieldDescriptorById(fieldDescriptorID);
		}

		public DomBehaviorDefinition GetBehaviorDefinitionById(Guid id)
		{
			var behaviorDefinition = _behaviorDefinitionsById.GetOrAdd(id, Helper.DomBehaviorDefinitions.GetById);

			if (behaviorDefinition != null)
			{
				_behaviorDefinitonsByName.TryAdd(behaviorDefinition.Name, behaviorDefinition);
			}

			return behaviorDefinition;
		}

		public DomBehaviorDefinition GetBehaviorDefinitionByName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			var behaviorDefinition = _behaviorDefinitonsByName.GetOrAdd(name, Helper.DomBehaviorDefinitions.GetByName);

			if (behaviorDefinition != null)
			{
				_behaviorDefinitionsById.TryAdd(behaviorDefinition.ID.Id, behaviorDefinition);
			}

			return behaviorDefinition;
		}
	}
}