# GlobalParams
## What is it?
An assembly that defines an implementation of the [IWizard](http://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard(v=vs.100).aspx) interface which provides all templates in a multi-project template with access to the parameters of the top level template.

* You should probably be familiar with the concepts on these links.
   * [How to: Create Project Templates](https://msdn.microsoft.com/en-us/library/xkh1wxd8(v=vs.100).aspx)  
   * [Template Parameters](http://msdn.microsoft.com/en-us/library/eehb4faa(v=vs.100).aspx)  
   * [How to: Use Wizards with Project Templates](http://msdn.microsoft.com/en-us/library/ms185301(v=vs.100).aspx)  
   * [How to: Create Multi-Project Templates](http://msdn.microsoft.com/en-us/library/ms185308(v=vs.100).aspx)  

* To see some existing implementations you can take a look at the source for these projects.
   * [WixWPF](https://github.com/tpalacino/WixWPF)
   * [IsWiX](https://github.com/iswix-llc/IsWiX)

## How to use it?
1. Ensure that the GlobalParams assembly is deployed to the GAC on any system that will use the template
1. Modify all of the .vstemplate files in the multi-project template to include the `WizardExtension` element from this example

```xml
<VSTemplate Type="Project" Version="3.0.0" xmlns="http://schemas.microsoft.com/developer/vstemplate/2005">
  <!-- VSTemplate required elements would be defined here. -->
  <WizardExtension>
    <Assembly>GlobalParams, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ea5a5299819fb7c0</Assembly>
    <FullClassName>GlobalParams.WizardMPT</FullClassName>
  </WizardExtension>
</VSTemplate>
```
## How to use it with custom UI?
1. Ensure that the GlobalParams assembly is deployed to the GAC on any system that will use the template
1. Create a project that will define the wizard implementation
1. Add a reference to the GlobalParams assembly
1. Create a class and have it derive from `GlobalParams.WizardMPT`
1. Override the `OnBeforeRunStarted` method

   1. Invoke the custom UI component
   1. Return `true` if the top level template parameters need to be accessible to all templates or `false` if they do not.
1. Modify all of the .vstemplate files in the multi-project template to include a `WizardExtension` element that points to your wizard assembly and class.

## Documentation

### Overridable Methods

| Method | Summary |
|:------ |:------- |
|OnBeforeOpeningFile|Invoked when the method [BeforeOpeningFile](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.beforeopeningfile(v=vs.100).aspx) is called by from Visual Studio|
|OnProjectFinishedGenerating|Invoked when the method [ProjectFinishedGenerating](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.projectfinishedgenerating(v=vs.100).aspx) is called by from Visual Studio|
|OnProjectItemFinishedGenerating|Invoked when the method [ProjectItemFinishedGenerating](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.projectitemfinishedgenerating(v=vs.100).aspx) is called by from Visual Studio|
|OnRunFinished|Invoked when the method [RunFinished](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.runfinished(v=vs.100).aspx) is called by from Visual Studio|
|OnShouldAddProjectItem|Invoked when the method [ShouldAddProjectItem](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.shouldaddprojectitem(v=vs.100).aspx) is called by from Visual Studio|
|OnBeforeRunStarted|Invoked when the method [RunStarted](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.runstarted(v=vs.100).aspx) is called by from Visual Studio and before any modifications are made. If the returned Boolean value is false modifications will NOT be made.|
|OnAfterRunStarted|Invoked immediately prior to exiting the [RunStarted](https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.templatewizard.iwizard.runstarted(v=vs.100).aspx) method.|

### Parameter List
> IMPORTANT NOTE: **Template parameters are case-sensitive.**

| Project Parameter | Global Parameter | Description |
|:----------------- |:---------------- |:----------- |
| clrversion | globalclrversion | Current version of the common language runtime (CLR). |
| guid[1-10] | globalguid[1-100] | A Guid used to replace the project GUID in a project file. You can specify up to 100 unique GUIDs (e.g. $globalguid1$, $globalguid83$, etc.) |
| itemname | globalitemname | The name provided by the user in the Add New Item dialog box. |
| machinename | globalmachinename | The current computer name (for example, Computer01). |
| projectname | globalprojectname | The name provided by the user in the New Project dialog box.|
| registeredorganization | globalregisteredorganization | The registry key value from HKLM\Software\Microsoft\Windows NT\CurrentVersion\RegisteredOrganization. |
| rootnamespace | globalrootnamespace | The root namespace of the current project. This parameter is used to replace the namespace only in an item being added to a project. |
| safeprojectname | globalsafeprojectname | The name provided by the user in the New Project dialog box, with all unsafe characters and spaces removed. |
| time | globaltime | The current time in the format DD/MM/YYYY 00:00:00. |
| userdomain | globaluserdomain | The current user domain. |
| username | globalusername | The current user name. |
| webnamespace | globalwebnamespace | The name of the current Web site. This parameter is used in the Web form template to guarantee unique class names. If the Web site is at the root directory of the Web server, this template parameter resolves to the root directory of the Web Server. |
| year | globalyear | The current year in the format YYYY. |
