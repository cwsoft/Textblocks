# MS Office Primary Interop Assemblies (PIA)
Textblocks makes use of MS Office primary interop assemblies (PIA) to connect with the locally installed MS Word application. The required PIAs are automatically installed on your machine with `MS Visual Studio` when selecting the `Office-/SharePoint` workload. The Dlls are typically stored in `C:\Program Files (x86)\Microsoft Visual Studio\Shared\Visual Studio Tools for Office\PIA`.

## Microsoft.Office.Interop.Word.dll
If you want to compile Textblocks on your own, you need to copy the `Microsoft.Office.Interop.Word.dll` from your machine into the `Textblocks/PIA` folder, or adjust the include path in the `Textblocks.csproj` file accordingly.

Textblocks was developed and tested with the Office 15 Dlls, but should work with Office 14 too. 

| Office | Dll | Version | Date |
| --- | --- | --- | --- |
| 14 | Microsoft.Office.Interop.Word.dll | 14.0.4762.1000 | 2015-03-31 |
| 15 | Microsoft.Office.Interop.Word.dll | 15.0.4420.1017 | 2016-12-28 |

Further details can be found on the official Microsoft support pages:

- [How to install Office PIA](https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-install-office-primary-interop-assemblies?view=vs-2022)
- [Office PIA](https://docs.microsoft.com/en-us/visualstudio/vsto/office-primary-interop-assemblies?view=vs-2022)

## Are there NuGet packages available
There is a [NuGet package](https://www.nuget.org/packages/Microsoft.Office.Interop.Word) available. However Textblocks won't compile with this NuGet package without code modifications as the interop types are not automatically embeded into the final solution. However one could install the package and copy the `Microsoft.Office.Interop.Word.dll` from the NuGet user folder into the `Textblocks/PIA` folder and then uninstall the NuGet package and compile the solution.

The recommended way is to just copy the `Microsoft.Office.Interop.Word.dll` provided by Microsoft when installing the `Office workload` via the `MS Visual Studio Installer`.
