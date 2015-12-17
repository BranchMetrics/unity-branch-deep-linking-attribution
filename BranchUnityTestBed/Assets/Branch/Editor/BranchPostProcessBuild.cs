using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using UnityEditor.iOS.Xcode;
using System.IO;

public class BranchPostProcessBuild {

	[PostProcessBuild(100)]
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

		// Is key "URL types" exist then delete
		if (rootDict.values.ContainsKey("URL types")) {
			rootDict.values.Remove("URL types");
		}

		// Add branchUri
		PlistElementArray urlTypesArray = rootDict.CreateArray("URL types");
		PlistElementDict  urlTypesItem01 = urlTypesArray.AddDict();
		PlistElementArray urlSchemesArray = urlTypesItem01.CreateArray("URL Schemes");
		urlSchemesArray.AddString(BranchData.Instance.branchUri);

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
