namespace Skyline.DataMiner.Utils.DOM
{
	using System;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;
	using Skyline.DataMiner.Net.SubscriptionFilters;

	/// <summary>
	/// Watches for changes to DOM (DataMiner Object Model) instances and raises events when changes occur.
	/// </summary>
	public class DomWatcher : IDisposable
	{
		private readonly object _lock = new object();
		private readonly IConnection _connection;

		private readonly string _subscriptionSetId;
		private readonly SubscriptionFilter[] _subscriptionFilters;

		private int _subscriberCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomWatcher"/> class.
		/// </summary>
		/// <param name="module">The module name used for filtering the subscription.</param>
		/// <param name="filter">The filter to apply on the DOM instances.</param>
		/// <param name="connection">The DataMiner connection object used for subscriptions.</param>
		/// <exception cref="ArgumentException">Thrown when <paramref name="module"/> is null or whitespace.</exception>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="filter"/> or <paramref name="connection"/> is null.</exception>
		public DomWatcher(string module, FilterElement<DomInstance> filter, IConnection connection)
		{
			if (String.IsNullOrWhiteSpace(module))
			{
				throw new ArgumentException($"'{nameof(module)}' cannot be null or whitespace.", nameof(module));
			}

			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter));
			}

			_connection = connection ?? throw new ArgumentNullException(nameof(connection));

			_subscriptionSetId = $"DomInstanceSubscription_{nameof(DomWatcher)}_{Guid.NewGuid()}";
			_subscriptionFilters = new SubscriptionFilter[]
			{
				new ModuleEventSubscriptionFilter<DomInstancesChangedEventMessage>(module),
				new SubscriptionFilter<DomInstancesChangedEventMessage, DomInstance>(filter),
			};
		}

		/// <summary>
		/// Occurs when a DOM instance change message is received that matches the module and filter.
		/// </summary>
		public event EventHandler<DomInstancesChangedEventMessage> OnChanged
		{
			add
			{
				lock (_lock)
				{
					CheckAndSubscribe();
					Changed += value;
				}
			}

			remove
			{
				lock (_lock)
				{
					Changed -= value;
					CheckAndUnsubscribe();
				}
			}
		}

		private event EventHandler<DomInstancesChangedEventMessage> Changed;

		/// <summary>
		/// Disposes the watcher, removing any active subscriptions.
		/// </summary>
		public void Dispose()
		{
			_connection.ClearSubscriptions(_subscriptionSetId);
			_connection.OnNewMessage -= Connection_OnNewMessage;
		}

		private void Connection_OnNewMessage(object sender, NewMessageEventArgs e)
		{
			if (!e.FromSet(_subscriptionSetId))
			{
				// Not for our subscription
				return;
			}

			if (e.Message is DomInstancesChangedEventMessage domChange)
			{
				Changed?.Invoke(this, domChange);
			}
		}

		private void CheckAndSubscribe()
		{
			if (_subscriberCount <= 0)
			{
				_connection.OnNewMessage += Connection_OnNewMessage;
				_connection.AddSubscription(_subscriptionSetId, _subscriptionFilters);
				_connection.Subscribe();
			}

			_subscriberCount++;
		}

		private void CheckAndUnsubscribe()
		{
			_subscriberCount--;

			if (_subscriberCount <= 0)
			{
				_connection.ClearSubscriptions(_subscriptionSetId);
				_connection.OnNewMessage -= Connection_OnNewMessage;
			}
		}
	}
}
