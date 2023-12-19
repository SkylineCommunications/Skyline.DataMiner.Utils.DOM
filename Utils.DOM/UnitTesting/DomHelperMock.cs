namespace Skyline.DataMiner.Utils.DOM.UnitTesting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages;

	public class DomHelperMock : DomHelper
	{
		public DomHelperMock(ICollection<DomInstance> instances) : base(x => HandleMessages(x, instances), "module")
		{
		}

		private static DMSMessage[] HandleMessages(DMSMessage[] messages, IEnumerable<DomInstance> instances)
		{
			var responses = new List<DMSMessage>();

			foreach (var message in messages)
			{
				switch (message)
				{
					case ManagerStoreReadRequest<DomInstance> domInstanceReadRequest:
						var domInstances = domInstanceReadRequest.Query
							.ExecuteInMemory(instances)
							.ToList();
						responses.Add(new ManagerStoreCrudResponse<DomInstance>(domInstances));
						break;
					default:
						throw new NotSupportedException($"Unsupported message type {message.GetType()}");
				}
			}

			return responses.ToArray();
		}
	}
}
