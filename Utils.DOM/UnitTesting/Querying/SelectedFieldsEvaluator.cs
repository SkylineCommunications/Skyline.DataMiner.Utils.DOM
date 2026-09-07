namespace Skyline.DataMiner.Utils.DOM.UnitTesting.Querying
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Select;
	using Skyline.DataMiner.Net.Apps.ManagerStore.Select;

	/// <summary>
	/// Evaluates the selected fields of a select request against in-memory <see cref="DomInstance"/> objects.
	/// </summary>
	/// <remarks>
	/// A real DataMiner Agent only reads the requested columns from the database and returns them as
	/// <see cref="PartialObjectData"/>. This evaluator mirrors that behavior by executing the exposers of the
	/// requested fields on the in-memory instances.
	/// </remarks>
	internal static class SelectedFieldsEvaluator
	{
		private static readonly ConcurrentDictionary<Type, Func<int, object, IPartialObjectValue>> _valueFactories =
			new ConcurrentDictionary<Type, Func<int, object, IPartialObjectValue>>();

		private static readonly ConcurrentDictionary<string, PartialDomInstanceFactory> _objectFactories =
			new ConcurrentDictionary<string, PartialDomInstanceFactory>();

		/// <summary>
		/// Builds the <see cref="SelectResult"/> for the specified instances and selected fields.
		/// </summary>
		/// <param name="moduleId">The ID of the DOM module the instances belong to.</param>
		/// <param name="instances">The instances to extract the selected fields from.</param>
		/// <param name="selectedFields">The fields that were selected in the request.</param>
		/// <returns>The result containing the partial object data for every instance.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="moduleId"/> is null or whitespace.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="instances"/> is null.</exception>
		/// <exception cref="InvalidOperationException">Thrown when an unsupported field was selected.</exception>
		public static SelectResult Apply(string moduleId, IEnumerable<DomInstance> instances, IEnumerable<SelectedFieldReference> selectedFields)
		{
			if (String.IsNullOrWhiteSpace(moduleId))
			{
				throw new ArgumentException($"'{nameof(moduleId)}' cannot be null or whitespace.", nameof(moduleId));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var references = Validate(selectedFields);
			var factory = _objectFactories.GetOrAdd(moduleId, x => new PartialDomInstanceFactory(x));

			var objects = instances
				.Where(instance => instance != null)
				.Select(instance => factory.GetPartialObjectData(instance.ID, GetValues(instance, references)))
				.ToList();

			return new SelectResult { Objects = objects };
		}

		private static List<SelectedFieldReference> Validate(IEnumerable<SelectedFieldReference> selectedFields)
		{
			var references = selectedFields?.ToList() ?? new List<SelectedFieldReference>();

			foreach (var reference in references)
			{
				var exposer = reference?.SerializableExposer?.Exposer;

				if (exposer == null)
				{
					throw new InvalidOperationException("A selected field without an exposer is not supported.");
				}

				if (String.Equals(exposer.fieldName, DomInstanceExposers.FieldValues.fieldName, StringComparison.Ordinal) ||
					String.Equals(exposer.fieldName, DomInstanceExposers.FullObject.fieldName, StringComparison.Ordinal))
				{
					throw new InvalidOperationException($"Selecting the '{exposer.fieldName}' exposer is not supported.");
				}
			}

			return references;
		}

		private static List<IPartialObjectValue> GetValues(DomInstance instance, List<SelectedFieldReference> references)
		{
			var values = new List<IPartialObjectValue>(references.Count);

			foreach (var reference in references)
			{
				if (TryGetValue(instance, reference, out var value))
				{
					values.Add(value);
				}
			}

			return values;
		}

		private static bool TryGetValue(DomInstance instance, SelectedFieldReference reference, out IPartialObjectValue partialObjectValue)
		{
			partialObjectValue = null;

			object value;

			try
			{
				value = reference.SerializableExposer.Exposer.execute(instance);
			}
			catch
			{
				// Mirror the fail-safe behavior of a real DataMiner Agent: a field that cannot be read
				// (e.g. the instance has no value for the requested field descriptor) has no value.
				return false;
			}

			if (value == null)
			{
				return false;
			}

			partialObjectValue = _valueFactories.GetOrAdd(value.GetType(), CreateValueFactory)(reference.Id, value);

			return true;
		}

		private static Func<int, object, IPartialObjectValue> CreateValueFactory(Type valueType)
		{
			var partialObjectValueType = typeof(PartialObjectValue<>).MakeGenericType(valueType);
			var idProperty = partialObjectValueType.GetProperty(nameof(PartialObjectValue<object>.FieldReferenceId));
			var valueProperty = partialObjectValueType.GetProperty(nameof(PartialObjectValue<object>.Value));

			return (id, value) =>
			{
				var partialObjectValue = (IPartialObjectValue)Activator.CreateInstance(partialObjectValueType);
				idProperty.SetValue(partialObjectValue, id, BindingFlags.Default, null, null, null);
				valueProperty.SetValue(partialObjectValue, value, BindingFlags.Default, null, null, null);

				return partialObjectValue;
			};
		}
	}
}
