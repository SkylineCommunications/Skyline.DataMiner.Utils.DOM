namespace Utils.Dom.Tests
{
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM;
	using Skyline.DataMiner.Utils.DOM.Builders;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class SectionExtensionTests
	{
		[TestMethod]
		public void Section_GetFieldValue()
		{
			var cache = new DomCache(TestData.DomHelper);

			var section = TestData.Instance1.Sections.First();

			var value = section.GetFieldValue<int>("Field 2", cache);

			value.Should().Be(123);
		}

		[TestMethod]
		public void Section_SetFieldValue()
		{
			var cache = new DomCache(TestData.DomHelper);

			var section = TestData.Instance1.Sections.First();

			// clone the section to avoid interference with other tests
			section = section.Clone() as Section;

			section.SetFieldValue("Field 2", 666, cache);

			var value = section.GetFieldValue<int>("Field 2", cache);
			value.Should().Be(666);
		}
	}
}
