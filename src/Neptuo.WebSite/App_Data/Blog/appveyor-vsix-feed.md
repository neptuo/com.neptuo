> One of the motivations behing this post is [Mads Kristensen's tool for generating private feeds](https://devblogs.microsoft.com/visualstudio/create-a-private-gallery-for-self-hosted-visual-studio-extensions/).

First of all, we need to update all VSIX manifests to match current CI build version.

Add a script to be executed before build.

```yml
before_build:
- ps: .\tools\AppVeyor-BeforeBuild.ps1
```

**[AppVeyor-BeforeBuild.ps1](https://github.com/neptuo/Productivity/blob/master/tools/AppVeyor-BeforeBuild.ps1)**
```powershell
...

Vsix-SetVersion;

...
```

**[Vsix-SetVersion.ps1](https://github.com/neptuo/Productivity/blob/master/tools/Vsix-SetVersion.ps1)**

```powershell
function Vsix-SetVersion 
{
    param([string] $version = $env:APPVEYOR_BUILD_NUMBER)

    if (-not($version)) 
    { 
        Throw "Parameter -Version is required";
    }

    $targetVersion = "0.0.$version";

    Push-Location $PSScriptRoot;

    Write-Host "Updating manifests to version '$targetVersion'.";

    $manifestFiles = Get-ChildItem -Path ..\ -Filter *.vsixmanifest -Recurse -File -Name;
    foreach ($manifestFile in $manifestFiles) 
    {
        $manifestFile = Resolve-Path -Path "..\$manifestFile";
        Write-Host "Updating manifest $manifestFile";

        [xml]$manifestXml = Get-Content $manifestFile

        $ns = New-Object System.Xml.XmlNamespaceManager $manifestXml.NameTable
        $ns.AddNamespace("ns", $manifestXml.DocumentElement.NamespaceURI) | Out-Null

        $attrVersion = ""

        if ($manifestXml.SelectSingleNode("//ns:Identity", $ns))
        {
            $attrVersion = $manifestXml.SelectSingleNode("//ns:Identity", $ns).Attributes["Version"]
            
            $attrVersion.InnerText = $targetVersion
            $manifestXml.Save($manifestFile) | Out-Null
        }
    }

    Pop-Location;
}
```

It iterates over all *.vsixmanifest files, loads content as XML and updates attribute `Version` of element `Identity`.

Now we can run msbuild and produce CI builds of correct version. Next step is to grab all VSIX and push them as AppVeyor artifacts.

Add a post build action `after_build` to the `appveyor.yml`.

```yml
after_build:
- ps: .\tools\AppVeyor-PushArtifacts.ps1
```

The `AppVeyor-PushArtifacts.ps1` included later uses a VSIX feed template file.

**[NightlyFeedTemplate.xml](https://github.com/neptuo/Productivity/blob/master/NightlyFeedTemplate.xml)**

```xml
<?xml version="1.0" encoding="UTF-8"?>
<feed xmlns="http://www.w3.org/2005/Atom" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <title type="text" />
  <id>uuid:24fb7e2e-6e27-4fb1-b02d-ffa720a647e2;id=1</id>
  <updated>{CurrentDateTime}</updated>
  <entry>
    <id>{Id}</id>
    <title type="text">{Name}</title>
    <summary type="text" xsi:nil="true" />
    <published>{CurrentDateTime}</published>
    <updated>{CurrentDateTime}</updated>
    <author>
      <name>Neptuo</name>
    </author>
    <content type="application/octet-stream" src="https://ci.appveyor.com/api/buildjobs/{JobId}/artifacts/{FileName}" />
    <Vsix xmlns="http://schemas.microsoft.com/developer/vsx-syndication-schema/2010" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
      <Id>{Id}</Id>
      <Version>{Version}</Version>
    </Vsix>
  </entry>
</feed>
```

It uses `{...}` tokens to be replaced by actual values. The `entry` element is a template that is used for each VSIX file. 

The publish script is quite long, but not so complicated.
It finds the `entry` template element in XML, iterates over *.vsix files and appends updated entry element to the document. Finally it pushes `Feed.xml` as AppVeyor artifact.

**[AppVeyor-PushArtifacts.ps1](https://github.com/neptuo/Productivity/blob/master/tools/AppVeyor-PushArtifacts.ps1)**

```powershell
param([string] $version = $env:APPVEYOR_BUILD_NUMBER, [string] $jobId = $env:APPVEYOR_JOB_ID)

if (-not($version)) 
{ 
    Throw "Parameter -Version is required";
}

if (-not($jobId)) 
{ 
    Throw "Parameter -JobId is required";
}

Push-Location $PSScriptRoot;
. ".\Vsix-GetId.ps1";

# Feed file paths.
$templatePath = Resolve-Path -Path "..\NightlyFeedTemplate.xml";
$outputPath = "..\NightlyFeed.xml";
Set-Content $outputPath "";
$outputPath = Resolve-Path -Path $outputPath;

$targetVersion = "0.0.$version";

$timestamp = Get-Date -Format o;
[xml]$xml = Get-Content $templatePath;

# XML namespaces.
$atomNs = New-Object System.Xml.XmlNamespaceManager $xml.NameTable;
$atomNs.AddNamespace("ns", "http://www.w3.org/2005/Atom") | Out-Null;
$vsixNs = New-Object System.Xml.XmlNamespaceManager $xml.NameTable;
$vsixNs.AddNamespace("vsix", "http://schemas.microsoft.com/developer/vsx-syndication-schema/2010") | Out-Null;

$xml.SelectSingleNode("//ns:updated", $atomNs).InnerText = $timestamp;

# Find entry template element and remove it from XML document.
# We need only for cloning, it shouldn't in the result feed.
$entryXml = $xml.SelectSingleNode("//ns:entry", $atomNs);
$xml.DocumentElement.RemoveChild($entryXml) | Out-Null;

foreach ($artifact in (Get-ChildItem ..\*.vsix -Recurse)) 
{
    # Publish VSIX as artifact.
    $artifactFileName = "$($artifact.BaseName)-v$($targetVersion)$($artifact.Extension)";
    Push-AppveyorArtifact $artifact.FullName -FileName $artifactFileName;

    # Find VSIX id in manifest file.
    $projectDirectory = $artifact.Directory.Parent.Parent;
    $manifestPath = Join-Path $projectDirectory.FullName "source.extension.vsixmanifest";
    $id = Vsix-GetId -Manifest $manifestPath;

    # Clone entry element.
    $artifactXml = $entryXml.Clone();

    # Modify entry XML.
    $artifactXml.SelectSingleNode("//ns:id", $atomNs).InnerText = $id;
    $artifactXml.SelectSingleNode("//ns:title", $atomNs).InnerText = $artifact.BaseName;
    $artifactXml.SelectSingleNode("//ns:published", $atomNs).InnerText = $timestamp;
    $artifactXml.SelectSingleNode("//ns:updated", $atomNs).InnerText = $timestamp;
    $url = $artifactXml.SelectSingleNode("//ns:content", $atomNs).Attributes["src"].Value;
    $url = $url.Replace("{FileName}", $artifactFileName);
    $url = $url.Replace("{JobId}", $jobId);
    $artifactXml.SelectSingleNode("//ns:content", $atomNs).Attributes["src"].Value = $url;
    $artifactXml.SelectSingleNode("//vsix:Id", $vsixNs).InnerText = $id;
    $artifactXml.SelectSingleNode("//vsix:Version", $vsixNs).InnerText = $targetVersion;

    # Append entry to the XML document.
    $xml.DocumentElement.AppendChild($artifactXml) | Out-Null;
}

# Save and publish Feed.xml as artifact.
$xml.Save($outputPath);
Push-AppveyorArtifact $outputPath -FileName "Feed.xml";

Pop-Location;
```

## Summary

Now you can grab `Feed.xml` and map it as a VSIX feed in Visual Studio.

![Visual Studio extension feed settings](/Content/Images/Blog/appveyor-vsix-feed/vs-feed-settings.png)

### Links
- [Repository on GitHub](https://github.com/neptuo/Productivity).
- [AppVeyor project](https://ci.appveyor.com/project/Neptuo/productivity).
- [AppVeyor generated feed](https://ci.appveyor.com/api/projects/neptuo/Productivity/artifacts/Feed.xml).
