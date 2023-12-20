namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;

	public class DomHelperMock : DomHelper
	{
		private readonly MockMessageHandler _messageHandler;

		private DomHelperMock(MockMessageHandler messageHandler) : base(messageHandler.HandleMessages, "module")
		{
			_messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
		}

		public static DomHelperMock Create()
		{
			var messageHandler = new MockMessageHandler();
			return new DomHelperMock(messageHandler);
		}

		public static DomHelperMock Create(IEnumerable<DomInstance> instances)
		{
			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var mock = Create();
			mock.SetInstances(instances);

			return mock;
		}

		public void SetDefinitions(IEnumerable<DomDefinition> definitions)
		{
			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			_messageHandler.SetDefinitions(definitions);
		}

		public void SetSectionDefinitions(IEnumerable<SectionDefinition> sectionDefinitions)
		{
			if (sectionDefinitions == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitions));
			}

			_messageHandler.SetSectionDefinitions(sectionDefinitions);
		}

		public void SetInstances(IEnumerable<DomInstance> instances)
		{
			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			_messageHandler.SetInstances(instances);
		}
	}
}
