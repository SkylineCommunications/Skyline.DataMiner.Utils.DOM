namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Concurrent;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.Modules;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.Sections;

	internal class DomModule
	{
		public DomModule(string moduleId)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			ModuleId = moduleId;
		}

		public string ModuleId { get; }

		public ModuleSettings Settings { get; set; }

		public ConcurrentDictionary<DomDefinitionId, DomDefinition> Definitions { get; } = new ConcurrentDictionary<DomDefinitionId, DomDefinition>();
		public ConcurrentDictionary<SectionDefinitionID, SectionDefinition> SectionDefinitions { get; } = new ConcurrentDictionary<SectionDefinitionID, SectionDefinition>();
		public ConcurrentDictionary<DomInstanceId, DomInstance> Instances { get; } = new ConcurrentDictionary<DomInstanceId, DomInstance>();
		public ConcurrentDictionary<DomBehaviorDefinitionId, DomBehaviorDefinition> BehaviorDefinitions { get; } = new ConcurrentDictionary<DomBehaviorDefinitionId, DomBehaviorDefinition>();
		public ConcurrentDictionary<PagingCookie, DomPagingHandler<DomInstance>> PagingHandlers { get; } = new ConcurrentDictionary<PagingCookie, DomPagingHandler<DomInstance>>();

		public void TrySetNameOnDomInstance(DomInstance instance)
		{
			if (instance == null)
			{
				return;
			}

			var nameDefinition = Settings?.DomManagerSettings?.DomInstanceNameDefinition;

			if (Definitions.TryGetValue(instance.DomDefinitionId, out var definition) &&
				definition.ModuleSettingsOverrides?.NameDefinition != null)
			{
				nameDefinition = definition.ModuleSettingsOverrides.NameDefinition;
			}

			nameDefinition?.SetNameOnDomInstance(instance);
		}

		public void TrySetInitialStatusOnDomInstance(DomInstance instance)
		{
			if (instance is null || !String.IsNullOrEmpty(instance.StatusId) || instance.DomDefinitionId is null)
			{
				return;
			}

			if (!Definitions.TryGetValue(instance.DomDefinitionId, out var definition) || definition.DomBehaviorDefinitionId is null)
			{
				return;
			}

			if (BehaviorDefinitions.TryGetValue(definition.DomBehaviorDefinitionId, out var behavior))
			{
				instance.StatusId = behavior.InitialStatusId;
			}
		}
	}
}
