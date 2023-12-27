namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Represents a mock implementation of the <see cref="DomCache"/> for unit testing purposes.
	/// </summary>
	public class DomCacheMock : DomCache
	{
		private readonly DomHelperMock _helper;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomCacheMock"/> class with a specified <see cref="DomHelperMock"/>.
		/// </summary>
		/// <param name="helper">The mock helper for interaction with the DOM.</param>
		public DomCacheMock(DomHelperMock helper) : base(helper)
		{
			_helper = helper ?? throw new ArgumentNullException(nameof(helper));
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomCacheMock"/>.
		/// </summary>
		/// <returns>A new instance of <see cref="DomCacheMock"/>.</returns>
		public static DomCacheMock Create()
		{
			var helper = DomHelperMock.Create();
			return new DomCacheMock(helper);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DomCacheMock"/> with pre-defined instances.
		/// </summary>
		/// <param name="instances">The pre-defined collection of DOM instances.</param>
		/// <returns>A new instance of <see cref="DomCacheMock"/> with pre-defined instances.</returns>
		public static DomCacheMock Create(IEnumerable<DomInstance> instances)
		{
			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var helper = DomHelperMock.Create(instances);
			return new DomCacheMock(helper);
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

			_helper.SetDefinitions(definitions);
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

			_helper.SetSectionDefinitions(sectionDefinitions);
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

			_helper.SetInstances(instances);
		}
	}
}
