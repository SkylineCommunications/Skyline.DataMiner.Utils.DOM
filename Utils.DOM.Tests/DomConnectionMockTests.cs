namespace Skyline.DataMiner.Utils.DOM.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.ManagerStore;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.SubscriptionFilters;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	[TestClass]
	public class DomConnectionMockTests
	{
		[TestMethod]
		public void DomConnectionMock_Subscription_Create()
		{
			// arrange
			var subscriptionSet = "my subscription set";
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");

			var receivedEvents = new List<DomInstancesChangedEventMessage>();
			connection.OnNewMessage += (s, e) =>
			{
				if (e.FromSet(subscriptionSet) &&
					e.Message is DomInstancesChangedEventMessage domInstancesChangedEvent)
				{
					receivedEvents.Add(domInstancesChangedEvent);
				}
			};

			// filter matching only TestData.Instance1
			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(TestData.Definition1.ID.Id),
				DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id));

			var subscriptionFilters = new SubscriptionFilter[]
			{
				new ModuleEventSubscriptionFilter<DomInstancesChangedEventMessage>("module"),
				new SubscriptionFilter<DomInstancesChangedEventMessage, DomInstance>(filter),
			};
			connection.AddSubscription(subscriptionSet, subscriptionFilters);

			// act
			domHelper.DomInstances.Create(TestData.Instance1); // matches filter
			domHelper.DomInstances.Create(TestData.Instance2); // does not match filter

			// assert
			Assert.AreEqual(1, receivedEvents.Count);

			var receivedEvent = receivedEvents[0];
			Assert.AreEqual("module", receivedEvent.ModuleId);
			CollectionAssert.AreEquivalent(new[] { TestData.Instance1 }, receivedEvent.Created);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Updated);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Deleted);
		}

		[TestMethod]
		public void DomConnectionMock_Subscription_Update()
		{
			// arrange
			var subscriptionSet = "my subscription set";
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");
			domHelper.DomInstances.CreateOrUpdate(new[] { TestData.Instance1, TestData.Instance2 }.ToList());

			var receivedEvents = new List<DomInstancesChangedEventMessage>();
			connection.OnNewMessage += (s, e) =>
			{
				if (e.FromSet(subscriptionSet) &&
					e.Message is DomInstancesChangedEventMessage domInstancesChangedEvent)
				{
					receivedEvents.Add(domInstancesChangedEvent);
				}
			};

			// filter matching only TestData.Instance1
			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(TestData.Definition1.ID.Id),
				DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id));

			var subscriptionFilters = new SubscriptionFilter[]
			{
				new ModuleEventSubscriptionFilter<DomInstancesChangedEventMessage>("module"),
				new SubscriptionFilter<DomInstancesChangedEventMessage, DomInstance>(filter),
			};
			connection.AddSubscription(subscriptionSet, subscriptionFilters);

			// act
			domHelper.DomInstances.Update(TestData.Instance1); // matches filter
			domHelper.DomInstances.Update(TestData.Instance2); // does not match filter

			// assert
			Assert.AreEqual(1, receivedEvents.Count);

			var receivedEvent = receivedEvents[0];
			Assert.AreEqual("module", receivedEvent.ModuleId);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Created);
			CollectionAssert.AreEquivalent(new[] { TestData.Instance1 }, receivedEvent.Updated);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Deleted);
		}

		[TestMethod]
		public void DomConnectionMock_Subscription_Delete()
		{
			// arrange
			var subscriptionSet = "my subscription set";
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");
			domHelper.DomInstances.CreateOrUpdate(new[] { TestData.Instance1, TestData.Instance2 }.ToList());

			var receivedEvents = new List<DomInstancesChangedEventMessage>();
			connection.OnNewMessage += (s, e) =>
			{
				if (e.FromSet(subscriptionSet) &&
					e.Message is DomInstancesChangedEventMessage domInstancesChangedEvent)
				{
					receivedEvents.Add(domInstancesChangedEvent);
				}
			};

			// filter matching only TestData.Instance1
			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(TestData.Definition1.ID.Id),
				DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id));

			var subscriptionFilters = new SubscriptionFilter[]
			{
				new ModuleEventSubscriptionFilter<DomInstancesChangedEventMessage>("module"),
				new SubscriptionFilter<DomInstancesChangedEventMessage, DomInstance>(filter),
			};
			connection.AddSubscription(subscriptionSet, subscriptionFilters);

			// act
			domHelper.DomInstances.Delete(TestData.Instance1); // matches filter
			domHelper.DomInstances.Delete(TestData.Instance2); // does not match filter

			// assert
			Assert.AreEqual(1, receivedEvents.Count);

			var receivedEvent = receivedEvents[0];
			Assert.AreEqual("module", receivedEvent.ModuleId);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Created);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Updated);
			CollectionAssert.AreEquivalent(new[] { TestData.Instance1 }, receivedEvent.Deleted);
		}

		[TestMethod]
		public void DomConnectionMock_ITrackProperties()
		{
			// arrange
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");

			// act
			domHelper.DomInstances.CreateOrUpdate(new[] { TestData.Instance1, TestData.Instance2 }.ToList());
			var result = domHelper.DomInstances.ReadAll();

			// assert
			result.Should().HaveCount(2);
			result[0].As<ITrackCreatedAt>().CreatedAt.Should().NotBe(default);
			result[0].As<ITrackCreatedBy>().CreatedBy.Should().NotBeNullOrEmpty();
			result[0].As<ITrackLastModified>().LastModified.Should().NotBe(default);
			result[0].As<ITrackLastModifiedBy>().LastModifiedBy.Should().NotBeNullOrEmpty();
		}
	}
}
