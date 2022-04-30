# MS Office Primary Interop Assemblies (PIA)
Textblocks requires the MS Office primary interop assemblies for MS Word to connect with your local MS Word application. The PIAs are normally installed on your local machine when installing MS Office or MS Visual Studio on your machine.

## Microsoft.Office.Interop.Word.dll
If you want to compile Textblocks on your own, please copy the `Microsoft.Office.Interop.Word.dll` from your machine into the `Textblocks/PIA` folder or adjust the include path in the `Textblocks.csproj` file accordingly.

Details about the typicall file location of the DLL or details about how to install them your own can be found in the following Microsoft links:

- [How to install Office PIA](https://docs.microsoft.com/en-us/visualstudio/vsto/how-to-install-office-primary-interop-assemblies?view=vs-2022)
- [Office PIA](https://docs.microsoft.com/en-us/visualstudio/vsto/office-primary-interop-assemblies?view=vs-2022)
