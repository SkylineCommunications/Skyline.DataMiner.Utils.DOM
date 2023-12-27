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

	/// <summary>
	/// Represents a cache for storing and retrieving DOM (DataMiner Object Model) objects.
	/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="DomCache"/> class with a specified <see cref="DomHelper"/>.
		/// </summary>
		/// <param name="helper">The <see cref="DomHelper"/> used for interaction with the DOM.</param>
		public DomCache(DomHelper helper)
		{
			Helper = helper ?? throw new ArgumentNullException(nameof(helper));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomCache"/>.
		/// </summary>
		/// <param name="messageHandler">The message handler for processing messages.</param>
		/// <param name="moduleId">The module ID associated with the DOM cache.</param>
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

		/// <summary>
		/// Gets the <see cref="DomHelper"/> associated with the DOM cache.
		/// </summary>
		public DomHelper Helper { get; }

		/// <summary>
		/// Retrieves a DOM instance by its unique ID.
		/// </summary>
		/// <param name="id">The unique ID of the DOM instance.</param>
		/// <returns>The <see cref="DomInstance"/> with the specified ID.</returns>
		public DomInstance GetInstanceById(Guid id)
		{
			return _instancesById.GetOrAdd(id, Helper.DomInstances.GetByID);
		}

		/// <summary>
		/// Retrieves a dictionary of DOM instances by their unique IDs.
		/// </summary>
		/// <param name="ids">The collection of unique IDs of DOM instances.</param>
		/// <returns>A dictionary containing the <see cref="DomInstance"/> objects with their respective IDs.</returns>
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

		/// <summary>
		/// Retrieves a collection of DOM instances based on a specified filter.
		/// </summary>
		/// <param name="filter">The filter condition for selecting DOM instances.</param>
		/// <returns>A collection of <see cref="DomInstance"/> objects that match the filter condition.</returns>
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

		/// <summary>
		/// Retrieves a collection of DOM instances associated with a specific definition.
		/// </summary>
		/// <param name="definitionName">The name of the DOM definition.</param>
		/// <returns>A collection of <see cref="DomInstance"/> objects associated with the specified definition.</returns>
		public ICollection<DomInstance> GetInstancesByDefinition(string definitionName)
		{
			if (String.IsNullOrWhiteSpace(definitionName))
			{
				throw new ArgumentException($"'{nameof(definitionName)}' cannot be null or whitespace.", nameof(definitionName));
			}

			var definition = GetDefinitionByName(definitionName);

			return definition != null
				? GetInstancesByDefinition(definition)
				: throw new ArgumentException($"Couldn't find definition with name '{definitionName}'");
		}

		/// <summary>
		/// Retrieves a collection of DOM instances associated with a specific definition.
		/// </summary>
		/// <param name="definition">The DOM definition object.</param>
		/// <returns>A collection of <see cref="DomInstance"/> objects associated with the specified definition.</returns>
		public ICollection<DomInstance> GetInstancesByDefinition(DomDefinition definition)
		{
			if (definition == null)
			{
				throw new ArgumentNullException(nameof(definition));
			}

			return GetInstancesByDefinition(definition.ID);
		}

		/// <summary>
		/// Retrieves a collection of DOM instances associated with a specific definition ID.
		/// </summary>
		/// <param name="definitionID">The unique ID of the DOM definition.</param>
		/// <returns>A collection of <see cref="DomInstance"/> objects associated with the specified definition ID.</returns>
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

		/// <summary>
		/// Retrieves a DOM definition by its unique ID.
		/// </summary>
		/// <param name="id">The unique ID of the DOM definition.</param>
		/// <returns>The <see cref="DomDefinition"/> with the specified ID.</returns>
		public DomDefinition GetDefinitionById(Guid id)
		{
			var definition = _definitionsById.GetOrAdd(id, Helper.DomDefinitions.GetByID);

			if (definition != null)
			{
				_definitionsByName.TryAdd(definition.Name, definition);
			}

			return definition;
		}

		/// <summary>
		/// Retrieves a DOM definition by its name.
		/// </summary>
		/// <param name="name">The name of the DOM definition.</param>
		/// <returns>The <see cref="DomDefinition"/> with the specified name.</returns>
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

		/// <summary>
		/// Retrieves a section definition by its unique ID.
		/// </summary>
		/// <param name="id">The unique ID of the section definition.</param>
		/// <returns>The <see cref="SectionDefinition"/> with the specified ID.</returns>
		public SectionDefinition GetSectionDefinitionById(Guid id)
		{
			var definition = _sectionDefinitionsById.GetOrAdd(id, Helper.SectionDefinitions.GetByID);

			if (definition != null)
			{
				_sectionDefinitionsByName.TryAdd(definition.GetName(), definition);
			}

			return definition;
		}

		/// <summary>
		/// Retrieves a section definition by its name.
		/// </summary>
		/// <param name="name">The name of the section definition.</param>
		/// <returns>The <see cref="SectionDefinition"/> with the specified name.</returns>
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

		/// <summary>
		/// Retrieves a field descriptor by section definition name and field name.
		/// </summary>
		/// <param name="sectionDefinitionName">The name of the section definition.</param>
		/// <param name="fieldName">The name of the field.</param>
		/// <returns>The <see cref="FieldDescriptor"/> with the specified section definition name and field name.</returns>
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

		/// <summary>
		/// Retrieves a field descriptor by section definition ID and field descriptor ID.
		/// </summary>
		/// <param name="sectionDefinitionID">The unique ID of the section definition.</param>
		/// <param name="fieldDescriptorID">The unique ID of the field descriptor.</param>
		/// <returns>The <see cref="FieldDescriptor"/> with the specified section definition ID and field descriptor ID.</returns>
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

		/// <summary>
		/// Retrieves a DOM behavior definition by its unique ID.
		/// </summary>
		/// <param name="id">The unique ID of the DOM behavior definition.</param>
		/// <returns>The <see cref="DomBehaviorDefinition"/> with the specified ID.</returns>
		public DomBehaviorDefinition GetBehaviorDefinitionById(Guid id)
		{
			var behaviorDefinition = _behaviorDefinitionsById.GetOrAdd(id, Helper.DomBehaviorDefinitions.GetById);

			if (behaviorDefinition != null)
			{
				_behaviorDefinitonsByName.TryAdd(behaviorDefinition.Name, behaviorDefinition);
			}

			return behaviorDefinition;
		}

		/// <summary>
		/// Retrieves a DOM behavior definition by its name.
		/// </summary>
		/// <param name="name">The name of the DOM behavior definition.</param>
		/// <returns>The <see cref="DomBehaviorDefinition"/> with the specified name.</returns>
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