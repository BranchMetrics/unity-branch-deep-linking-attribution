#if UNITY_IOS

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

public class BranchPostProcessBuild
{

	[PostProcessBuild(900)]
	public static void ChangeBranchBuiltProject(BuildTarget buildTarget, string pathToBuiltProject) {
		if ( buildTarget == BuildTarget.iOS ) {
			UpdateXcodePlist(pathToBuiltProject);
			UpdateXcodeProject(pathToBuiltProject);
            UpdateEntitlements(pathToBuiltProject);
        }
	}

	private static void UpdateXcodePlist(string pathToBuiltProject) {
		string plistPath = pathToBuiltProject + "/Info.plist";
		PlistDocument plist = new PlistDocument();
		plist.ReadFromString(File.ReadAllText(plistPath));
		
		UpdateBranchKeys(plist);
		UpdateURIs(plist);

		// Write to file
		File.WriteAllText(plistPath, plist.WriteToString());
	}

	private static void UpdateBranchKeys(PlistDocument plist)
	{
		PlistElementDict rootDict = plist.root;
		PlistElementDict branchKeyDict = null;

		if (!rootDict.values.ContainsKey("branch_key")) {
			branchKeyDict = rootDict.CreateDict("branch_key");
		} else {
			branchKeyDict = rootDict.values["branch_key"].AsDict();
			if (branchKeyDict == null) {
				branchKeyDict = rootDict.CreateDict("branch_key");
			}
		}

		if (BranchData.Instance.liveBranchKey != null && BranchData.Instance.liveBranchKey.Length > 0) {
			branchKeyDict.SetString("live", BranchData.Instance.liveBranchKey);
		}

		if (BranchData.Instance.testBranchKey != null && BranchData.Instance.testBranchKey.Length > 0) {
			branchKeyDict.SetString("test", BranchData.Instance.testBranchKey);
		}
	}

	private static void UpdateURIs(PlistDocument plist) 
	{
		PlistElementDict rootDict = plist.root;
		PlistElementArray urlTypesArray = null;
		PlistElementDict  urlTypesDict = null;
		PlistElementArray urlSchemesArray = null;

		// URL types
		if (!rootDict.values.ContainsKey("CFBundleURLTypes")) {
			urlTypesArray = rootDict.CreateArray("CFBundleURLTypes");
		} else {
			urlTypesArray = rootDict.values["CFBundleURLTypes"].AsArray();
			if (urlTypesArray == null) {
				urlTypesArray = rootDict.CreateArray("CFBundleURLTypes");
			}
		}

		if (urlTypesArray.values.Count == 0) {
			urlTypesDict = urlTypesArray.AddDict();
		} else {
			urlTypesDict = urlTypesArray.values[0].AsDict();
			if (urlTypesDict == null) {
				urlTypesDict = urlTypesArray.AddDict();
			}
		}

		// URL Schemes
		if (!urlTypesDict.values.ContainsKey("CFBundleURLSchemes")) {
			urlSchemesArray = urlTypesDict.CreateArray("CFBundleURLSchemes");
		} else {
			urlSchemesArray = urlTypesDict.values["CFBundleURLSchemes"].AsArray();

			if (urlSchemesArray == null) {
				urlSchemesArray = urlTypesDict.CreateArray("CFBundleURLSchemes");
			}
		}

		// delete old branch URIs
		foreach(PlistElement elem in urlSchemesArray.values) {
			if (elem.AsString() != null && elem.AsString().Equals(BranchData.Instance.liveBranchUri)) {
				urlSchemesArray.values.Remove(elem);
				break;
			}
		}

		foreach(PlistElement elem in urlSchemesArray.values) {
			if (elem.AsString() != null && elem.AsString().Equals(BranchData.Instance.testBranchUri)) {
				urlSchemesArray.values.Remove(elem);
				break;
			}
		}

		// add branch URIs
		if (BranchData.Instance.testMode && !string.IsNullOrEmpty(BranchData.Instance.testBranchUri) ) {
			urlSchemesArray.AddString(BranchData.Instance.testBranchUri);
		}
		else if (!BranchData.Instance.testMode && !string.IsNullOrEmpty(BranchData.Instance.liveBranchUri)) {
			urlSchemesArray.AddString(BranchData.Instance.liveBranchUri);
		}
	}

    private static void UpdateEntitlements(string pathToBuiltProject)
    {
        string projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        string targetName = "Unity-iPhone";
        string entitlementsFileName = "branch_domains.entitlements";

        var entitlements = new ProjectCapabilityManager(projectPath, entitlementsFileName, targetName);

        entitlements.AddAssociatedDomains(BuildEntitlements());
        entitlements.WriteToFile();
    }

    private static string[] BuildEntitlements()
    {
        var links = BranchData.Instance.liveAppLinks;
        if (BranchData.Instance.testMode) {
            links = BranchData.Instance.testAppLinks;
		}

        if (links == null) {
            return null;
		}

        string[] domains = new string[links.Length];
        for (int i = 0; i < links.Length; i++)
        {
            domains[i] = "applinks:" + links[i];
        }

        return domains;
    }

    private static void UpdateXcodeProject(string pathToBuiltProject) {
		string pathToProject = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
		string[] lines = File.ReadAllLines(pathToProject);

		// We'll open/replace project.pbxproj for writing and iterate over the old
		// file in memory, copying the original file and inserting every extra we need.
		// Create new file and open it for read and write, if the file exists overwrite it.
		FileStream fileProject = new FileStream(pathToProject, FileMode.Create);
		fileProject.Close();

		StreamWriter fCurrentXcodeProjFile = new StreamWriter(pathToProject) ;

		foreach (string line in lines) {
			
			// The C# to ObjC code is bridged by C++ enable a few flags
			if (line.Contains("GCC_ENABLE_OBJC_EXCEPTIONS")) {
                fCurrentXcodeProjFile.Write("\t\t\t\tGCC_ENABLE_OBJC_EXCEPTIONS = YES;\n");
            }
            else if (line.Contains("GCC_ENABLE_CPP_EXCEPTIONS")) {
                fCurrentXcodeProjFile.Write("\t\t\t\tGCC_ENABLE_CPP_EXCEPTIONS = YES;\n");
            }
            else if (line.Contains("CLANG_ENABLE_MODULES")) {
				fCurrentXcodeProjFile.Write("\t\t\t\tCLANG_ENABLE_MODULES = YES;\n");
			}
			else if (line.Contains("ENABLE_BITCODE")) {
				// Apple deprecated bitcode, Unity still enables it as of March 2, 2023
				fCurrentXcodeProjFile.Write("\t\t\t\tENABLE_BITCODE = NO;\n");
			}
			else {                          
				fCurrentXcodeProjFile.WriteLine(line);
			}
		}
        fCurrentXcodeProjFile.Close();
	}
}
#endif
