namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Sections;

	internal class MockMessageHandler
	{
		private readonly ConcurrentDictionary<Guid, DomDefinition> _definitions = new ConcurrentDictionary<Guid, DomDefinition>();
		private readonly ConcurrentDictionary<Guid, SectionDefinition> _sectionDefinitions = new ConcurrentDictionary<Guid, SectionDefinition>();
		private readonly ConcurrentDictionary<Guid, DomInstance> _instances = new ConcurrentDictionary<Guid, DomInstance>();

		internal MockMessageHandler()
		{
		}

		public void SetDefinitions(IEnumerable<DomDefinition> definitions)
		{
			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			_definitions.Clear();

			foreach (var definition in definitions)
			{
				_definitions.TryAdd(definition.ID.SafeId(), definition);
			}
		}

		public void SetSectionDefinitions(IEnumerable<SectionDefinition> sectionDefinitions)
		{
			if (sectionDefinitions == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitions));
			}

			_sectionDefinitions.Clear();

			foreach (var definition in sectionDefinitions)
			{
				_sectionDefinitions.TryAdd(definition.GetID().SafeId(), definition);
			}
		}

		public void SetInstances(IEnumerable<DomInstance> instances)
		{
			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			_instances.Clear();

			foreach (var instance in instances)
			{
				_instances.TryAdd(instance.ID.SafeId(), instance);
			}
		}

		public DMSMessage[] HandleMessages(DMSMessage[] messages)
		{
			if (messages == null)
			{
				throw new ArgumentNullException(nameof(messages));
			}

			return messages.Select(HandleMessage).ToArray();
		}

		private DMSMessage HandleMessage(DMSMessage message)
		{
			switch (message)
			{
				#region Definitions

				case ManagerStoreReadRequest<DomDefinition> request:
					var definitions = request.Query.ExecuteInMemory(_definitions.Values).ToList();
					return new ManagerStoreCrudResponse<DomDefinition>(definitions);

				case ManagerStoreCreateRequest<DomDefinition> request:
					_definitions[request.Object.ID.SafeId()] = request.Object;
					return new ManagerStoreCrudResponse<DomDefinition>(request.Object);

				case ManagerStoreUpdateRequest<DomDefinition> request:
					_definitions[request.Object.ID.SafeId()] = request.Object;
					return new ManagerStoreCrudResponse<DomDefinition>(request.Object);

				case ManagerStoreDeleteRequest<DomDefinition> request:
					_definitions.TryRemove(request.Object.ID.SafeId(), out _);
					return new ManagerStoreCrudResponse<DomDefinition>(request.Object);

				#endregion

				#region Section Definitions

				case ManagerStoreReadRequest<SectionDefinition> request:
					var sectionDefinitions = request.Query.ExecuteInMemory(_sectionDefinitions.Values).ToList();
					return new ManagerStoreCrudResponse<SectionDefinition>(sectionDefinitions);

				case ManagerStoreCreateRequest<SectionDefinition> request:
					_sectionDefinitions[request.Object.GetID().SafeId()] = request.Object;
					return new ManagerStoreCrudResponse<SectionDefinition>(request.Object);

				case ManagerStoreUpdateRequest<SectionDefinition> request:
					_sectionDefinitions[request.Object.GetID().SafeId()] = request.Object;
					return new ManagerStoreCrudResponse<SectionDefinition>(request.Object);

				case ManagerStoreDeleteRequest<SectionDefinition> request:
					_sectionDefinitions.TryRemove(request.Object.GetID().SafeId(), out _);
					return new ManagerStoreCrudResponse<SectionDefinition>(request.Object);

				#endregion

				#region Instances

				case ManagerStoreReadRequest<DomInstance> request:
					var instances = request.Query.ExecuteInMemory(_instances.Values).ToList();
					return new ManagerStoreCrudResponse<DomInstance>(instances);

				case ManagerStoreCreateRequest<DomInstance> request:
					_instances[request.Object.ID.SafeId()] = request.Object;
					return new ManagerStoreCrudResponse<DomInstance>(request.Object);

				case ManagerStoreUpdateRequest<DomInstance> request:
					_instances[request.Object.ID.SafeId()] = request.Object;
					return new ManagerStoreCrudResponse<DomInstance>(request.Object);

				case ManagerStoreDeleteRequest<DomInstance> request:
					_instances.TryRemove(request.Object.ID.SafeId(), out _);
					return new ManagerStoreCrudResponse<DomInstance>(request.Object);

				#endregion

				default:
					throw new NotSupportedException($"Unsupported message type {message.GetType()}");
			}
		}
	}
}
