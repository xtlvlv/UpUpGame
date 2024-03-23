using System;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections.Generic;
using System.Linq;


using UnityEngine;

namespace Unity.MSBuild
{
    public class UnityCSProject
    {

        public PropertyGroup PropertyDebug;
        public PropertyGroup PropertyRelease;

        public string ProductVersion;
        public string SchemaVersion;
        public string AssemblyName;
        public string TargetFrameworkVersion;
        public string FileAlignment;
        public string BaseDirectory;
        public string OutputType;
        public string ProjectGuid;

        public List<CompileItem> CompileItems;
        public List<ReferenceItem> ReferenceItems;
        public List<AnalyzerItem> AnalyzerItems;

        private XElement m_xproject = null;
        private string m_projPath;
        private string m_xnamespace;

        private UnityCSProject()
        {
        }

        public void WriteToFile()
        {
            var fmtstr = m_xproject.ToString();
            File.WriteAllText(m_projPath, fmtstr);
        }

        public void AddAnalyzer(AnalyzerItem item)
        {
            if (AnalyzerItems == null) AnalyzerItems = new List<AnalyzerItem>();
            AnalyzerItems.Add(item);

            var analyzer = new XElement(XName.Get("Analyzer", m_xnamespace), new XAttribute(XName.Get("Include"), item.Include));
            XElement itemgroup = new XElement(XName.Get("ItemGroup", m_xnamespace), analyzer);
            m_xproject.Add(itemgroup);
        }

        public static UnityCSProject Parse(string path)
        {

            if (string.IsNullOrEmpty(path)) return null;
            if (!path.ToLower().EndsWith(".csproj")) return null;
            if (!File.Exists(path)) return null;



            var xproject = XElement.Load(path);
            string xns = xproject.Name.NamespaceName;


            UnityCSProject csproj = new UnityCSProject();

            try
            {
                List<CompileItem> compileFiles = new List<CompileItem>();
                List<ReferenceItem> referenceItems = new List<ReferenceItem>();
                List<AnalyzerItem> analyzerItems = new List<AnalyzerItem>();
                var itemgroups = xproject.Elements(XName.Get("ItemGroup", xns));


                foreach (var itemgroup in itemgroups)
                {
                    var compiles = itemgroup.Elements(XName.Get("Compile", xns));
                    foreach (var compile in compiles)
                    {
                        var compilePath = compile.Attribute(XName.Get("Include"));
                        if (compilePath == null) continue;
                        compileFiles.Add(new CompileItem(compilePath.Value));
                    }

                    var references = itemgroup.Elements(XName.Get("Reference", xns));
                    foreach (var reference in references)
                    {
                        var attrName = reference.Attribute(XName.Get("Include")).Value;
                        var hintPath = reference.Element(XName.Get("HintPath", xns)).Value();
                        referenceItems.Add(new ReferenceItem(attrName, hintPath));
                    }

                    var analyzers = itemgroup.Elements(XName.Get("Analyzer", xns));

                    foreach (var analyzer in analyzers)
                    {
                        var analyzerPath = analyzer.Attribute(XName.Get("Include"));
                        if (analyzerPath == null) continue;
                        analyzerItems.Add(new AnalyzerItem(analyzerPath.Value));
                    }


                }

                csproj.CompileItems = compileFiles;
                csproj.ReferenceItems = referenceItems;
                csproj.AnalyzerItems = analyzerItems;

                //PropettyGroup

                var propertyGroups = xproject.Elements(XName.Get("PropertyGroup", xns));
                foreach (var propertyGroup in propertyGroups)
                {
                    var condition = propertyGroup.Attribute(XName.Get("Condition"));
                    if (condition == null)
                    {
                        //parse main config
                        csproj.AssemblyName = propertyGroup.Element(XName.Get("AssemblyName", xns)).Value();
                        csproj.BaseDirectory = propertyGroup.Element(XName.Get("BaseDirectory", xns)).Value();
                        csproj.FileAlignment = propertyGroup.Element(XName.Get("FileAlignment", xns)).Value();
                        csproj.OutputType = propertyGroup.Element(XName.Get("OutputType", xns)).Value();
                        csproj.ProductVersion = propertyGroup.Element(XName.Get("ProductVersion", xns)).Value();
                        csproj.ProjectGuid = propertyGroup.Element(XName.Get("ProjectGuid", xns)).Value();
                        csproj.SchemaVersion = propertyGroup.Element(XName.Get("SchemaVersion", xns)).Value();
                        csproj.TargetFrameworkVersion = propertyGroup.Element(XName.Get("TargetFrameworkVersion", xns)).Value();
                    }
                    else
                    {
                        var conditionDesc = condition.Value;
                        if (conditionDesc.Contains("Debug"))
                        {
                            csproj.PropertyDebug = new PropertyGroup(propertyGroup, xns);
                        }
                        else if (conditionDesc.Contains("Release"))
                        {
                            csproj.PropertyRelease = new PropertyGroup(propertyGroup, xns);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            csproj.m_projPath = path;
            csproj.m_xproject = xproject;
            csproj.m_xnamespace = xns;

            return csproj;
        }
    }

    public class CompileItem
    {
        public string Path;

        public CompileItem(string path)
        {
            this.Path = path;
        }
    }

    public class AnalyzerItem
    {
        public string Include;

        public AnalyzerItem(string path)
        {
            Include = path;
        }

    }


    public class PropertyGroup
    {
        public string DebugSymbols;
        public string DebugType;
        public string Optimize;
        public string OutputPath;
        public string DefineConstants;
        public string ErrorReport;
        public string WarningLevel;
        public string AllowUnsafeBlocks;
        public PropertyGroup(XElement property, string xns)
        {
            DebugSymbols = property.Element(XName.Get("DebugSymbols", xns)).Value();
            DebugType = property.Element(XName.Get("DebugType", xns)).Value();
            Optimize = property.Element(XName.Get("Optimize", xns)).Value();
            OutputPath = property.Element(XName.Get("OutputPath", xns)).Value();
            DefineConstants = property.Element(XName.Get("DefineConstants", xns)).Value();
            ErrorReport = property.Element(XName.Get("ErrorReport", xns)).Value();
            WarningLevel = property.Element(XName.Get("WarningLevel", xns)).Value();
            AllowUnsafeBlocks = property.Element(XName.Get("AllowUnsafeBlocks", xns)).Value();
        }
    }

    public class ReferenceItem
    {
        public string Name;
        public string HintPath;

        public ReferenceItem(string name, string hintpath)
        {
            this.Name = name;
            this.HintPath = hintpath;
        }
    }

    public static class XElementExtension
    {

        public static string Value(this XElement e)
        {
            if (e == null) return null;
            return e.Value;
        }
    }
}
