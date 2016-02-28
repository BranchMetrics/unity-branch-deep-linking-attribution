using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

[CustomEditor(typeof(Branch))]
public class BranchEditor : Editor {

	public override void OnInspectorGUI() {
		GUI.changed = false;
		BranchData.Instance.branchUri = EditorGUILayout.TextField("branchUri", BranchData.Instance.branchUri, new GUILayoutOption[]{});
		BranchData.Instance.branchKey = EditorGUILayout.TextField("branchKey", BranchData.Instance.branchKey, new GUILayoutOption[]{});

		EditorGUILayout.BeginHorizontal(new GUILayoutOption[]{});
		if (GUILayout.Button("Update Android Manifest", new GUILayoutOption[]{})) {
			UpdateManifest();
			AssetDatabase.Refresh();
		}
		EditorGUILayout.EndHorizontal();

		if (GUI.changed) {
			AssetDatabase.SaveAssets();
			EditorUtility.SetDirty(BranchData.Instance);
			UpdateIOSKey();
		}
	}

	#region IOS Key update

	private void UpdateIOSKey() {
		string iosWrapperPath = Path.Combine(Application.dataPath, "Plugins/Branch/iOS/BranchiOSWrapper.mm");

		if (!File.Exists(iosWrapperPath)) {
			return;
		}

		StreamReader sr = new StreamReader(iosWrapperPath, Encoding.Default);
		string [] lines = sr.ReadToEnd().Split('\n').ToArray();
		sr.Close();

		StreamWriter sw = new StreamWriter(iosWrapperPath, false, Encoding.Default);
		foreach (string line in lines)
		{
			if (line.Contains("static NSString *_branchKey")) {
				sw.WriteLine("static NSString *_branchKey = @\"" + BranchData.Instance.branchKey + "\";");
			}
			else {
				sw.WriteLine(line);
			}
		}
		sw.Close();
	}

	#endregion

	#region Manifest update

	private void UpdateManifest() {
		
		string manifestFolder = Path.Combine(Application.dataPath, "Plugins/Android");
		string defaultManifestPath = Path.Combine(Application.dataPath, "Plugins/Branch/Android/AndroidManifest.xml");
		string manifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
		
		if (!File.Exists(manifestPath)) {
			
			if (!Directory.Exists(manifestFolder)) {
				Directory.CreateDirectory(manifestFolder);
			}
			
			File.Copy(defaultManifestPath, manifestPath);
		}
		
		// Opening android manifest
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(manifestPath);
		
		// Adding intent-filter into UnityPlayerActivity activity
		UpdateIntentFilter(xmlDoc);
		
		// Adding permissions
		UpdatePermissions(xmlDoc);
		
		// Saving android manifest
		xmlDoc.Save(manifestPath);

		//Changing "android___" to "android:" after changings
		TextReader manifestReader = new StreamReader(manifestPath);
		string content = manifestReader.ReadToEnd();
		manifestReader.Close();

		Regex regex = new Regex("android____");
		content = regex.Replace (content, "android:");

		TextWriter manifestWriter = new StreamWriter(manifestPath);
		manifestWriter.Write(content);
		manifestWriter.Close();
	}
	
	private void UpdateIntentFilter(XmlDocument doc) {
		XmlElement rootElem = doc.DocumentElement;
		XmlNode appNode = null;
		XmlNode unityActivityNode = null;
		XmlNode intentFilterNode = null;

		// finding node named "application"
		foreach(XmlNode node in rootElem.ChildNodes) {
			if (node.Name == "application") {
				appNode = node;
				break;
			}
		}

		if (appNode == null) {
			Debug.LogError("Current Android Manifest was broken, it does not contain \"<application>\" node");
			return;
		}

		// finding UnityPlayerActivity node
		foreach(XmlNode node in appNode.ChildNodes) {
			if (node.Name == "activity") {
				foreach(XmlAttribute attr in node.Attributes) {
					if (attr.Value.Contains("UnityPlayerActivity")) {
						unityActivityNode = node;
						break;
					}
				}
			}
		}

		if (unityActivityNode == null) {
			Debug.LogError("Current Android Manifest was broken, it does not contain \"<activity android:name=\"com.unity3d.player.UnityPlayerActivity\">\"");
			return;
		}

		// update or adding intent-filter
		foreach(XmlNode node in unityActivityNode.ChildNodes) {
			if (node.Name == "intent-filter") {
				foreach(XmlNode childNode in node.ChildNodes) {
					foreach(XmlAttribute attr in childNode.Attributes) {
						if (attr.Name.Contains("host") && attr.Value == "open") {
							intentFilterNode = node;
						}
					}
				}
			}
		}


		// <intent-filter>
		//	  <data android:scheme="APP_URI" android:host="open" />
		//	  <action android:name="android.intent.action.VIEW" />
		//	  <category android:name="android.intent.category.DEFAULT" />
		//	  <category android:name="android.intent.category.BROWSABLE" />
		// </intent-filter>

		if (intentFilterNode == null) {
			// adding intent-filter
			XmlElement ifElem = doc.CreateElement("intent-filter");

			XmlElement ifData = doc.CreateElement("data");
			ifData.SetAttribute("android____scheme", BranchData.Instance.branchUri);
			ifData.SetAttribute("android____host", "open");

			XmlElement ifAction = doc.CreateElement("action");
			ifAction.SetAttribute("android____name", "android.intent.action.VIEW");

			XmlElement ifCategory01 = doc.CreateElement("category");
			ifCategory01.SetAttribute("android____name", "android.intent.category.DEFAULT");

			XmlElement ifCategory02 = doc.CreateElement("category");
			ifCategory02.SetAttribute("android____name", "android.intent.category.BROWSABLE");

			ifElem.AppendChild(ifData);
			ifElem.AppendChild(ifAction);
			ifElem.AppendChild(ifCategory01);
			ifElem.AppendChild(ifCategory02);
			unityActivityNode.AppendChild(ifElem);
		}
		else {
			// changing intent-filter
			XmlElement ifData = doc.CreateElement("data");
			ifData.SetAttribute("android____scheme", BranchData.Instance.branchUri);
			ifData.SetAttribute("android____host", "open");

			foreach(XmlNode node in intentFilterNode.ChildNodes) {
				if (node.Name == "data") {
					intentFilterNode.ReplaceChild(ifData, node);
					break;
				}
			}
		}
	}
	
	private void UpdatePermissions(XmlDocument doc) {
		// we have to add the next permissions:
		// <uses-permission android:name="android.permission.INTERNET" />
		// <uses-permission android:name="android.permission.READ_PHONE_STATE" />
		// <uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />

		bool isInternetPermission = false;
		bool isReadPhoneState = false;
		bool isAccessNetworkState = false;

		// finding permissions nodes
		XmlElement rootElem = doc.DocumentElement;

		foreach(XmlNode node in rootElem.ChildNodes) {
			if (node.Name == "uses-permission") {
				foreach(XmlAttribute attr in node.Attributes) {
					if (attr.Value.Contains("android.permission.INTERNET")) {
						isInternetPermission = true;
					}
					else if (attr.Value.Contains("android.permission.READ_PHONE_STATE")) {
						isReadPhoneState = true;
					}
					else if (attr.Value.Contains("android.permission.ACCESS_NETWORK_STATE")) {
						isAccessNetworkState = true;
					}
				}
			}
		}

		// adding permissions if need
		// we add "android____name" because there is some troubles to add android:name
		if (!isInternetPermission) {
			XmlElement elem = doc.CreateElement("uses-permission");
			elem.SetAttribute("android____name", "android.permission.INTERNET");
			rootElem.AppendChild(elem);
		}

		if (!isReadPhoneState) {
			XmlElement elem = doc.CreateElement("uses-permission");
			elem.SetAttribute("android____name", "android.permission.READ_PHONE_STATE");
			rootElem.AppendChild(elem);
		}

		if (!isAccessNetworkState) {
			XmlElement elem = doc.CreateElement("uses-permission");
			elem.SetAttribute("android____name", "android.permission.ACCESS_NETWORK_STATE");
			rootElem.AppendChild(elem);
		}
	}

	#endregion
}
