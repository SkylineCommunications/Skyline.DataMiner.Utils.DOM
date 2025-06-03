namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.CustomMessages;
	using Skyline.DataMiner.Net.Apps.Modules;
	using Skyline.DataMiner.Net.Helper;
	using Skyline.DataMiner.Net.ManagerStore;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	/// <summary>
	/// Represents a handler for handling DMS messages related to DOM (Data Object Model) entities.
	/// </summary>
	public class DomSLNetMessageHandler
	{
		private readonly ConcurrentDictionary<string, DomModule> _domModules = new ConcurrentDictionary<string, DomModule>();

		/// <summary>
		/// Initializes a new instance of the <see cref="DomSLNetMessageHandler"/> class.
		/// </summary>
		public DomSLNetMessageHandler()
		{
		}

		/// <summary>
		/// Occurs when DOM instances have changed.
		/// </summary>
		public event EventHandler<DomInstancesChangedEventMessage> OnInstancesChanged;

		/// <summary>
		/// Sets the DomDefinitions for the handler.
		/// </summary>
		/// <param name="moduleId">The ID of the DOM module.</param>
		/// <param name="definitions">The collection of DomDefinitions to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of DomDefinitions is null.</exception>
		public void SetDefinitions(string moduleId, IEnumerable<DomDefinition> definitions)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			var module = GetDomModule(moduleId);

			module.Definitions.Clear();

			foreach (var definition in definitions)
			{
				module.Definitions.TryAdd(definition.ID, definition);
			}
		}

		/// <summary>
		/// Sets the SectionDefinitions for the handler.
		/// </summary>
		/// <param name="moduleId">The ID of the DOM module.</param>
		/// <param name="sectionDefinitions">The collection of SectionDefinitions to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of SectionDefinitions is null.</exception>
		public void SetSectionDefinitions(string moduleId, IEnumerable<SectionDefinition> sectionDefinitions)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			if (sectionDefinitions == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitions));
			}

			var module = GetDomModule(moduleId);

			module.SectionDefinitions.Clear();

			foreach (var definition in sectionDefinitions)
			{
				module.SectionDefinitions.TryAdd(definition.GetID(), definition);
			}
		}

		/// <summary>
		/// Sets the DomInstances for the handler.
		/// </summary>
		/// <param name="moduleId">The ID of the DOM module.</param>
		/// <param name="instances">The collection of DomInstances to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of DomInstances is null.</exception>
		public void SetInstances(string moduleId, IEnumerable<DomInstance> instances)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var module = GetDomModule(moduleId);

			module.Instances.Clear();

			foreach (var instance in instances)
			{
				module.TrySetNameOnDomInstance(instance);
				module.Instances.TryAdd(instance.ID, instance);
			}
		}

		/// <summary>
		/// Sets the DomBehaviorDefinitions for the handler.
		/// </summary>
		/// <param name="moduleId">The ID of the DOM module.</param>
		/// <param name="definitions">The collection of DomBehaviorDefinitions to set.</param>
		/// <exception cref="ArgumentNullException">Thrown when the input collection of DomBehaviorDefinitions is null.</exception>
		public void SetBehaviorDefinitions(string moduleId, IEnumerable<DomBehaviorDefinition> definitions)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			var module = GetDomModule(moduleId);

			module.BehaviorDefinitions.Clear();

			foreach (var definition in definitions)
			{
				module.BehaviorDefinitions.TryAdd(definition.ID, definition);
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
					{
						var module = GetDomModule(request.ModuleId);
						var definitions = request.Query.ExecuteInMemory(module.Definitions.Values).ToList();
						response = new ManagerStoreCrudResponse<DomDefinition>(definitions);
						return true;
					}

				case ManagerStoreCreateRequest<DomDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackCreatedAt)request.Object).CreatedAt = utcNow;
						((ITrackCreatedBy)request.Object).CreatedBy = "DomSLNetMessageHandler";
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.Definitions[request.Object.ID] = request.Object;
						response = new ManagerStoreCrudResponse<DomDefinition>(request.Object);
						return true;
					}

				case ManagerStoreUpdateRequest<DomDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.Definitions[request.Object.ID] = request.Object;
						response = new ManagerStoreCrudResponse<DomDefinition>(request.Object);
						return true;
					}

				case ManagerStoreDeleteRequest<DomDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						module.Definitions.TryRemove(request.Object.ID, out _);
						response = new ManagerStoreCrudResponse<DomDefinition>(request.Object);
						return true;
					}

				#endregion

				#region Section Definitions

				case ManagerStoreReadRequest<SectionDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var sectionDefinitions = request.Query.ExecuteInMemory(module.SectionDefinitions.Values).ToList();
						response = new ManagerStoreCrudResponse<SectionDefinition>(sectionDefinitions);
						return true;
					}

				case ManagerStoreCreateRequest<SectionDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackCreatedAt)request.Object).CreatedAt = utcNow;
						((ITrackCreatedBy)request.Object).CreatedBy = "DomSLNetMessageHandler";
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.SectionDefinitions[request.Object.GetID()] = request.Object;
						response = new ManagerStoreCrudResponse<SectionDefinition>(request.Object);
						return true;
					}

				case ManagerStoreUpdateRequest<SectionDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.SectionDefinitions[request.Object.GetID()] = request.Object;
						response = new ManagerStoreCrudResponse<SectionDefinition>(request.Object);
						return true;
					}

				case ManagerStoreDeleteRequest<SectionDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						module.SectionDefinitions.TryRemove(request.Object.GetID(), out _);
						response = new ManagerStoreCrudResponse<SectionDefinition>(request.Object);
						return true;
					}

				#endregion

				#region Instances

				case ManagerStoreReadRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var instances = request.Query.ExecuteInMemory(module.Instances.Values).ToList();
						response = new ManagerStoreCrudResponse<DomInstance>(instances);
						return true;
					}

				case ManagerStoreCreateRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var instance = (DomInstance)request.Object.Clone();

						var utcNow = DateTime.UtcNow;
						((ITrackCreatedAt)instance).CreatedAt = utcNow;
						((ITrackCreatedBy)instance).CreatedBy = "DomSLNetMessageHandler";
						((ITrackLastModified)instance).LastModified = utcNow;
						((ITrackLastModifiedBy)instance).LastModifiedBy = "DomSLNetMessageHandler";

						module.TrySetNameOnDomInstance(instance);

						if (!module.Instances.TryAdd(instance.ID, instance))
						{
							throw new InvalidOperationException($"Instance with ID '{instance.ID.Id}' already exists in module '{request.ModuleId}'.");
						}

						var @event = new DomInstancesChangedEventMessage(-1, request.ModuleId);
						@event.Created.Add(instance);
						OnInstancesChanged?.Invoke(this, @event);

						response = new ManagerStoreCrudResponse<DomInstance>(instance);
						return true;
					}

				case ManagerStoreUpdateRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var instance = (DomInstance)request.Object.Clone();

						var utcNow = DateTime.UtcNow;
						((ITrackLastModified)instance).LastModified = utcNow;
						((ITrackLastModifiedBy)instance).LastModifiedBy = "DomSLNetMessageHandler";

						module.TrySetNameOnDomInstance(instance);

						if (!module.Instances.ContainsKey(instance.ID))
						{
							throw new InvalidOperationException($"Instance with ID '{instance.ID.Id}' not found in module '{request.ModuleId}'.");
						}

						module.Instances[instance.ID] = instance;

						var @event = new DomInstancesChangedEventMessage(-1, request.ModuleId);
						@event.Updated.Add(instance);
						OnInstancesChanged?.Invoke(this, @event);

						response = new ManagerStoreCrudResponse<DomInstance>(instance);
						return true;
					}

				case ManagerStoreDeleteRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var instance = (DomInstance)request.Object.Clone();

						var @event = new DomInstancesChangedEventMessage(-1, request.ModuleId);

						if (module.Instances.TryRemove(instance.ID, out var removed))
						{
							@event.Deleted.Add(instance);
						}
						else
						{
							throw new InvalidOperationException($"Instance with ID '{instance.ID.Id}' not found in module '{request.ModuleId}'.");
						}

						OnInstancesChanged?.Invoke(this, @event);

						response = new ManagerStoreCrudResponse<DomInstance>(instance);
						return true;
					}

				case ManagerStoreCountRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var count = request.Query.ExecuteInMemory(module.Instances.Values).LongCount();
						response = new ManagerStoreCountResponse<DomInstance>(count);
						return true;
					}

				case ManagerStoreBulkCreateOrUpdateRequest<DomInstance> request:
					{
						var utcNow = DateTime.UtcNow;

						var module = GetDomModule(request.ModuleId);
						var instances = request.Objects.Clone();

						var @event = new DomInstancesChangedEventMessage(-1, request.ModuleId);

						foreach (var obj in instances)
						{
							if (!module.Instances.ContainsKey(obj.ID))
							{
								@event.Created.Add(obj);
								((ITrackCreatedAt)obj).CreatedAt = utcNow;
								((ITrackCreatedBy)obj).CreatedBy = "DomSLNetMessageHandler";
							}
							else
							{
								@event.Updated.Add(obj);
							}

							((ITrackLastModified)obj).LastModified = utcNow;
							((ITrackLastModifiedBy)obj).LastModifiedBy = "DomSLNetMessageHandler";

							module.TrySetNameOnDomInstance(obj);
							module.Instances[obj.ID] = obj;
						}

						OnInstancesChanged?.Invoke(this, @event);

						var traceData = instances.ToDictionary(x => x.ID, x => new TraceData());
						var unsuccessfulIds = new List<DomInstanceId>();
						var result = new BulkCreateOrUpdateResult<DomInstance, DomInstanceId>(instances, unsuccessfulIds, traceData);

						response = new ManagerStoreCrudResponse<DomInstance>(result);
						return true;
					}

				case ManagerStoreBulkDeleteRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var instances = request.Objects.Clone();

						var successfulIds = new List<DomInstanceId>();
						var unsuccessfulIds = new List<DomInstanceId>();
						var @event = new DomInstancesChangedEventMessage(-1, request.ModuleId);

						foreach (var instance in instances)
						{
							var instanceId = instance.ID;

							if (module.Instances.TryRemove(instanceId, out var removed))
							{
								successfulIds.Add(instanceId);
								@event.Deleted.Add(removed);
							}
							else
							{
								unsuccessfulIds.Add(instanceId);
							}
						}

						OnInstancesChanged?.Invoke(this, @event);

						var traceData = instances.ToDictionary(x => x.ID, x => new TraceData());
						var result = new BulkDeleteResult<DomInstanceId>(successfulIds, unsuccessfulIds, traceData);

						response = new ManagerStoreCrudResponse<DomInstance>(result);
						return true;
					}

				case ManagerStoreStartPagingRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						var instances = request.Filter.ExecuteInMemory(module.Instances.Values).ToList();
						var pagingHandler = new DomPagingHandler<DomInstance>(instances);
						module.PagingHandlers.TryAdd(pagingHandler.Cookie, pagingHandler);

						var nextPage = pagingHandler.GetNextPage(request.PreferredPageSize, out var isLast);

						if (isLast)
						{
							module.PagingHandlers.TryRemove(pagingHandler.Cookie, out pagingHandler);
							pagingHandler.Dispose();
						}

						response = new ManagerStorePagingResponse<DomInstance>(nextPage, isLast, pagingHandler.Cookie);
						return true;
					}

				case ManagerStoreNextPagingRequest<DomInstance> request:
					{
						var module = GetDomModule(request.ModuleId);
						if (!module.PagingHandlers.TryGetValue(request.PagingCookie, out var pagingHandler))
						{
							throw new InvalidOperationException($"Invalid paging cookie: {request.PagingCookie}");
						}

						var nextPage = pagingHandler.GetNextPage(request.PreferredPageSize, out var isLast);

						if (isLast)
						{
							module.PagingHandlers.TryRemove(pagingHandler.Cookie, out pagingHandler);
							pagingHandler.Dispose();
						}

						response = new ManagerStorePagingResponse<DomInstance>(nextPage, isLast, pagingHandler.Cookie);
						return true;
					}

				#endregion

				#region BehaviorDefinitions

				case ManagerStoreReadRequest<DomBehaviorDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var behaviorDefinitions = request.Query.ExecuteInMemory(module.BehaviorDefinitions.Values).ToList();
						response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(behaviorDefinitions);
						return true;
					}

				case ManagerStoreCreateRequest<DomBehaviorDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackCreatedAt)request.Object).CreatedAt = utcNow;
						((ITrackCreatedBy)request.Object).CreatedBy = "DomSLNetMessageHandler";
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.BehaviorDefinitions[request.Object.ID] = request.Object;
						response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(request.Object);
						return true;
					}

				case ManagerStoreUpdateRequest<DomBehaviorDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.BehaviorDefinitions[request.Object.ID] = request.Object;
						response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(request.Object);
						return true;
					}

				case ManagerStoreDeleteRequest<DomBehaviorDefinition> request:
					{
						var module = GetDomModule(request.ModuleId);
						module.BehaviorDefinitions.TryRemove(request.Object.ID, out _);
						response = new ManagerStoreCrudResponse<DomBehaviorDefinition>(request.Object);
						return true;
					}

				#endregion

				#region Status Transition

				case DomInstanceStatusTransitionRequestMessage request:
					response = HandleDomInstanceStatusTransitionRequestMessage(request);
					return true;

				#endregion

				#region Module Settings

				case ManagerStoreReadRequest<ModuleSettings> request:
					{
						var instances = request.Query.ExecuteInMemory(_domModules.Values.SelectNonNull(x => x.Settings)).ToList();
						response = new ManagerStoreCrudResponse<ModuleSettings>(instances);
						return true;
					}

				case ManagerStoreCreateRequest<ModuleSettings> request:
					{
						var module = GetDomModule(request.Object.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackCreatedAt)request.Object).CreatedAt = utcNow;
						((ITrackCreatedBy)request.Object).CreatedBy = "DomSLNetMessageHandler";
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.Settings = request.Object;
						response = new ManagerStoreCrudResponse<ModuleSettings>(request.Object);
						return true;
					}

				case ManagerStoreUpdateRequest<ModuleSettings> request:
					{
						var module = GetDomModule(request.Object.ModuleId);
						var utcNow = DateTime.UtcNow;
						((ITrackLastModified)request.Object).LastModified = utcNow;
						((ITrackLastModifiedBy)request.Object).LastModifiedBy = "DomSLNetMessageHandler";
						module.Settings = request.Object;
						response = new ManagerStoreCrudResponse<ModuleSettings>(request.Object);
						return true;
					}

				case ManagerStoreDeleteRequest<ModuleSettings> request:
					{
						var module = GetDomModule(request.Object.ModuleId);
						module.Settings = null;
						response = new ManagerStoreCrudResponse<ModuleSettings>(request.Object);
						return true;
					}

				case ManagerStoreCountRequest<ModuleSettings> request:
					{
						var count = request.Query.ExecuteInMemory(_domModules.Values.SelectNonNull(x => x.Settings)).LongCount();
						response = new ManagerStoreCountResponse<ModuleSettings>(count);
						return true;
					}

				#endregion

				default:
					response = default;
					return false;
			}
		}

		private DMSMessage HandleDomInstanceStatusTransitionRequestMessage(DomInstanceStatusTransitionRequestMessage request)
		{
			var module = GetDomModule(request.ModuleId);

			if (!module.Instances.TryGetValue(request.DomInstanceId, out var instance))
			{
				throw new InvalidOperationException($"Couldn't find instance with ID '{request.DomInstanceId.Id}'");
			}

			if (!module.Definitions.TryGetValue(instance.DomDefinitionId, out var definition))
			{
				throw new InvalidOperationException($"Couldn't find definition with ID '{instance.DomDefinitionId.Id}'");
			}

			if (!module.BehaviorDefinitions.TryGetValue(definition.DomBehaviorDefinitionId, out var behavior))
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
				throw new InvalidOperationException($"Instance doesn't have status '{transition.FromStatusId}', but '{instance.StatusId}'");
			}

			var utcNow = DateTime.UtcNow;
			((ITrackLastModified)instance).LastModified = utcNow;
			((ITrackLastModifiedBy)instance).LastModifiedBy = "DomSLNetMessageHandler";
			instance.StatusId = transition.ToStatusId;

			return new DomInstanceStatusTransitionResponseMessage { DomInstance = instance };
		}

		private DomModule GetDomModule(string moduleId)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			return _domModules.GetOrAdd(moduleId, x => new DomModule(x));
		}
	}
}
