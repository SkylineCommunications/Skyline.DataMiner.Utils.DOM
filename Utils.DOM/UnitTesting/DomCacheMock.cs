namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System.Collections.Generic;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;

	public class DomCacheMock : DomCache
	{
		public DomCacheMock(ICollection<DomInstance> instances) : base(new DomHelperMock(instances))
		{
		}
	}
}
