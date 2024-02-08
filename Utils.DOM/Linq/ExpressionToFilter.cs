namespace Skyline.DataMiner.Utils.DOM.Linq
{
	using System;
	using System.Linq.Expressions;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	internal static class ExpressionToFilter
	{
		public static FilterElement<DomInstance> Convert(Expression expression)
		{
			switch (expression)
			{
				case BinaryExpression binaryExpression:
					return BinaryExpression(binaryExpression);

				case ConstantExpression constantExpression:
					return ConstantExpression(constantExpression);

				default:
					throw new NotSupportedException($"Expression is not supported: {expression}");
			}
		}

		private static FilterElement<DomInstance> ConstantExpression(ConstantExpression expression)
		{
			switch (expression.Value)
			{
				case true:
					return new TRUEFilterElement<DomInstance>();

				case false:
					return new FALSEFilterElement<DomInstance>();

				default:
					throw new NotSupportedException($"Expression is not supported: {expression}");
			}
		}

		private static FilterElement<DomInstance> BinaryExpression(BinaryExpression expression)
		{
			if (expression.NodeType == ExpressionType.OrElse)
			{
				return new ORFilterElement<DomInstance>(Convert(expression.Left), Convert(expression.Right));
			}
			else if (expression.NodeType == ExpressionType.AndAlso)
			{
				return new ANDFilterElement<DomInstance>(Convert(expression.Left), Convert(expression.Right));
			}

			var left = expression.Left as MemberExpression;
			var right = expression.Right;

			if (left.Member.DeclaringType == typeof(DomInstance))
			{
				switch (left.Member.Name)
				{
					case nameof(DomInstance.ID):
						var lambda = Expression.Lambda<Func<DomInstanceId>>(right).Compile();
						return DomInstanceExposers.Id.Equal(lambda());
				}
			}

			throw new NotSupportedException($"Expression is not supported: {expression}");
		}
	}
}
