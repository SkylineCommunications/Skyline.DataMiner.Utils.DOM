// Ignore Spelling: Utils

namespace Skyline.DataMiner.Utils.DOM.Builders
{
	using System;
	using System.Text.RegularExpressions;

	using Skyline.DataMiner.Net.Apps.DataMinerObjectModel.Settings;
	using Skyline.DataMiner.Net.Apps.Modules;

	/// <summary>
	/// Provides a generic builder for configuring and creating <see cref="ModuleSettings"/> instances for DOM modules.
	/// </summary>
	/// <typeparam name="T">The type of the builder, used for fluent chaining.</typeparam>
	public class DomModuleBuilder<T> where T : DomModuleBuilder<T>
	{
		/// <summary>
		/// Regular expression used to validate module IDs.
		/// </summary>
		private readonly Regex _moduleValidator = new Regex(@"^(?!.*[A-Z\\/\\*\\?""<>| ,#:\-]).{1,40}$", RegexOptions.Compiled);

		/// <summary>
		/// The <see cref="ModuleSettings"/> instance being built.
		/// </summary>
		protected readonly ModuleSettings _moduleSettings;

		/// <summary>
		/// Initializes a new instance of the <see cref="DomModuleBuilder{T}"/> class.
		/// </summary>
		public DomModuleBuilder()
		{
			_moduleSettings = new ModuleSettings();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomModuleBuilder{T}"/> class using an existing <see cref="ModuleSettings"/>.
		/// </summary>
		/// <param name="moduleSettings">The existing <see cref="ModuleSettings"/> to use.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="moduleSettings"/> is <c>null</c>.</exception>
		public DomModuleBuilder(ModuleSettings moduleSettings)
		{
			if (moduleSettings == null)
			{
				throw new ArgumentNullException(nameof(moduleSettings));
			}

			_moduleSettings = moduleSettings;
		}

		/// <summary>
		/// Builds and returns the configured <see cref="ModuleSettings"/> instance.
		/// </summary>
		/// <returns>The configured <see cref="ModuleSettings"/>.</returns>
		public ModuleSettings Build()
		{
			return _moduleSettings;
		}

		/// <summary>
		/// Sets the module ID for the DOM module.
		/// </summary>
		/// <param name="moduleId">The module ID. Must be 1-40 lowercase alphanumeric characters or underscores.</param>
		/// <returns>The builder instance for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="moduleId"/> is <c>null</c> or empty.</exception>
		/// <exception cref="ArgumentException">Thrown when <paramref name="moduleId"/> does not match the required format.</exception>
		public T WithModuleId(string moduleId)
		{
			if (String.IsNullOrEmpty(moduleId))
			{
				throw new ArgumentNullException(nameof(moduleId));
			}

			if (!_moduleValidator.IsMatch(moduleId))
			{
				throw new ArgumentException("Module ID must be 1-40 lowercase alphanumeric characters and/or lowercases.", nameof(moduleId));
			}

			_moduleSettings.ModuleId = moduleId;

			return (T)this;
		}

		/// <summary>
		/// Enables or disables information events for the DOM module.
		/// </summary>
		/// <param name="enableInformationEvents">If <c>true</c>, information events are enabled; otherwise, disabled.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithInformationEvents(bool enableInformationEvents)
		{
			_moduleSettings.DomManagerSettings.InformationEventSettings.Enable = enableInformationEvents;
			return (T)this;
		}

		/// <summary>
		/// Enables or disables history storage for DOM instances.
		/// </summary>
		/// <param name="enableHistory">If <c>true</c>, history storage is enabled asynchronously; otherwise, it is disabled.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithHistory(bool enableHistory)
		{
			if (enableHistory)
			{
				_moduleSettings.DomManagerSettings.DomInstanceHistorySettings.StorageBehavior = DomInstanceHistoryStorageBehavior.EnabledAsync;
			}
			else
			{
				_moduleSettings.DomManagerSettings.DomInstanceHistorySettings.StorageBehavior = DomInstanceHistoryStorageBehavior.Disabled;
			}

			return (T)this;
		}

		/// <summary>
		/// Sets the history storage behavior for DOM instances.
		/// </summary>
		/// <param name="behavior">The history storage behavior to use.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithHistoryBehavior(DomInstanceHistoryStorageBehavior behavior)
		{
			_moduleSettings.DomManagerSettings.DomInstanceHistorySettings.StorageBehavior = behavior;
			return (T)this;
		}

		/// <summary>
		/// Sets the time-to-live (TTL) for DOM instances.
		/// </summary>
		/// <param name="instanceLifetime">The instance lifetime. If <c>null</c> or negative, TTL is set to unlimited.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithInstanceTTL(TimeSpan? instanceLifetime)
		{
			if (!instanceLifetime.HasValue || instanceLifetime.Value < TimeSpan.Zero)
			{
				_moduleSettings.DomManagerSettings.TtlSettings.DomInstanceTtl = TimeSpan.Zero;
			}
			else
			{
				_moduleSettings.DomManagerSettings.TtlSettings.DomInstanceTtl = instanceLifetime.Value;
			}

			return (T)this;
		}

		/// <summary>
		/// Sets the time-to-live (TTL) for DOM instance history.
		/// </summary>
		/// <param name="historyLifetime">The history lifetime. If <c>null</c> or negative, TTL is set to unlimited.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithHistoryTTL(TimeSpan? historyLifetime)
		{
			if (!historyLifetime.HasValue || historyLifetime.Value < TimeSpan.Zero)
			{
				_moduleSettings.DomManagerSettings.TtlSettings.DomInstanceHistoryTtl = TimeSpan.Zero;
			}
			else
			{
				_moduleSettings.DomManagerSettings.TtlSettings.DomInstanceHistoryTtl = historyLifetime.Value;
			}

			return (T)this;
		}

		/// <summary>
		/// Sets the time-to-live (TTL) for DOM templates.
		/// </summary>
		/// <param name="templateLifetime">The template lifetime. If <c>null</c> or negative, TTL is set to unlimited.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithTemplateTTL(TimeSpan? templateLifetime)
		{
			if (!templateLifetime.HasValue || templateLifetime.Value < TimeSpan.Zero)
			{
				_moduleSettings.DomManagerSettings.TtlSettings.DomTemplateTtl = TimeSpan.Zero;
			}
			else
			{
				_moduleSettings.DomManagerSettings.TtlSettings.DomTemplateTtl = templateLifetime.Value;
			}

			return (T)this;
		}

		/// <summary>
		/// Sets the script to be executed when a DOM instance is created.
		/// </summary>
		/// <param name="scriptName">The name of the script to execute on creation, or an empty string to not execute a script.</param>
		/// <returns>The builder instance for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="scriptName"/> is <c>null</c>.</exception>
		public T WithOnCreateScript(string scriptName)
		{
			if (scriptName == null)
			{
				throw new ArgumentNullException(nameof(scriptName));
			}

			_moduleSettings.DomManagerSettings.ScriptSettings.OnCreate = scriptName;
			return (T)this;
		}

		/// <summary>
		/// Sets the script to be executed when a DOM instance is updated.
		/// </summary>
		/// <param name="scriptName">The name of the script to execute on update, or an empty string to not execute a script.</param>
		/// <returns>The builder instance for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="scriptName"/> is <c>null</c>.</exception>
		public T WithOnUpdateScript(string scriptName)
		{
			if (scriptName == null)
			{
				throw new ArgumentNullException(nameof(scriptName));
			}

			_moduleSettings.DomManagerSettings.ScriptSettings.OnUpdate = scriptName;
			return (T)this;
		}

		/// <summary>
		/// Sets the script to be executed when a DOM instance is deleted.
		/// </summary>
		/// <param name="scriptName">The name of the script to execute on deletion, or an empty string to not execute a script</param>
		/// <returns>The builder instance for chaining.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="scriptName"/> is <c>null</c>.</exception>
		public T WithOnDeleteScript(string scriptName)
		{
			if (scriptName == null)
			{
				throw new ArgumentNullException(nameof(scriptName));
			}

			_moduleSettings.DomManagerSettings.ScriptSettings.OnDelete = scriptName;
			return (T)this;
		}

		/// <summary>
		/// Sets the CRUD script type for DOM instance actions.
		/// </summary>
		/// <param name="crudType">The CRUD script type to use.</param>
		/// <returns>The builder instance for chaining.</returns>
		public T WithCrudType(OnDomInstanceActionScriptType crudType)
		{
			_moduleSettings.DomManagerSettings.ScriptSettings.ScriptType = crudType;
			return (T)this;
		}
	}

	/// <summary>
	/// Represents a builder for creating instances of <see cref="ModuleSettings"/>.
	/// </summary>
	public class DomModuleBuilder : DomModuleBuilder<DomModuleBuilder>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DomModuleBuilder"/> class.
		/// </summary>
		public DomModuleBuilder()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DomModuleBuilder"/> class with a specified <see cref="ModuleSettings"/>.
		/// </summary>
		/// <param name="moduleSettings">The existing DOM definition to be used in the builder.</param>
		public DomModuleBuilder(ModuleSettings moduleSettings) : base(moduleSettings)
		{
		}
	}
}
