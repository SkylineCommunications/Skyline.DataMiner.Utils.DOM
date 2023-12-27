namespace Utils.DOM.Tests.Extensions
{
	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using Skyline.DataMiner.Net.Sections;
	using Skyline.DataMiner.Utils.DOM.Extensions;

	[TestClass]
	public class ValueWrapperExtensionTests
	{
		[TestMethod]
		public void ValueWrapper_WithValue()
		{
			var valueWrapper = new ValueWrapper<string>("Hello world!");

			valueWrapper.GetValue().Should().Be("Hello world!");
		}

		[TestMethod]
		public void ValueWrapper_WithNull()
		{
			ValueWrapper<string> valueWrapper = null;

			valueWrapper.GetValue().Should().BeNull();
		}
	}
}
