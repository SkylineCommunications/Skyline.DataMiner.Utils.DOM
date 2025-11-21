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
		/// <param name="moduleId">ID of the DOM module.</param>
		private DomHelperMock(DomSLNetMessageHandler messageHandler, string moduleId) : base(messageHandler.HandleMessages, moduleId)
		{
			_messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomHelperMock"/>.
		/// </summary>
		/// <returns>A new instance of <see cref="DomHelperMock"/>.</returns>
		public static DomHelperMock Create()
		{
			return Create("module");
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomHelperMock"/>.
		/// </summary>
		/// <param name="moduleId">ID of the DOM module.</param>
		/// <returns>A new instance of <see cref="DomHelperMock"/>.</returns>
		public static DomHelperMock Create(string moduleId)
		{
			var messageHandler = new DomSLNetMessageHandler();
			return new DomHelperMock(messageHandler, moduleId);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomHelperMock"/>.
		/// </summary>
		/// <param name="moduleId">ID of the DOM module.</param>
		/// <param name="validateAgainstDefinition">If set to <c>true</c>, validates DOM instances against their definitions, section definitions, and required fields when performing CRUD operations.</param>
		/// <returns>A new instance of <see cref="DomHelperMock"/>.</returns>
		public static DomHelperMock Create(string moduleId, bool validateAgainstDefinition)
		{
			var messageHandler = new DomSLNetMessageHandler(validateAgainstDefinition);
			return new DomHelperMock(messageHandler, moduleId);
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
		/// Creates a new instance of <see cref="DomHelperMock"/> with pre-defined instances.
		/// </summary>
		/// <param name="moduleId">The ID of the DOM module.</param>
		/// <param name="instances">The pre-defined collection of DOM instances.</param>
		/// <returns>A new instance of <see cref="DomHelperMock"/> with pre-defined instances.</returns>
		public static DomHelperMock Create(string moduleId, IEnumerable<DomInstance> instances)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var mock = Create(moduleId);
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

			_messageHandler.SetDefinitions(ModuleId, definitions);
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

			_messageHandler.SetSectionDefinitions(ModuleId, sectionDefinitions);
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

			_messageHandler.SetInstances(ModuleId, instances);
		}

		/// <summary>
		/// Sets the behavior definitions in the mock.
		/// </summary>
		/// <param name="definitions">The collection of DOM behavior definitions to set.</param>
		public void SetBehaviorDefinitions(IEnumerable<DomBehaviorDefinition> definitions)
		{
			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			_messageHandler.SetBehaviorDefinitions(ModuleId, definitions);
		}
	}
}
