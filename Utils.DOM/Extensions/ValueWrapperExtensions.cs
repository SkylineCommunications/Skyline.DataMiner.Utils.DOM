namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Contains extension methods for the <see cref="ValueWrapper{T}"/> class.
	/// </summary>
	public static class ValueWrapperExtensions
	{
		/// <summary>
		/// Gets the value from the specified <see cref="ValueWrapper{T}"/>.
		/// </summary>
		/// <typeparam name="T">The type of the value in the wrapper.</typeparam>
		/// <param name="valueWrapper">The value wrapper.</param>
		/// <returns>The value from the wrapper or the default value of the type if the wrapper is null.</returns>
		public static T GetValue<T>(this ValueWrapper<T> valueWrapper)
		{
			// valueWrapper is allowed to be null!
			return valueWrapper != null ? valueWrapper.Value : default;
		}
	}
}
