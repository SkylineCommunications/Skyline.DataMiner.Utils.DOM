namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using Skyline.DataMiner.Net.Sections;

	public static class ValueWrapperExtensions
	{
		public static T GetValue<T>(this ValueWrapper<T> valueWrapper)
		{
			// valueWrapper is allowed to be null!
			return valueWrapper != null ? valueWrapper.Value : default;
		}
	}
}
