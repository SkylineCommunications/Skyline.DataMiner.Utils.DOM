# Skyline.DataMiner.Utils.DOM

## About

### About Skyline.DataMiner.Utils.DOM

Provides a set of useful classes and extension methods that can make it easier to interact with the core [DataMiner Object Model (DOM)](https://docs.dataminer.services/user-guide/Advanced_Modules/DOM/DOM.html) API.

### About DataMiner

DataMiner is a transformational platform that provides vendor-independent control and monitoring of devices and services. Out of the box and by design, it addresses key challenges such as security, complexity, multi-cloud, and much more. It has a pronounced open architecture and powerful capabilities enabling users to evolve easily and continuously.

The foundation of DataMiner is its powerful and versatile data acquisition and control layer. With DataMiner, there are no restrictions to what data users can access. Data sources may reside on premises, in the cloud, or in a hybrid setup.

A unique catalog of 7000+ connectors already exists. In addition, you can leverage DataMiner Development Packages to build your own connectors (also known as "protocols" or "drivers").

> [!NOTE]
> See also: [About DataMiner](https://aka.dataminer.services/about-dataminer).

### About Skyline Communications

At Skyline Communications, we deal with world-class solutions that are deployed by leading companies around the globe. Check out [our proven track record](https://aka.dataminer.services/about-skyline) and see how we make our customers' lives easier by empowering them to take their operations to the next level.

## Requirements

The "DataMiner Integration Studio" Visual Studio extension is required for development of connectors and Automation scripts using NuGets.

See [Installing DataMiner Integration Studio](https://aka.dataminer.services/DisInstallation)

## Getting started

To get started, the `Skyline.DataMiner.Utils.DOM` NuGet package needs to be added to the C# project from [nuget.org](https://www.nuget.org/packages/Skyline.DataMiner.Utils.DOM)
For more information see https://docs.dataminer.services/develop/TOOLS/NuGet/Consuming_NuGet.html.

### Caching

The `DomCache` class can be used to store DOM instances and definitions in memory. When an object is retrieved for the first time, it will be fetched from DataMiner and stored in memory.
For subsequent requests with the same ID, the cached object is retrieved directly from memory. This will improve the performance, in scenarios where the same object is retrieved multiple times from DOM.

> [!NOTE]
> When a DOM object is being updated from an external location, it's not automatically updated in this cache as well. A new cache can be created to retrieve the latest version from the server again.

 ```cs
var guid = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-1234567890ab");
var domCache = new DomCache(...);

// will be retrieved from DMA
var instance1 = domCache.GetInstanceById(guid);

// will be retrieved from cache
var instance2 = domCache.GetInstanceById(guid);
```

### Builders

Quickly build DOM instances and sections using the fluent builder pattern:

 ```cs
public DomInstance CreateDomInstance()
{
	var instance = new DomInstanceBuilder(FleFlows.Definitions.Flow)
		.WithID(_sourceFlowId1)
		.AddSection(FleFlows.Sections.FlowInfo.Id, b => b
			.WithFieldValue(FleFlows.Sections.FlowInfo.Name, "Source Flow 1"))
		.AddSection(FleFlows.Sections.FlowPath.Id, b => b
			.WithFieldValue(FleFlows.Sections.FlowPath.FlowDirection, (int)FleFlows.Enums.FlowDirection.Tx)
			.WithFieldValue(FleFlows.Sections.FlowPath.Element, "123/1")
			.WithFieldValue(FleFlows.Sections.FlowPath.Interface, "eth0"))
		.AddSection(FleFlows.Sections.FlowTransportIP.Id, b => b
			.WithFieldValue(FleFlows.Sections.FlowTransportIP.SourceIP, "10.20.30.5")
			.WithFieldValue(FleFlows.Sections.FlowTransportIP.DestinationIP, "239.17.0.5")
			.WithFieldValue(FleFlows.Sections.FlowTransportIP.DestinationPort, 5000))
		.Build();

	return instance;
}
```

### Unit testing

Using the `DomHelperMock` and `DomCacheMock` classes it's possible to execute DOM queries completely on memory on a provided collection of DOM instances and definitions.
These classes are derived from the `DomHelper` and `DomCache` classes, so the mocked object can directly be used in the code that should be unit tested.

```cs
var instances = new List<DomInstance>
{
	new DomInstanceBuilder(FleFlows.Definitions.Flow)
		.WithID(Guid.Parse("d70834e1-f9b5-4551-b181-c6c59dbd2127"))
		.AddSection(new DomSectionBuilder(FleFlows.Sections.FlowInfo.Id)
			.WithFieldValue(FleFlows.Sections.FlowInfo.Name, "Source Flow 1"))
		.Build(),
	new DomInstanceBuilder(FleFlows.Definitions.Flow)
		.WithID(Guid.Parse("c5776178-026d-4826-9801-9dd43bb1ccfb"))
		.AddSection(new DomSectionBuilder(FleFlows.Sections.FlowInfo.Id)
			.WithFieldValue(FleFlows.Sections.FlowInfo.Name, "Source Flow 2"))
		.Build(),
};

var domHelper = DomHelperMock.Create(instances);
var instance1 = domHelper.DomInstances.GetByID(Guid.Parse("d70834e1-f9b5-4551-b181-c6c59dbd2127"));
```

In the same way, `DomCacheMock` can be used to mock the `DomCache` class:

```cs
var instances = new List<DomInstance>();
var domCache = DomCacheMock.Create(instances);
```

