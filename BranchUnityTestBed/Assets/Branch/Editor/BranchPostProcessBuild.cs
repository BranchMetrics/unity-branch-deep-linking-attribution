using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class BranchPostProcessBuild {

	[PostProcessBuild(900)]
	public static void ChangeBranchBuiltProject(BuildTarget buildTarget, string pathToBuiltProject) {
		if ( buildTarget == BuildTarget.iOS ) {
			ChangeXcodePlist(pathToBuiltProject);
			ChangeXcodeProject(pathToBuiltProject);
		}
	}

	public static void ChangeXcodePlist(string pathToBuiltProject) {
		// Get plist
		string plistPath = pathToBuiltProject + "/Info.plist";
		PlistDocument plist = new PlistDocument();
		plist.ReadFromString(File.ReadAllText(plistPath));
		
		// Get root
		PlistElementDict rootDict = plist.root;
		PlistElementArray urlTypesArray = null;
		PlistElementDict  urlTypesItems = null;
		PlistElementArray urlSchemesArray = null;

		if (!rootDict.values.ContainsKey("CFBundleURLTypes")) {
			urlTypesArray = rootDict.CreateArray("CFBundleURLTypes");
		}
		else {
			urlTypesArray = rootDict.values["CFBundleURLTypes"].AsArray();

			if (urlTypesArray == null) {
				urlTypesArray = rootDict.CreateArray("CFBundleURLTypes");
			}
		}

		if (urlTypesArray.values.Count == 0) {
			urlTypesItems = urlTypesArray.AddDict();
		}
		else {
			urlTypesItems = urlTypesArray.values[0].AsDict();

			if (urlTypesItems == null) {
				urlTypesItems = urlTypesArray.AddDict();
			}
		}

		if (!urlTypesItems.values.ContainsKey("CFBundleURLSchemes")) {
			urlSchemesArray = urlTypesItems.CreateArray("CFBundleURLSchemes");
		}
		else {
			urlSchemesArray = urlTypesItems.values["CFBundleURLSchemes"].AsArray();

			if (urlSchemesArray == null) {
				urlSchemesArray = urlTypesItems.CreateArray("CFBundleURLSchemes");
			}
		}

		bool isExist = false;
		foreach(PlistElement elem in urlSchemesArray.values) {
			if (elem.AsString() != null && elem.AsString().Equals(BranchData.Instance.branchUri)) {
				isExist = true;
				break;
			}
		}

		if (!isExist) {
			urlSchemesArray.AddString(BranchData.Instance.branchUri);
		}

		// Write to file
		File.WriteAllText(plistPath, plist.WriteToString());
	}

	public static void ChangeXcodeProject(string pathToBuiltProject) {
		// Get xcodeproj
		string pathToProject = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
		string[] lines = File.ReadAllLines(pathToProject);

		// We'll open/replace project.pbxproj for writing and iterate over the old
		// file in memory, copying the original file and inserting every extra we need.
		// Create new file and open it for read and write, if the file exists overwrite it.
		FileStream fileProject = new FileStream(pathToProject, FileMode.Create);
		fileProject.Close();

		// Will be used for writing
		StreamWriter fCurrentXcodeProjFile = new StreamWriter(pathToProject) ;

		// Write all lines to new file and enable objective C exceptions
		foreach (string line in lines) {
			
			if (line.StartsWith("\t\t\t\tGCC_ENABLE_OBJC_EXCEPTIONS") ) {
				fCurrentXcodeProjFile.Write("\t\t\t\tGCC_ENABLE_OBJC_EXCEPTIONS = YES;\n");
			}
			else {                          
				fCurrentXcodeProjFile.WriteLine(line);
			}
		}

		// Close file
		fCurrentXcodeProjFile.Close();

		// Add frameworks
		PBXProject proj = new PBXProject();
		proj.ReadFromString(File.ReadAllText(pathToProject));

		string target = proj.TargetGuidByName("Unity-iPhone");

		if (!proj.HasFramework("AdSupport.framework")) {
			proj.AddFrameworkToProject(target, "AdSupport.framework", false);
		}

		if (!proj.HasFramework("CoreTelephony.framework")) {
			proj.AddFrameworkToProject(target, "CoreTelephony.framework", false);
		}

		if (!proj.HasFramework("CoreSpotlight.framework")) {
			proj.AddFrameworkToProject(target, "CoreSpotlight.framework", false);
		}

		if (!proj.HasFramework("Security.framework")) {
			proj.AddFrameworkToProject(target, "Security.framework", false);
		}

		File.WriteAllText(pathToProject, proj.WriteToString());
	}
}
