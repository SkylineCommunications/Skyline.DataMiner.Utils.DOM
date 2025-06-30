namespace Skyline.DataMiner.Utils.DOM.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net;
	using Skyline.DataMiner.Net.Helper;
	using Skyline.DataMiner.Net.IManager.Objects;
	using Skyline.DataMiner.Net.ManagerStore;
	using Skyline.DataMiner.Net.Messages;
	using Skyline.DataMiner.Net.Messages.SLDataGateway;

	using SLDataGateway.API.Types.Querying;

	/// <summary>
	/// Extension methods for <see cref="CrudHelperComponent{T}" />.
	/// </summary>
	public static class CrudHelperComponentExtensions
	{
		/// <summary>
		/// Reads data from the helper in a paged manner using a specified filter.
		/// </summary>
		/// <typeparam name="T">The type of the data elements.</typeparam>
		/// <param name="helper">The helper component used to retrieve data.</param>
		/// <param name="filter">The filter criteria to apply.</param>
		/// <param name="pageSize">The size of each page to retrieve.</param>
		/// <returns>An enumerable collection of data elements.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> or <paramref name="filter"/> is null.</exception>
		public static IEnumerable<IEnumerable<T>> ReadPaged<T>(this ICrudHelperComponent<T> helper, FilterElement<T> filter, long pageSize = 500)
			where T : DataType
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (filter == null)
			{
				throw new ArgumentNullException(nameof(filter));
			}

			return ReadPagedIterator(helper, filter, pageSize);
		}

		/// <summary>
		/// Reads items in a paged manner, based on the specified query and page size.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="helper">The helper component for CRUD operations.</param>
		/// <param name="query">The query to apply to the data.</param>
		/// <param name="pageSize">The number of items per page.</param>
		/// <returns>An enumerable collection of the queried data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when helper or query is null.</exception>
		public static IEnumerable<IEnumerable<T>> ReadPaged<T>(this ICrudHelperComponent<T> helper, IQuery<T> query, long pageSize = 500)
			where T : DataType
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			return ReadPagedIterator(helper, query, pageSize);
		}

		/// <summary>
		/// Reads all items in a paged manner with the specified page size.
		/// </summary>
		/// <typeparam name="T">The type of data to read.</typeparam>
		/// <param name="helper">The helper component for CRUD operations.</param>
		/// <param name="pageSize">The number of items per page.</param>
		/// <returns>An enumerable collection of all data.</returns>
		/// <exception cref="ArgumentNullException">Thrown when helper is null.</exception>
		public static IEnumerable<IEnumerable<T>> ReadAllPaged<T>(this ICrudHelperComponent<T> helper, long pageSize = 500)
			where T : DataType
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			return ReadPaged(helper, new TRUEFilterElement<T>(), pageSize);
		}

		/// <summary>
		/// Creates or updates a collection of instances in batches.
		/// </summary>
		/// <typeparam name="T">The type of the data elements.</typeparam>
		/// <typeparam name="K">The type of the identifier for the data elements.</typeparam>
		/// <param name="helper">The bulk CRUD helper component used to perform create or update operations.</param>
		/// <param name="instances">The instances to create or update.</param>
		/// <returns>A result indicating the success and failure details of the operation.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> or <paramref name="instances"/> is null.</exception>
		public static BulkCreateOrUpdateResult<T, K> CreateOrUpdateInBatches<T, K>(this IBulkCrudTryHelperComponent<T, K> helper, IEnumerable<T> instances)
			where T : IManagerIdentifiableObject<K>, DataType
			where K : IEquatable<K>
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var successfulItems = new List<T>();
			var unsuccessfulIds = new List<K>();
			var traceDataPerItem = new Dictionary<K, TraceData>();

			foreach (var batch in instances.Batch(100))
			{
				helper.TryCreateOrUpdate(batch.ToList(), out var batchResult);

				successfulItems.AddRange(batchResult.SuccessfulItems);
				unsuccessfulIds.AddRange(batchResult.UnsuccessfulIds);

				foreach (var item in batchResult.TraceDataPerItem)
				{
					traceDataPerItem[item.Key] = item.Value;
				}
			}

			var result = new BulkCreateOrUpdateResult<T, K>(successfulItems, unsuccessfulIds, traceDataPerItem);
			result.ThrowOnFailure();

			return result;
		}

		/// <summary>
		/// Creates or updates a collection of instances in batches.
		/// </summary>
		/// <typeparam name="T">The type of the data elements.</typeparam>
		/// <typeparam name="K">The type of the identifier for the data elements.</typeparam>
		/// <param name="helper">The bulk CRUD helper component used to perform create or update operations.</param>
		/// <param name="instances">The instances to create or update.</param>
		/// <param name="result">The result indicating the success and failure details of the operation.</param>
		/// <returns>True if all items were created or updated successfully; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> or <paramref name="instances"/> is null.</exception>
		public static bool TryCreateOrUpdateInBatches<T, K>(this IBulkCrudTryHelperComponent<T, K> helper, IEnumerable<T> instances, out BulkCreateOrUpdateResult<T, K> result)
			where T : IManagerIdentifiableObject<K>, DataType
			where K : IEquatable<K>
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var successfulItems = new List<T>();
			var unsuccessfulIds = new List<K>();
			var traceDataPerItem = new Dictionary<K, TraceData>();

			foreach (var batch in instances.Batch(100))
			{
				helper.TryCreateOrUpdate(batch.ToList(), out var batchResult);

				successfulItems.AddRange(batchResult.SuccessfulItems);
				unsuccessfulIds.AddRange(batchResult.UnsuccessfulIds);

				foreach (var item in batchResult.TraceDataPerItem)
				{
					traceDataPerItem[item.Key] = item.Value;
				}
			}

			result = new BulkCreateOrUpdateResult<T, K>(successfulItems, unsuccessfulIds, traceDataPerItem);

			return !result.HasFailures();
		}

		/// <summary>
		/// Deletes a collection of instances in batches.
		/// </summary>
		/// <typeparam name="T">The type of the data elements.</typeparam>
		/// <typeparam name="K">The type of the identifier for the data elements.</typeparam>
		/// <param name="helper">The bulk CRUD helper component used to perform delete operations.</param>
		/// <param name="instances">The instances to delete.</param>
		/// <returns>A result indicating the success and failure details of the operation.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> or <paramref name="instances"/> is null.</exception>
		public static BulkDeleteResult<K> DeleteInBatches<T, K>(this IBulkCrudTryHelperComponent<T, K> helper, IEnumerable<T> instances)
			where T : IManagerIdentifiableObject<K>, DataType
			where K : IEquatable<K>
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var successfulIds = new List<K>();
			var unsuccessfulIds = new List<K>();
			var traceDataPerItem = new Dictionary<K, TraceData>();

			foreach (var batch in instances.Batch(100))
			{
				helper.TryDelete(batch.ToList(), out var batchResult);

				successfulIds.AddRange(batchResult.SuccessfulIds);
				unsuccessfulIds.AddRange(batchResult.UnsuccessfulIds);

				foreach (var item in batchResult.TraceDataPerItem)
				{
					traceDataPerItem[item.Key] = item.Value;
				}
			}

			var result = new BulkDeleteResult<K>(successfulIds, unsuccessfulIds, traceDataPerItem);
			result.ThrowOnFailure();

			return result;
		}

		/// <summary>
		/// Deletes a collection of instances in batches.
		/// </summary>
		/// <typeparam name="T">The type of the data elements.</typeparam>
		/// <typeparam name="K">The type of the identifier for the data elements.</typeparam>
		/// <param name="helper">The bulk CRUD helper component used to perform delete operations.</param>
		/// <param name="instances">The instances to delete.</param>
		/// <param name="result">The result indicating the success and failure details of the operation.</param>
		/// <returns>True if all items were deleted successfully; otherwise, false.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="helper"/> or <paramref name="instances"/> is null.</exception>
		public static bool TryDeleteInBatches<T, K>(this IBulkCrudTryHelperComponent<T, K> helper, IEnumerable<T> instances, out BulkDeleteResult<K> result)
			where T : IManagerIdentifiableObject<K>, DataType
			where K : IEquatable<K>
		{
			if (helper == null)
			{
				throw new ArgumentNullException(nameof(helper));
			}

			if (instances == null)
			{
				throw new ArgumentNullException(nameof(instances));
			}

			var successfulIds = new List<K>();
			var unsuccessfulIds = new List<K>();
			var traceDataPerItem = new Dictionary<K, TraceData>();

			foreach (var batch in instances.Batch(100))
			{
				helper.TryDelete(batch.ToList(), out var batchResult);

				successfulIds.AddRange(batchResult.SuccessfulIds);
				unsuccessfulIds.AddRange(batchResult.UnsuccessfulIds);

				foreach (var item in batchResult.TraceDataPerItem)
				{
					traceDataPerItem[item.Key] = item.Value;
				}
			}

			result = new BulkDeleteResult<K>(successfulIds, unsuccessfulIds, traceDataPerItem);

			return !result.HasFailures();
		}

		private static IEnumerable<IEnumerable<T>> ReadPagedIterator<T>(ICrudHelperComponent<T> helper, FilterElement<T> filter, long pageSize) where T : DataType
		{
			var pagingHelper = helper.PreparePaging(filter, pageSize);

			while (pagingHelper.MoveToNextPage())
			{
				yield return pagingHelper.GetCurrentPage();
			}
		}

		private static IEnumerable<IEnumerable<T>> ReadPagedIterator<T>(ICrudHelperComponent<T> helper, IQuery<T> query, long pageSize) where T : DataType
		{
			var pagingHelper = helper.PreparePaging(query, pageSize);

			while (pagingHelper.MoveToNextPage())
			{
				yield return pagingHelper.GetCurrentPage();
			}
		}
	}
}
