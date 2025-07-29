// Ignore Spelling: tooltip utils

namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.GenericEnums;
	using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Generic builder class for creating instances of the <see cref="FieldDescriptor"/> class.
	/// </summary>
	/// <typeparam name="T">The type of the builder class.</typeparam>
	/// <typeparam name="TFieldDescriptor">The type of field descriptor.</typeparam>
	public abstract class FieldDescriptorBuilder<T, TFieldDescriptor>
		where T : FieldDescriptorBuilder<T, TFieldDescriptor>
		where TFieldDescriptor : FieldDescriptor, new()
	{
		/// <summary>
		/// The <see cref="FieldDescriptor"/> instance being built by the builder.
		/// </summary>
		protected TFieldDescriptor _fieldDescriptor;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDescriptorBuilder{T, TFieldDescriptor}"/> class with a specified field descriptor.
		/// </summary>
		/// <param name="fieldDescriptor">The existing field descriptor to be used in the builder.</param>
		protected FieldDescriptorBuilder(TFieldDescriptor fieldDescriptor)
		{
			_fieldDescriptor = fieldDescriptor ?? throw new ArgumentNullException(nameof(fieldDescriptor));
		}

		/// <summary>
		/// Builds the field descriptor.
		/// </summary>
		/// <returns>The constructed <see cref="FieldDescriptor"/>.</returns>
		public TFieldDescriptor Build()
		{
			return _fieldDescriptor;
		}

		/// <summary>
		/// Sets the ID for the field descriptor.
		/// </summary>
		/// <param name="id">The ID to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(FieldDescriptorID id)
		{
			_fieldDescriptor.ID = id ?? throw new ArgumentNullException(nameof(id));

			return (T)this;
		}

		/// <summary>
		/// Sets the ID for the field descriptor using a GUID.
		/// </summary>
		/// <param name="id">The GUID to set as the ID.</param>
		/// <returns>The builder instance.</returns>
		public T WithID(Guid id)
		{
			return WithID(new FieldDescriptorID(id));
		}

		/// <summary>
		/// Sets the name for the field descriptor.
		/// </summary>
		/// <param name="name">The name to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			_fieldDescriptor.Name = name;

			return (T)this;
		}

		/// <summary>
		/// Sets the type for the field descriptor.
		/// </summary>
		/// <param name="type">The type to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithType(Type type)
		{
			_fieldDescriptor.FieldType = type ?? throw new ArgumentNullException(nameof(type));

			return (T)this;
		}

		/// <summary>
		/// Sets the default value for the field descriptor.
		/// </summary>
		/// <param name="value">The default value to set.</param>
		/// <returns>The builder instance.</returns>
		public T WithDefaultValue(IValueWrapper value)
		{
			_fieldDescriptor.DefaultValue = value;

			return (T)this;
		}

		/// <summary>
		/// Sets whether the field descriptor is optional.
		/// </summary>
		/// <param name="isOptional">A boolean indicating whether the field descriptor is optional.</param>
		/// <returns>The builder instance.</returns>
		public T WithIsOptional(bool isOptional)
		{
			_fieldDescriptor.IsOptional = isOptional;

			return (T)this;
		}

		/// <summary>
		/// Sets whether the field descriptor is read-only.
		/// </summary>
		/// <param name="isReadonly">A boolean indicating whether the field descriptor is read-only.</param>
		/// <returns>The builder instance.</returns>
		public T WithReadonly(bool isReadonly)
		{
			_fieldDescriptor.IsReadonly = isReadonly;

			return (T)this;
		}

		/// <summary>
		/// Sets whether the field descriptor is soft deleted.
		/// </summary>
		/// <param name="isSoftDeleted">A boolean indicating whether the field descriptor is soft deleted.</param>
		/// <returns>The builder instance.</returns>
		public T WithSoftDeleted(bool isSoftDeleted)
		{
			_fieldDescriptor.IsSoftDeleted = isSoftDeleted;
			return (T)this;
		}

		/// <summary>
		/// Sets whether the field descriptor is hidden.
		/// </summary>
		/// <param name="isHidden">A boolean indicating whether the field descriptor is hidden.</param>
		/// <returns>The builder instance.</returns>
		public T WithHidden(bool isHidden)
		{
			_fieldDescriptor.IsHidden = isHidden;
			return (T)this;
		}

		/// <summary>
		/// Sets the tooltip for the field descriptor.
		/// </summary>
		/// <param name="tooltip">The tooltip text to set. If null or whitespace, the tooltip will not be set.</param>
		/// <returns>The builder instance.</returns>
		public T WithTooltip(string tooltip)
		{
			if (String.IsNullOrWhiteSpace(tooltip))
			{
				return (T)this;
			}

			_fieldDescriptor.Tooltip = tooltip;
			return (T)this;
		}
	}

	/// <summary>
	/// Builder class for creating instances of the <see cref="FieldDescriptor"/> class.
	/// </summary>
	public class FieldDescriptorBuilder : FieldDescriptorBuilder<FieldDescriptorBuilder, FieldDescriptor>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDescriptorBuilder"/> class.
		/// </summary>
		public FieldDescriptorBuilder() : base(new FieldDescriptor())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDescriptorBuilder"/> class with a specified field descriptor.
		/// </summary>
		/// <param name="fieldDescriptor">The existing field descriptor to be used in the builder.</param>
		public FieldDescriptorBuilder(FieldDescriptor fieldDescriptor) : base(fieldDescriptor)
		{
		}
	}

	/// <summary>
	/// Builder class for creating instances of the <see cref="DomInstanceFieldDescriptor"/> class.
	/// </summary>
	public class DomInstanceFieldDescriptorBuilder : FieldDescriptorBuilder<DomInstanceFieldDescriptorBuilder, DomInstanceFieldDescriptor>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceFieldDescriptorBuilder"/> class.
		/// </summary>
		public DomInstanceFieldDescriptorBuilder() : base(new DomInstanceFieldDescriptor())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomInstanceFieldDescriptorBuilder"/> class with a specified field descriptor.
		/// </summary>
		/// <param name="fieldDescriptor">The existing <see cref="DomInstanceFieldDescriptor"/> to be used in the builder.</param>
		public DomInstanceFieldDescriptorBuilder(DomInstanceFieldDescriptor fieldDescriptor) : base(fieldDescriptor)
		{
		}

		/// <summary>
		/// Sets whether the field descriptor allows multiple values.
		/// </summary>
		/// <param name="allowMultiple">If <c>true</c>, the field will allow multiple values; otherwise, it will allow a single value.</param>
		/// <returns>The builder instance.</returns>
		public DomInstanceFieldDescriptorBuilder WithAllowMultiple(bool allowMultiple)
		{
			_fieldDescriptor.FieldType = allowMultiple ? typeof(List<Guid>) : typeof(Guid);

			return this;
		}

		/// <summary>
		/// Sets the module for the field descriptor and copies matching DOM definition IDs.
		/// </summary>
		/// <param name="module">The module name to set.</param>
		/// <returns>The builder instance.</returns>
		public DomInstanceFieldDescriptorBuilder WithModule(string module)
		{
			var newField = new DomInstanceFieldDescriptor(module)
			{
				ID = _fieldDescriptor.ID,
				Name = _fieldDescriptor.Name,
				FieldType = _fieldDescriptor.FieldType,
				IsHidden = _fieldDescriptor.IsHidden,
				IsOptional = _fieldDescriptor.IsOptional,
				IsReadonly = _fieldDescriptor.IsReadonly,
				IsSoftDeleted = _fieldDescriptor.IsSoftDeleted,
				DefaultValue = _fieldDescriptor.DefaultValue,
				Tooltip = _fieldDescriptor.Tooltip,
			};
			newField.DomDefinitionIds.AddRange(_fieldDescriptor.DomDefinitionIds.Where(
				def => String.IsNullOrEmpty(module) || def.ModuleId == module));

			_fieldDescriptor = newField;
			return this;
		}

		/// <summary>
		/// Sets the DOM definition IDs for the field descriptor.
		/// </summary>
		/// <param name="definitionIds">The collection of <see cref="DomDefinitionId"/> objects to set.</param>
		/// <returns>The builder instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="definitionIds"/> is <c>null</c>.</exception>
		public DomInstanceFieldDescriptorBuilder WithDefinitions(IEnumerable<DomDefinitionId> definitionIds)
		{
			if (definitionIds == null)
			{
				throw new ArgumentNullException(nameof(definitionIds));
			}

			_fieldDescriptor.DomDefinitionIds.Clear();
			foreach (var definition in definitionIds)
			{
				AddDomDefinition(definition);
			}

			return this;
		}

		/// <summary>
		/// Adds a DOM definition ID to the field descriptor.
		/// </summary>
		/// <param name="domDefinitionId">The DOM definition ID to add.</param>
		/// <returns>The builder instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="domDefinitionId"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="domDefinitionId"/> has an empty GUID.</exception>
		/// <remarks>
		/// If the <see cref="DomInstanceFieldDescriptor.ModuleId"/> or <see cref="DomDefinitionId.ModuleId"/> is not set (null or empty),
		/// the definition ID is assumed to be in the correct module and is added. Otherwise, the definition ID is only added if the module IDs match.
		/// </remarks>
		public DomInstanceFieldDescriptorBuilder AddDomDefinition(DomDefinitionId domDefinitionId)
		{
			if (domDefinitionId == null)
			{
				throw new ArgumentNullException(nameof(domDefinitionId));
			}

			if (domDefinitionId.Id == Guid.Empty)
			{
				throw new ArgumentException("domDefinitionId cannot be an empty guid.", nameof(domDefinitionId));
			}

			// In the case the ModuleId isn't filled in in either the descriptor or the given definition id, assume that it's in the correct module.
			if (String.IsNullOrEmpty(_fieldDescriptor.ModuleId) ||
			_fieldDescriptor.ModuleId == domDefinitionId.ModuleId)
			{
				_fieldDescriptor.DomDefinitionIds.Add(domDefinitionId);
				return this;
			}

			if (String.IsNullOrEmpty(domDefinitionId.ModuleId))
			{
				_fieldDescriptor.DomDefinitionIds.Add(domDefinitionId);
				return this;
			}

			return this;
		}
	}

	/// <summary>
	/// Builder class for creating instances of the <see cref="GenericEnumFieldDescriptor"/> class.
	/// </summary>
	public class GenericEnumFieldDescriptorBuilder : FieldDescriptorBuilder<GenericEnumFieldDescriptorBuilder, GenericEnumFieldDescriptor>
	{
		/// <summary>
		/// Specifies the underlying type for the enum values in a <see cref="GenericEnumFieldDescriptor"/>.
		/// </summary>
		public enum EnumType
		{
			/// <summary>
			/// Enum values are represented as integers.
			/// </summary>
			Int,

			/// <summary>
			/// Enum values are represented as strings.
			/// </summary>
			String,
		}

		private EnumType _type;
		private bool _allowMultiple;
		private string _enumName;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericEnumFieldDescriptorBuilder"/> class.
		/// </summary>
		public GenericEnumFieldDescriptorBuilder() : base(new GenericEnumFieldDescriptor())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericEnumFieldDescriptorBuilder"/> class with a specified field descriptor.
		/// </summary>
		/// <param name="fieldDescriptor">The existing <see cref="GenericEnumFieldDescriptor"/> to be used in the builder.</param>
		public GenericEnumFieldDescriptorBuilder(GenericEnumFieldDescriptor fieldDescriptor) : base(fieldDescriptor)
		{
		}

		/// <summary>
		/// Sets the enum type for the field descriptor.
		/// </summary>
		/// <param name="type">The underlying type of the enum values.</param>
		/// <returns>The builder instance.</returns>
		public GenericEnumFieldDescriptorBuilder WithEnumType(EnumType type)
		{
			_type = type;
			_fieldDescriptor.FieldType = BuildType();

			return this;
		}

		/// <summary>
		/// Sets whether the field descriptor allows multiple enum values.
		/// </summary>
		/// <param name="allowMultiple">If <c>true</c>, the field will allow multiple values; otherwise, it will allow a single value.</param>
		/// <returns>The builder instance.</returns>
		public GenericEnumFieldDescriptorBuilder WithAllowMultiple(bool allowMultiple)
		{
			_allowMultiple = allowMultiple;
			_fieldDescriptor.FieldType = BuildType();

			return this;
		}

		/// <summary>
		/// Sets the name of the enum for the field descriptor.
		/// </summary>
		/// <param name="enumName">The name of the enum.</param>
		/// <returns>The builder instance.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="enumName"/> is null or whitespace.</exception>
		public GenericEnumFieldDescriptorBuilder WithEnumName(string enumName)
		{
			if (String.IsNullOrWhiteSpace(enumName))
			{
				throw new ArgumentException($"'{nameof(enumName)}' cannot be null or whitespace.", nameof(enumName));
			}

			_enumName = enumName;
			_fieldDescriptor.GenericEnumInstance = BuildEnum();

			return this;
		}

		/// <summary>
		/// Sets the enum values for the field descriptor.
		/// </summary>
		/// <param name="enumEntries">The collection of enum entries to set.</param>
		/// <returns>The builder instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="enumEntries"/> is <c>null</c>.</exception>
		/// <exception cref="NotSupportedException">Thrown when an enum entry type does not match the configured enum type.</exception>
		public GenericEnumFieldDescriptorBuilder WithEnumValues(IEnumerable<IGenericEnumEntry> enumEntries)
		{
			if (enumEntries == null)
			{
				throw new ArgumentNullException(nameof(enumEntries));
			}

			var @enum = BuildEnum(true);
			foreach (var entry in enumEntries)
			{
				if (entry.ValueType == typeof(int) && _type == EnumType.Int)
				{
					(@enum as GenericEnum<int>).AddEntry(entry as GenericEnumEntry<int>);
				}
				else if (entry.ValueType == typeof(string) && _type == EnumType.String)
				{
					(@enum as GenericEnum<string>).AddEntry(entry as GenericEnumEntry<string>);
				}
				else
				{
					throw new NotSupportedException("The enum entry type does not match the configured enum type.");
				}
			}

			_fieldDescriptor.GenericEnumInstance = @enum;

			return this;
		}

		/// <summary>
		/// Adds a single enum value to the field descriptor.
		/// </summary>
		/// <param name="enumEntry">The enum entry to add.</param>
		/// <returns>The builder instance.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="enumEntry"/> is null.</exception>
		/// <exception cref="NotSupportedException">Thrown when the enum entry type does not match the configured enum type.</exception>
		public GenericEnumFieldDescriptorBuilder AddEnumValue(IGenericEnumEntry enumEntry)
		{
			if (enumEntry == null)
			{
				throw new ArgumentNullException(nameof(enumEntry));
			}

			if (_fieldDescriptor.GenericEnumInstance == null)
			{
				_fieldDescriptor.GenericEnumInstance = BuildEnum();
			}

			if (enumEntry.ValueType == typeof(int) && _type == EnumType.Int)
			{
				(_fieldDescriptor.GenericEnumInstance as GenericEnum<int>).AddEntry(enumEntry as GenericEnumEntry<int>);
			}
			else if (enumEntry.ValueType == typeof(string) && _type == EnumType.String)
			{
				(_fieldDescriptor.GenericEnumInstance as GenericEnum<string>).AddEntry(enumEntry as GenericEnumEntry<string>);
			}
			else
			{
				throw new NotSupportedException("The enum entry type does not match the configured enum type.");
			}

			return this;
		}

		/// <summary>
		/// Builds the type for the field descriptor based on the enum type and multiplicity.
		/// </summary>
		/// <returns>The constructed <see cref="Type"/> for the field descriptor.</returns>
		/// <exception cref="NotSupportedException">Thrown when the enum type cannot be determined.</exception>
		private Type BuildType()
		{
			switch (_type)
			{
				case EnumType.Int when _allowMultiple:
					return typeof(List<GenericEnum<int>>);

				case EnumType.Int:
					return typeof(GenericEnum<int>);

				case EnumType.String when _allowMultiple:
					return typeof(List<GenericEnum<string>>);

				case EnumType.String:
					return typeof(GenericEnum<string>);

				default:
					throw new NotSupportedException("Could not build the enum type based on the builder");
			}
		}

		/// <summary>
		/// Builds the enum instance for the field descriptor.
		/// </summary>
		/// <param name="empty">If <c>true</c>, creates an empty enum instance; otherwise, copies existing entries.</param>
		/// <returns>The constructed <see cref="IGenericEnum"/> instance.</returns>
		/// <exception cref="NotSupportedException">Thrown when the enum type cannot be determined.</exception>
		private IGenericEnum BuildEnum(bool empty = false)
		{
			switch (_type)
			{
				case EnumType.Int:
				{
					var @enum = new GenericEnum<int>
					{
						EnumName = _enumName,
					};

					if (empty)
					{
						return @enum;
					}

					var entries = _fieldDescriptor.GenericEnumInstance?.Entries.OfType<GenericEnumEntry<int>>().ToList() ?? new List<GenericEnumEntry<int>>();
					foreach (var entry in entries)
					{
						@enum.AddEntry(entry);
					}

					return @enum;
				}

				case EnumType.String:
				{
					var @enum = new GenericEnum<string>
					{
						EnumName = _enumName,
					};

					if (empty)
					{
						return @enum;
					}

					var entries = _fieldDescriptor.GenericEnumInstance?.Entries.OfType<GenericEnumEntry<string>>().ToList() ?? new List<GenericEnumEntry<string>>();
					foreach (var entry in entries)
					{
						@enum.AddEntry(entry);
					}

					return @enum;
				}

				default:
					throw new NotSupportedException("Could not build the enum type based on the builder");
			}
		}
	}
}