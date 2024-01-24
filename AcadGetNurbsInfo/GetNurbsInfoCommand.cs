using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Text;
using NurbSurface = Autodesk.AutoCAD.DatabaseServices.NurbSurface;

[assembly: CommandClass(typeof(AcadGetNurbsInfo.GetNurbsInfoCommand))]
namespace AcadGetNurbsInfo
{

    public class GetNurbsInfoCommand
    {
        private string KnotsToString(KnotCollection knots)
        {
            StringBuilder sb = new StringBuilder();
            foreach (double knot in knots)
            {
                sb.Append($"{knot} ");
            }
            return sb.ToString();
        }

        private string KnotsToString(DoubleCollection knots)
        {
            StringBuilder sb = new StringBuilder();
            foreach (double knot in knots)
            {
                sb.Append($"{knot} ");
            }
            return sb.ToString();
        }


        [CommandMethod(nameof(GetNurbsInfo), CommandFlags.Modal | CommandFlags.UsePickSet)]
        public void GetNurbsInfo()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;
            Editor ed = doc.Editor;
            PromptSelectionResult result = ed.GetSelection();
            if (result.Status != PromptStatus.OK) return;
            using (Transaction tr = doc.TransactionManager.StartTransaction())
            {
                foreach (ObjectId id in result.Value.GetObjectIds())
                {
                    Entity ent = tr.GetObject(id, OpenMode.ForRead) as Entity;
                    if (ent == null) continue;
                    if (ent is NurbSurface surface)
                    {
                        ed.WriteMessage("\nNurbs surface info:\n");
                        ed.WriteMessage($"DegreeU: {surface.DegreeInU}\n");
                        ed.WriteMessage($"DegreeV: {surface.DegreeInV}\n");
                        ed.WriteMessage($"NumberOfControlPointsInU: {surface.NumberOfControlPointsInU}\n");
                        ed.WriteMessage($"NumberOfControlPointsInV: {surface.NumberOfControlPointsInV}\n");
                        ed.WriteMessage($"NumberOfKnotsInU: {surface.NumberOfKnotsInU}\n");
                        ed.WriteMessage($"NumberOfKnotsInV: {surface.NumberOfKnotsInV}\n");
                        ed.WriteMessage($"UKnots: {KnotsToString(surface.UKnots)}\n");
                        ed.WriteMessage($"VKnots: {KnotsToString(surface.VKnots)}\n");
                        for (int v = 0; v < surface.NumberOfControlPointsInV; ++v)
                        {
                            for (int u = 0; u < surface.NumberOfControlPointsInU; ++u)
                            {
                                var point = surface.GetControlPointAt(u, v);
                                var weight = surface.GetWeight(u, v);
                                ed.WriteMessage($"({point.X}, {point.Y}, {point.Z}, {weight}) ");
                            }
                            ed.WriteMessage("\n");
                        }
                    }
                    if (ent is Spline spline)
                    {
                        NurbsData data = spline.NurbsData;
                        var controlPoints = data.GetControlPoints();
                        var weights = data.GetWeights();
                        ed.WriteMessage("\nSpline nurbs info:\n");
                        ed.WriteMessage($"Degree: {data.Degree}\n");
                        ed.WriteMessage($"NumControlPoints: {controlPoints.Count}\n");
                        ed.WriteMessage($"Knots: {KnotsToString(data.GetKnots())}\n");
                        for (int u = 0; u < controlPoints.Count; ++u)
                        {
                            var point = controlPoints[u];
                            var weight = (weights.Count > u) ? weights[u] : 0;
                            ed.WriteMessage($"({point.X}, {point.Y}, {point.Z}, {weight}) ");
                        }

                    }
                }
                tr.Commit();
            }
        }
    }
}