namespace Utils.DOM.Tests
{
	using System;
	using System.Linq;
	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Skyline.DataMiner.Utils.DOM.Linq;

	[TestClass]
	public class Experiments
	{
		[TestMethod]
		public void Experiment1()
		{
			var helper = TestData.DomHelper;

			var list = helper.DomInstances
				.Where(x => x.ID == TestData.Instance1.ID)
				.ToList();

			list.Should().Equal(TestData.Instance1);
		}

	}
}
