# SharpKit extension called UnobtrusiveFeatures

We really like developing client-side (web) applications using the same tools as the server-side. This started with [GWT](http://www.gwtproject.org/). After moving to C# we found [SharpKit](http://sharpkit.net), which allows compiling C# to javascript.

SharpKit is super, not as GWT, but still super. But there are some things that irritate me and one of them is the need for referencing SharpKit libraries for defining export behavior. Also you can't simply share export location for the whole solution. When some assembly is exported to more than one file, you must decorate every type with `JsTypeAttribute` and define `Filename`.

This plugin enables export to javascript without referencing any SharpKit library. The goal behide is to have reference-clear projects, that are "shared" between server and client.

When developing client-side only library, typically UI, there is no need for removing reference to SharpKit. But when such library is re-used in the server project, defining export in SharpKit standard way requires reference to SharpKit, and so requires distributing this library with the server application and also loading the assembly to the AppDomain.

This plugin enables exporting whole namespace to the specified file with single line in XML. This plugin enables inheriting definitions from parent directories. Define once, use on all projects in subdirectories.

## How to use?

As any other SharpKit plugin, copy released asemblies (from [Releases](https://github.com/maraf/SharpKit.UnobtrusiveFeatures/releases)) to the `C:\Windows\Microsoft.NET\Framework\v4.0.30319\SharpKit\5` and import SharpKit targets with plugin.

```XML
<Import Project="$(MSBuildBinPath)\SharpKit\5\SharpKit.Build.targets" />
<ItemGroup>
    <SkcPlugin Include="SharpKit.UnobtrusiveFeatures.Plugin, SharpKit.UnobtrusiveFeatures">
        <InProject>false</InProject>
    </SkcPlugin>
</ItemGroup>
```

## Unobtrusive export

With the UnobtrusiveFeatures plugin there is only XML configuration file and build targets import in the csproj. No reference required for defining export to the javascript.

The configuration XML follows [XSD](http://schemas.neptuo.com/xsd/sharpkit-unobtrusivefeatures.xsd) and so VisualStudio offers intellisense for it. 

```XML
<UnobtrusiveFeatures xmlns="http://schemas.neptuo.com/xsd/sharpkit-unobtrusivefeatures.xsd">
    <Export Filename="Neptuo.js" FilenameFormat="~/bin/{0}" />
    <Namespace Target="Neptuo.Compilers" Export="False" />
    <Namespace Target="Neptuo.Security.Cryptography" Export="False" />
    <Namespace Target="Neptuo.Threading" Export="False" />
    <Type Target="Neptuo.Activators.DependencyServiceProvider" Export="False" />
    <Type Target="Neptuo.CodeDom.Compiler.CsCodeDomCompiler" Export="False" />
    <Type Target="Neptuo.Collections.Specialized.NameValueReadOnlyDictionary" Export="False" />
</UnobtrusiveFeatures>
```

Also, the configuration file can be hierarchically inherited. If the project directory is for example `D:\Development\Framework\MyFramework\src`, in every directory on the path can be configuration file, named `SharpKit.UnobtrusiveFeatures.xml` or as currenly exported assembly, eg: `MyFramework.xml`. These file are then in-memory-merged into a single configuration during compilation.

Also, the `Filename` and `FilenameFormat` attributes of `Export` element can contain placeholder for assembly name. We can define single configuration file in the root of all development projects, define like:

```XML
<UnobtrusiveFeatures xmlns="http://schemas.neptuo.com/xsd/sharpkit-unobtrusivefeatures.xsd">
    <Export FilenameFormat="~/bin/{0}" Filename="{AssemblyName}.js" />
</UnobtrusiveFeatures>
```

And this configuration file will export every SharpKit-enabled project to the file, named as assembly in the bin folder of the project.

Advanced export features:
- _Export_ defines global export. With this element we can define fallback file and path format to export all types to.
- _ExternalTypes_ can be used, when we want to export used attributes which are referenced from assemblies, that we don't have source files for. As SharpKit such attributes automatically ommits, we can override this behavior to include them in the compilation. Note: Only the use of attributes is compiled. We are responsible to provide javascript type defines ourselves.
- _Namespace_ defines exporting rules to namespace and all subnamespaces. With this element we can exclude whole namespace or change target file name.
- _Type_ defines exporting rules for concrete class, like using `JsTypeAttribute`.

## Extended C# artifacts

The plugin also exports some C# artifacts that are not export the SharpKit it self. The first one is a `IsAbstract` flag, that contains information whether the class can be instantiated. 

```Javascript
var TestSharpKit$UI$Presenter = {
    ...
    IsAbstract: false,
    ...
}
```

The second one is a list of constructors defined in the class. With this feature, we can simply build IoC container with constructor injection. The export looks like:

```Javascript
var TestSharpKit$UI$Presenter = {
    ...
    ctors: [{
        name: "ctor",
        parameters: ["TestSharpKit.UI.Views.MainView"]
    }],
    ...
}
```

## Summary

So, export plugin simplifies javascript compilation. The constructor definitions are useful for IoC container or for reflection uses. The `IsAbstract` flag is always good to have. When talking about IoC container, [in the Neptuo framework repository there is one (SharpKit and javascript compatible) implemented](https://github.com/neptuo/Framework/tree/master/src/Neptuo.Activators.Simple). It supports both property and constructor injection.

The `SharpKit.UnobtrusiveFeatures` is developed as open-source at [GitHub repo SharpKit.UnobtrusiveFeatures](http://github.com/maraf/SharpKit.UnobtrusiveFeatures) where you can find more documentation.
