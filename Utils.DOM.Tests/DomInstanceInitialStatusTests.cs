namespace Skyline.DataMiner.Utils.DOM.Tests
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Utils.DOM.Builders;
	using Skyline.DataMiner.Utils.DOM.Extensions;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	[TestClass]
	public class DomInstanceInitialStatusTests
	{
		private static readonly DomBehaviorDefinition Behavior = new DomBehaviorDefinitionBuilder()
			.WithID(new DomBehaviorDefinitionId(Guid.Parse("0f2c2d3a-6d2b-4b58-9b1a-9f7d0e6f1a01")))
			.WithName("Behavior 1")
			.WithInitialStatusId("draft")
			.Build();

		private static readonly DomDefinition Definition = new DomDefinitionBuilder()
			.WithID(Guid.Parse("a4f0a1b2-3c4d-4e5f-8a9b-0c1d2e3f4a5b"))
			.WithName("Definition with behavior")
			.WithDomBehaviorDefinition(Behavior)
			.Build();

		[TestMethod]
		public void Create_WithoutStatus_AppliesInitialStatusFromBehavior()
		{
			// arrange
			var helper = DomHelperMock.Create();
			helper.SetDefinitions(new[] { Definition });
			helper.SetBehaviorDefinitions(new[] { Behavior });

			var instance = new DomInstanceBuilder()
				.WithDefinition(Definition)
				.WithID(Guid.NewGuid())
				.Build();

			instance.StatusId.Should().BeNullOrEmpty();

			// act
			var created = helper.DomInstances.Create(instance);

			// assert
			created.StatusId.Should().Be("draft");
			helper.DomInstances.ReadAll(Definition)
				.Single().StatusId.Should().Be("draft");
		}

		[TestMethod]
		public void Create_WithExistingStatus_DoesNotOverrideStatus()
		{
			// arrange
			var helper = DomHelperMock.Create();
			helper.SetDefinitions(new[] { Definition });
			helper.SetBehaviorDefinitions(new[] { Behavior });

			var instance = new DomInstanceBuilder()
				.WithDefinition(Definition)
				.WithID(Guid.NewGuid())
				.WithStatusID("confirmed")
				.Build();

			// act
			var created = helper.DomInstances.Create(instance);

			// assert
			created.StatusId.Should().Be("confirmed");
		}

		[TestMethod]
		public void BulkCreateOrUpdate_WithoutStatus_AppliesInitialStatusFromBehavior()
		{
			// arrange
			var helper = DomHelperMock.Create();
			helper.SetDefinitions(new[] { Definition });
			helper.SetBehaviorDefinitions(new[] { Behavior });

			var instance1 = new DomInstanceBuilder()
				.WithDefinition(Definition)
				.WithID(Guid.NewGuid())
				.Build();

			var instance2 = new DomInstanceBuilder()
				.WithDefinition(Definition)
				.WithID(Guid.NewGuid())
				.Build();

			// act
			helper.DomInstances.CreateOrUpdate(new[] { instance1, instance2 }.ToList());

			// assert
			var stored = helper.DomInstances.ReadAll(Definition);
			stored.Should().OnlyContain(i => i.StatusId == "draft");
		}
	}
}
