namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;

	using Skyline.DataMiner.Net.Sections;

	public class FieldDescriptorBuilder<T> where T : FieldDescriptorBuilder<T>
	{
		private readonly FieldDescriptor _fieldDescriptor;

		public FieldDescriptorBuilder()
		{
			_fieldDescriptor = new FieldDescriptor();
		}

		public FieldDescriptorBuilder(FieldDescriptor fieldDescriptor)
		{
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException(nameof(fieldDescriptor));
			}

			_fieldDescriptor = fieldDescriptor;
		}

		public FieldDescriptor Build()
		{
			return _fieldDescriptor;
		}

		public T WithID(Guid id)
		{
			_fieldDescriptor.ID = new FieldDescriptorID(id);

			return (T)this;
		}

		public T WithName(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			_fieldDescriptor.Name = name;

			return (T)this;
		}

		public T WithType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			_fieldDescriptor.FieldType = type;

			return (T)this;
		}
	}

	public class FieldDescriptorBuilder : FieldDescriptorBuilder<FieldDescriptorBuilder>
	{
		public FieldDescriptorBuilder()
		{
		}

		public FieldDescriptorBuilder(FieldDescriptor fieldDescriptor) : base(fieldDescriptor)
		{
		}
	}
}