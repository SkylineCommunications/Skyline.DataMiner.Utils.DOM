namespace Skyline.DataMiner.Utils.DOM.Builders
{
    using System;

    using Skyline.DataMiner.Net.Sections;

	/// <summary>
	/// Generic builder class for creating instances of the <see cref="FieldDescriptor"/> class.
	/// </summary>
	/// <typeparam name="T">The type of the builder class.</typeparam>
	public class FieldDescriptorBuilder<T> where T : FieldDescriptorBuilder<T>
    {
        private readonly FieldDescriptor _fieldDescriptor;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDescriptorBuilder{T}"/> class.
		/// </summary>
		public FieldDescriptorBuilder()
        {
            _fieldDescriptor = new FieldDescriptor();
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDescriptorBuilder{T}"/> class with a specified field descriptor.
		/// </summary>
		/// <param name="fieldDescriptor">The existing field descriptor to be used in the builder.</param>
		public FieldDescriptorBuilder(FieldDescriptor fieldDescriptor)
        {
            _fieldDescriptor = fieldDescriptor ?? throw new ArgumentNullException(nameof(fieldDescriptor));
        }

		/// <summary>
		/// Builds the field descriptor.
		/// </summary>
		/// <returns>The constructed <see cref="FieldDescriptor"/>.</returns>
		public FieldDescriptor Build()
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
    }

	/// <summary>
	/// Builder class for creating instances of the <see cref="FieldDescriptor"/> class.
	/// </summary>
	public class FieldDescriptorBuilder : FieldDescriptorBuilder<FieldDescriptorBuilder>
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="FieldDescriptorBuilder"/> class.
		/// </summary>
		public FieldDescriptorBuilder()
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
}