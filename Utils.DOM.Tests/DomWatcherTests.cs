namespace Skyline.DataMiner.Utils.DOM.Tests
{
	using System;
	using System.Collections.Generic;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Utils.DOM;
	using Skyline.DataMiner.Utils.DOM.UnitTesting;

	[TestClass]
	public class DomWatcherTests
	{
		[TestMethod]
		public void DomWatcher_Create()
		{
			// Arrange
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");

			// filter matching only TestData.Instance1
			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(TestData.Definition1.ID.Id),
				DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id));

			using var domWatcher = new DomWatcher("module", filter, connection);

			var receivedEvents = new List<DomInstancesChangedEventMessage>();
			domWatcher.OnChanged += (s, e) =>
			{
				receivedEvents.Add(e);
			};

			// Act
			domHelper.DomInstances.Create(TestData.Instance1); // matches filter
			domHelper.DomInstances.Create(TestData.Instance2); // does not match filter

			// Assert
			Assert.AreEqual(1, receivedEvents.Count);

			var receivedEvent = receivedEvents[0];
			Assert.AreEqual("module", receivedEvent.ModuleId);
			CollectionAssert.AreEquivalent(new[] { TestData.Instance1 }, receivedEvent.Created);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Updated);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Deleted);
		}

		[TestMethod]
		public void DomWatcher_Update()
		{
			// Arrange
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");
			domHelper.DomInstances.CreateOrUpdate([TestData.Instance1, TestData.Instance2]);

			// filter matching only TestData.Instance1
			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(TestData.Definition1.ID.Id),
				DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id));

			using var domWatcher = new DomWatcher("module", filter, connection);

			var receivedEvents = new List<DomInstancesChangedEventMessage>();
			domWatcher.OnChanged += (s, e) =>
			{
				receivedEvents.Add(e);
			};

			// Act
			domHelper.DomInstances.Update(TestData.Instance1); // matches filter
			domHelper.DomInstances.Update(TestData.Instance2); // does not match filter

			// Assert
			Assert.AreEqual(1, receivedEvents.Count);

			var receivedEvent = receivedEvents[0];
			Assert.AreEqual("module", receivedEvent.ModuleId);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Created);
			CollectionAssert.AreEquivalent(new[] { TestData.Instance1 }, receivedEvent.Updated);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Deleted);
		}

		[TestMethod]
		public void DomWatcher_Delete()
		{
			// Arrange
			var connection = new DomConnectionMock();
			var domHelper = new DomHelper(connection.HandleMessages, "module");
			domHelper.DomInstances.CreateOrUpdate([TestData.Instance1, TestData.Instance2]);

			// filter matching only TestData.Instance1
			var filter = new ANDFilterElement<DomInstance>(
				DomInstanceExposers.DomDefinitionId.Equal(TestData.Definition1.ID.Id),
				DomInstanceExposers.Id.Equal(TestData.Instance1.ID.Id));

			using var domWatcher = new DomWatcher("module", filter, connection);

			var receivedEvents = new List<DomInstancesChangedEventMessage>();
			domWatcher.OnChanged += (s, e) =>
			{
				receivedEvents.Add(e);
			};

			// Act
			domHelper.DomInstances.Delete(TestData.Instance1); // matches filter
			domHelper.DomInstances.Delete(TestData.Instance2); // does not match filter

			// Assert
			Assert.AreEqual(1, receivedEvents.Count);

			var receivedEvent = receivedEvents[0];
			Assert.AreEqual("module", receivedEvent.ModuleId);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Created);
			CollectionAssert.AreEquivalent(Array.Empty<DomInstance>(), receivedEvent.Updated);
			CollectionAssert.AreEquivalent(new[] { TestData.Instance1 }, receivedEvent.Deleted);
		}

		[TestMethod]
		public void DomWatcher_Cleanup()
		{
			// Arrange
			var connection = new DomConnectionMock();

			var filter = new TRUEFilterElement<DomInstance>();
			using var domWatcher = new DomWatcher("module", filter, connection);

			EventHandler<DomInstancesChangedEventMessage> handler = (s, e) => throw new NotImplementedException();
			domWatcher.OnChanged += handler;

			Assert.IsTrue(domWatcher.HasOnChangedSubscribers);
			Assert.IsTrue(connection.HasOnNewMessageSubscribers);

			// Act
			domWatcher.OnChanged -= handler;

			// Assert
			Assert.IsFalse(domWatcher.HasOnChangedSubscribers);
			Assert.IsFalse(connection.HasOnNewMessageSubscribers);
		}
	}
}
