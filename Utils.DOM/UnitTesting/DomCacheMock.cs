namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Sections;

	public class DomCacheMock : DomCache
	{
		private readonly DomHelperMock _helper;

		public DomCacheMock(DomHelperMock helper) : base(helper)
		{
			_helper = helper ?? throw new ArgumentNullException(nameof(helper));
		}

		public static DomCacheMock Create()
		{
			var helper = DomHelperMock.Create();
			return new DomCacheMock(helper);
		}

		public static DomCacheMock Create(IEnumerable<DomInstance> instances)
		{
			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var helper = DomHelperMock.Create(instances);
			return new DomCacheMock(helper);
		}

		public void SetDefinitions(IEnumerable<DomDefinition> definitions)
		{
			if (definitions == null)
			{
				throw new ArgumentNullException(nameof(definitions));
			}

			_helper.SetDefinitions(definitions);
		}

		public void SetSectionDefinitions(IEnumerable<SectionDefinition> sectionDefinitions)
		{
			if (sectionDefinitions == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitions));
			}

			_helper.SetSectionDefinitions(sectionDefinitions);
		}

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
