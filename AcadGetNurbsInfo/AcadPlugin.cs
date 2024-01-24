using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;

[assembly: ExtensionApplication(typeof(AcadGetNurbsInfo.GetNurbsInfoPlugin))]
namespace AcadGetNurbsInfo
{
    public class GetNurbsInfoPlugin : IExtensionApplication
    {
        void IExtensionApplication.Initialize()
        {
            // Initialize your plug-in application here
        }

        void IExtensionApplication.Terminate()
        {
            // Do plug-in application clean up here
        }
    }
}