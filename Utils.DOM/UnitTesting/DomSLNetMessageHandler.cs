namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.CustomMessages;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Represents a handler for handling DMS messages related to DOM (Data Object Model) entities.
	/// </summary>
	public class DomSLNetMessageHandler
	{
		private readonly ConcurrentDictionary<Guid, DomDefinition> _definitions = new ConcurrentDictionary<Guid, DomDefinition>();
		private readonly ConcurrentDictionary<Guid, SectionDefinition> _sectionDefinitions = new ConcurrentDictionary<Guid, SectionDefinition>();
		private readonly ConcurrentDictionary<Guid, DomInstance> _instances = new ConcurrentDictionary<Guid, DomInstance>();
		private readonly ConcurrentDictionary<Guid, DomBehaviorDefinition> _behaviorDefinitions = new ConcurrentDictionary<Guid, DomBehaviorDefinition>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSLNetMessageHandler"/> class.
		/// </summary>
		public DomSLNetMessageHandler()
		{
		}

		/// <summary>
		/// Sets the DomDefinitions for the handler.
		/// </summary>
		/// <param name="definitions">The collection of DomDefinitions to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of DomDefinitions is null.</exception>
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

		/// <summary>
		/// Sets the SectionDefinitions for the handler.
		/// </summary>
		/// <param name="sectionDefinitions">The collection of SectionDefinitions to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of SectionDefinitions is null.</exception>
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

		/// <summary>
		/// Sets the DomInstances for the handler.
		/// </summary>
		/// <param name="instances">The collection of DomInstances to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of DomInstances is null.</exception>
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

		/// <summary>
		/// Sets the DomBehaviorDefinitions for the handler.
		/// </summary>
		/// <param name="definitions">The collection of DomBehaviorDefinitions to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of DomBehaviorDefinitions is null.</exception>
		public void SetBehaviorDefinitions(IEnumerable<DomBehaviorDefinition> definitions)
		{
			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			_behaviorDefinitions.Clear();

			foreach (var definition in definitions)
			{
				_behaviorDefinitions.TryAdd(definition.ID.SafeId(), definition);
			}
		}

		/// <summary>
		/// Handles an array of DMS messages, processing each message and returning an array of responses.
		/// </summary>
		/// <param name="messages">The array of DMS messages to handle.</param>
		/// <returns>An array of DMS messages representing the responses to the input messages.</returns>
		/// <exception cref="ArgumentNullException">Thrown when the input array of DMS messages is null.</exception>
		public DMSMessage[] HandleMessages(DMSMessage[] messages)
		{
			if (messages == null)
			{
				throw new ArgumentNullException(nameof(messages));
			}

			return messages.Select(HandleMessage).ToArray();
		}

		/// <summary>
		/// Handles a single DMS message, processing the message and returning a response.
		/// </summary>
		/// <param name="message">The DMS message to handle.</param>
		/// <returns>A DMS message representing the response to the input message.</returns>
		/// <exception cref="NotSupportedException">Thrown when the message type is not supported.</exception>
		public DMSMessage HandleMessage(DMSMessage message)
		{
			if (!TryHandleMessage(message, out var response))
			{
				throw new NotSupportedException($"Unsupported message type {message.GetType()}");
			}

			return response;
		}

		/// <summary>
		/// Tries to handle a DMS message, processing the message and providing a response.
		/// </summary>
		/// <param name="message">The DMS message to handle.</param>
		/// <param name="response">When the method returns, contains the response to the input message, if the message is supported; otherwise, the default value.</param>
		/// <returns><c>true</c> if the message is supported and a response is provided; otherwise, <c>false</c>.</returns>
		public bool TryHandleMessage(DMSMessage message, out DMSMessage response)
		{
			switch (message)
			{
				#region Definitions

				case ManagerStoreReadRequest<DomDefinition> request:
					var definitions = request.Query.ExecuteInMemory(_definitions.Values).ToList();
					response = new ManagerStoreCrudResponse<DomDefinition>(definitions);
					return true;

				case ManagerStoreCreateRequest<DomDefinition> request:
					_definitions[request.Object.ID.SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<DomDefinition>(request.Object);
					return true;

				case ManagerStoreUpdateRequest<DomDefinition> request:
					_definitions[request.Object.ID.SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<DomDefinition>(request.Object);
					return true;

				case ManagerStoreDeleteRequest<DomDefinition> request:
					_definitions.TryRemove(request.Object.ID.SafeId(), out _);
					response = new ManagerStoreCrudResponse<DomDefinition>(request.Object);
					return true;

				#endregion

				#region Section Definitions

				case ManagerStoreReadRequest<SectionDefinition> request:
					var sectionDefinitions = request.Query.ExecuteInMemory(_sectionDefinitions.Values).ToList();
					response = new ManagerStoreCrudResponse<SectionDefinition>(sectionDefinitions);
					return true;

				case ManagerStoreCreateRequest<SectionDefinition> request:
					_sectionDefinitions[request.Object.GetID().SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<SectionDefinition>(request.Object);
					return true;

				case ManagerStoreUpdateRequest<SectionDefinition> request:
					_sectionDefinitions[request.Object.GetID().SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<SectionDefinition>(request.Object);
					return true;

				case ManagerStoreDeleteRequest<SectionDefinition> request:
					_sectionDefinitions.TryRemove(request.Object.GetID().SafeId(), out _);
					response = new ManagerStoreCrudResponse<SectionDefinition>(request.Object);
					return true;

				#endregion

				#region Instances

				case ManagerStoreReadRequest<DomInstance> request:
					var instances = request.Query.ExecuteInMemory(_instances.Values).ToList();
					response = new ManagerStoreCrudResponse<DomInstance>(instances);
					return true;

				case ManagerStoreCreateRequest<DomInstance> request:
					_instances[request.Object.ID.SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<DomInstance>(request.Object);
					return true;

				case ManagerStoreUpdateRequest<DomInstance> request:
					_instances[request.Object.ID.SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<DomInstance>(request.Object);
					return true;

				case ManagerStoreDeleteRequest<DomInstance> request:
					_instances.TryRemove(request.Object.ID.SafeId(), out _);
					response = new ManagerStoreCrudResponse<DomInstance>(request.Object);
					return true;

				#endregion

				#region BehaviorDefinitions

				case ManagerStoreReadRequest<DomBehaviorDefinition> request:
					var behaviorDefinitions = request.Query.ExecuteInMemory(_behaviorDefinitions.Values).ToList();
					response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(behaviorDefinitions);
					return true;

				case ManagerStoreCreateRequest<DomBehaviorDefinition> request:
					_behaviorDefinitions[request.Object.ID.SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(request.Object);
					return true;

				case ManagerStoreUpdateRequest<DomBehaviorDefinition> request:
					_behaviorDefinitions[request.Object.ID.SafeId()] = request.Object;
					response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(request.Object);
					return true;

				case ManagerStoreDeleteRequest<DomBehaviorDefinition> request:
					_behaviorDefinitions.TryRemove(request.Object.ID.SafeId(), out _);
					response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(request.Object);
					return true;

				#endregion

				#region Status Transition

				case DomInstanceStatusTransitionRequestMessage request:
					response = HandleDomInstanceStatusTransitionRequestMessage(request);
					return true;

				#endregion

				default:
					response = default;
					return false;
			}
		}

		private DMSMessage HandleDomInstanceStatusTransitionRequestMessage(DomInstanceStatusTransitionRequestMessage request)
		{
			if (!_instances.TryGetValue(request.DomInstanceId.Id, out var instance))
			{
				throw new InvalidOperationException($"Couldn't find instance with ID '{request.DomInstanceId.Id}'");
			}

			if (!_definitions.TryGetValue(instance.DomDefinitionId.Id, out var definition))
			{
				throw new InvalidOperationException($"Couldn't find definition with ID '{instance.DomDefinitionId.Id}'");
			}

			if (!_behaviorDefinitions.TryGetValue(definition.DomBehaviorDefinitionId.Id, out var behavior))
			{
				throw new InvalidOperationException($"Couldn't find behavior with ID '{definition.DomBehaviorDefinitionId.Id}'");
			}

			var transition = behavior.StatusTransitions.FirstOrDefault(x => String.Equals(x.Id, request.TransitionId));
			if (transition == null)
			{
				throw new InvalidOperationException($"Couldn't find transition with ID '{request.TransitionId}'");
			}

			if (instance.StatusId != transition.FromStatusId)
			{
				throw new InvalidOperationException($"Instance doesn't have status '{transition.FromStatusId}'");
			}

			instance.StatusId = transition.ToStatusId;

			return new DomInstanceStatusTransitionResponseMessage { DomInstance = instance };
		}
	}
}
