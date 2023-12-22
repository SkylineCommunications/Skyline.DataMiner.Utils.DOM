namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Apps.Sections.SectionDefinitions;
	using Skyline.DataMiner.Net.Sections;

	public class DomDefinitionBuilder<T> where T : DomDefinitionBuilder<T>
	{
		private readonly DomDefinition _definition;

		public DomDefinitionBuilder()
		{
			_definition = new DomDefinition();
		}

		public DomDefinitionBuilder(DomDefinition definition)
		{
			_definition = definition ?? throw new ArgumentNullException(nameof(definition));
		}

		public DomDefinition Build()
		{
			return _definition;
		}

		public T WithID(Guid id)
		{
			_definition.ID = new DomDefinitionId(id);

			return (T)this;
		}

		public T WithName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
			}

			_definition.Name = name;

			return (T)this;
		}

		public T WithDomBehaviorDefinition(DomBehaviorDefinitionId domBehaviorDefinitionId)
		{
			if (domBehaviorDefinitionId == null)
			{
				throw new ArgumentNullException(nameof(domBehaviorDefinitionId));
			}

			_definition.DomBehaviorDefinitionId = domBehaviorDefinitionId;

			return (T)this;
		}

		public T AddSectionDefinitionLink(SectionDefinitionLink sectionDefinitionLink)
		{
			if (sectionDefinitionLink == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionLink));
			}

			_definition.SectionDefinitionLinks.Add(sectionDefinitionLink);

			return (T)this;
		}

		public T AddSectionDefinitionLink(SectionDefinitionID sectionDefinitionID)
		{
			if (sectionDefinitionID == null)
			{
				throw new ArgumentNullException(nameof(sectionDefinitionID));
			}

			var sectionDefinitionLink = new SectionDefinitionLink(sectionDefinitionID);

			return AddSectionDefinitionLink(sectionDefinitionLink);
		}
	}

	public class DomDefinitionBuilder : DomDefinitionBuilder<DomDefinitionBuilder>
	{
		public DomDefinitionBuilder()
		{
		}

		public DomDefinitionBuilder(DomDefinition definition) : base(definition)
		{
		}
	}
}