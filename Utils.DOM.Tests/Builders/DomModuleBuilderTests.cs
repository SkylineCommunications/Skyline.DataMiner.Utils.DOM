namespace Skyline.DataMiner.Utils.DOM.Tests.Builders
{
	using System;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Utils.DOM.Builders;

	[TestClass]
	public class DomModuleBuilderTests
	{
		[TestMethod]
		public void DomModuleBuilder_WithModuleId()
		{
			var id1 = "(slc)resource_studio";
			var id2 = "MyIncorrectModule";

			var module1 = new DomModuleBuilder()
				.WithModuleId(id1)
				.Build();

			var module2 = () => new DomModuleBuilder()
				.WithModuleId(id2)
				.Build();

			module1.ModuleId.Should().Be(id1);
			module2.Should().Throw<ArgumentException>();
		}

		[TestMethod]
		public void DomModuleBuilder_WithInformationEvents()
		{
			var module = new DomModuleBuilder()
				.WithInformationEvents(true)
				.Build();

			module.DomManagerSettings.InformationEventSettings.Enable.Should().Be(true);
		}

		[TestMethod]
		public void DomModuleBuilder_WithoutHistory()
		{
			var module = new DomModuleBuilder()
				.WithHistory(false)
				.Build();

			module.DomManagerSettings.DomInstanceHistorySettings.StorageBehavior.Should().Be(Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Settings.DomInstanceHistoryStorageBehavior.Disabled);
		}

		[TestMethod]
		public void DomModuleBuilder_WithSynchronousHistory()
		{
			var module = new DomModuleBuilder()
				.WithHistoryBehavior(Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Settings.DomInstanceHistoryStorageBehavior.EnabledSync)
				.Build();

			module.DomManagerSettings.DomInstanceHistorySettings.StorageBehavior.Should().Be(Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Settings.DomInstanceHistoryStorageBehavior.EnabledSync);
		}

		[TestMethod]
		public void DomModuleBuilder_WithInstanceLifetime()
		{
			var ttl = TimeSpan.FromDays(5);
			var module = new DomModuleBuilder()
				.WithInstanceTTL(ttl)
				.Build();

			module.DomManagerSettings.TtlSettings.DomInstanceTtl.Should().Be(ttl);
		}

		[TestMethod]
		public void DomModuleBuilder_WithHistoryLifetime()
		{
			var ttl = TimeSpan.FromDays(5);
			var module = new DomModuleBuilder()
				.WithHistoryTTL(ttl)
				.Build();

			module.DomManagerSettings.TtlSettings.DomInstanceHistoryTtl.Should().Be(ttl);
		}

		[TestMethod]
		public void DomModuleBuilder_WithTemplateLifetime()
		{
			var ttl = TimeSpan.FromDays(5);
			var module = new DomModuleBuilder()
				.WithTemplateTTL(ttl)
				.Build();

			module.DomManagerSettings.TtlSettings.DomTemplateTtl.Should().Be(ttl);
		}

		[TestMethod]
		public void DomModuleBuilder_WithCreateCrudScript()
		{
			var scriptName = "my_script";
			var module = new DomModuleBuilder()
				.WithOnCreateScript(scriptName)
				.Build();

			module.DomManagerSettings.ScriptSettings.OnCreate.Should().Be(scriptName);
		}

		[TestMethod]
		public void DomModuleBuilder_WithUpdateCrudScript()
		{
			var scriptName = "my_script";
			var module = new DomModuleBuilder()
				.WithOnUpdateScript(scriptName)
				.Build();

			module.DomManagerSettings.ScriptSettings.OnUpdate.Should().Be(scriptName);
		}

		[TestMethod]
		public void DomModuleBuilder_WithDeleteCrudScript()
		{
			var scriptName = "my_script";
			var module = new DomModuleBuilder()
				.WithOnDeleteScript(scriptName)
				.Build();

			module.DomManagerSettings.ScriptSettings.OnDelete.Should().Be(scriptName);
		}

		[TestMethod]
		public void DomModuleBuilder_WithCrudType()
		{
			var crudType = Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Settings.OnDomInstanceActionScriptType.FullCrudMeta;
			var module = new DomModuleBuilder()
				.WithCrudType(crudType)
				.Build();

			module.DomManagerSettings.ScriptSettings.ScriptType.Should().Be(crudType);
		}
	}
}
