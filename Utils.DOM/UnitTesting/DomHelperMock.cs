namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Represents a mock implementation of the <see cref="DomHelper"/> for unit testing purposes.
	/// </summary>
	public class DomHelperMock : DomHelper
	{
		private readonly DomSLNetMessageHandler _messageHandler;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomHelperMock"/> class with a specified <see cref="DomSLNetMessageHandler"/>.
		/// </summary>
		/// <param name="messageHandler">The mock message handler for processing DOM messages.</param>
		private DomHelperMock(DomSLNetMessageHandler messageHandler) : base(messageHandler.HandleMessages, "module")
		{
			_messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomHelperMock"/>.
		/// </summary>
		/// <returns>A new instance of <see cref="DomHelperMock"/>.</returns>
		public static DomHelperMock Create()
		{
			var messageHandler = new DomSLNetMessageHandler();
			return new DomHelperMock(messageHandler);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomHelperMock"/> with pre-defined instances.
		/// </summary>
		/// <param name="instances">The pre-defined collection of DOM instances.</param>
		/// <returns>A new instance of <see cref="DomHelperMock"/> with pre-defined instances.</returns>
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

		/// <summary>
		/// Sets the definitions in the mock.
		/// </summary>
		/// <param name="definitions">The collection of DOM definitions to set.</param>
		public void SetDefinitions(IEnumerable<DomDefinition> definitions)
		{
			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			_messageHandler.SetDefinitions(definitions);
		}

		/// <summary>
		/// Sets the section definitions in the mock.
		/// </summary>
		/// <param name="sectionDefinitions">The collection of section definitions to set.</param>
		public void SetSectionDefinitions(IEnumerable<SectionDefinition> sectionDefinitions)
		{
			if (sectionDefinitions == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitions));
			}

			_messageHandler.SetSectionDefinitions(sectionDefinitions);
		}

		/// <summary>
		/// Sets the instances in the mock.
		/// </summary>
		/// <param name="instances">The collection of DOM instances to set.</param>
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
