namespace Skyline.DataMiner.Utils.DOM.Linq
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;

	public static class Extensions
	{
		public static ICollection<DomInstance> Where(
			this DomInstanceCrudHelperComponent source,
			Expression<Func<DomInstance, bool>> expression)
		{
			var filter = ExpressionToFilter.Convert(expression.Body);

			return source.Read(filter);
		}
	}
}
