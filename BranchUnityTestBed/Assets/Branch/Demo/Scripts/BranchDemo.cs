using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class BranchDemo : MonoBehaviour {
	
	public InputField inputShortLink;
	public Text lblRewardPoints;
	public Text lblSetUserID;
	public Text lblLog;
	public GameObject mainPanel;
	public GameObject rewardsHistoryPanel;
	public GameObject logPanel;

	private BranchUniversalObject universalObject = null;
	private BranchLinkProperties linkProperties = null;
	private string logString = "";

	#region Init

	void Awake() {
		Application.logMessageReceived += OnLogMessage;
	}

	void Start() {
		// set debug if need
		if (BranchData.Instance.simulateFreshInstalls) {
			Branch.setDebug();
		}

		//init Branch with Dictionary
//		Branch.initSession(CallbackWithParams);

		//init Branch with BUO
		Branch.initSession(CallbackWithBranchUniversalObject);
	}

	public void CallbackWithParams(Dictionary<string, object> parameters, string error) {
		if (error != null) {
			Debug.Log("Branch Error: " + error);
		}
		else {
			Debug.Log("Branch initialization completed: ");
			foreach(string str in parameters.Keys) {
				Debug.Log(str + " : " + parameters[str].ToString());
			}
		}
	}
	
	public void CallbackWithBranchUniversalObject(BranchUniversalObject universalObject, BranchLinkProperties linkProperties, string error) {
		if (error != null) {
			Debug.LogError("Branch Error: " + error);
		} else {
			Debug.Log("Branch initialization completed: ");

			Debug.Log("Universal Object: " + universalObject.ToJsonString());
			Debug.Log("Link Properties: " + linkProperties.ToJsonString());
		}
	}

	#endregion

	#region Utils

	private void OnLogMessage(string condition, string stackTrace, LogType type)
	{
		var typePrefix = type != LogType.Log ? type + ": " : "";
		logString += DateTime.Now.ToLongTimeString() + "> " + typePrefix + condition + "\n";
	}

	#endregion

	#region MainPanel

	public void OnBtn_CreateBranchLink() {
		try {
			universalObject = new BranchUniversalObject();
			universalObject.canonicalIdentifier = "id12345";
			universalObject.title = "id12345 title";
			universalObject.contentDescription = "My awesome piece of content!";
			universalObject.imageUrl = "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png";
			universalObject.metadata.Add("foo", "bar");

			linkProperties = new BranchLinkProperties();
			linkProperties.tags.Add("tag1");
			linkProperties.tags.Add("tag2");
			linkProperties.feature = "sharing";
			linkProperties.channel = "facebook";
			linkProperties.controlParams.Add("$desktop_url", "http://example.com");
			
			Branch.getShortURL(universalObject, linkProperties, (url, error) => {
				if (error != null) {
					Debug.LogError("Branch.getShortURL failed: " + error);
				} else {
					Debug.Log("Branch.getShortURL url: " + url);
					inputShortLink.text = url;
				}
			});
		} catch(Exception e) {
			Debug.Log(e);
		}
	}

	public void OnBtn_Redeem5() {
		Branch.redeemRewards(5);
		OnBtn_RefreshRewards();
	}

	public void OnBtn_RefreshRewards() {
		lblRewardPoints.text = "updating...";

		Branch.loadRewards( (changed, error) => {
			if (error != null) {
				Debug.LogError("Branch.loadRewards failed: " + error);
				lblRewardPoints.text = "error";
			} else {
				Debug.Log("Branch.loadRewards changed: " + changed);
				lblRewardPoints.text = Branch.getCredits().ToString();
			}
		});
	}

	public void OnBtn_SendBuyEvent() {
		Branch.userCompletedAction("buy");
		OnBtn_RefreshRewards();
	}

	public void OnBtn_ShowRewardsHistory() {
		rewardsHistoryPanel.SetActive(true);
		mainPanel.SetActive(false);
	}

	public void OnBtn_SetUserID() {
		Branch.setIdentity("test_user_10", (parameters, error) => {
			if (error != null) {
				Debug.LogError("Branch.setIdentity failed: " + error);
				lblSetUserID.text = "Set User ID";
			} else {
				Debug.Log("Branch.setIdentity install params: " + parameters.ToString());
				lblSetUserID.text = "test_user_10";
				OnBtn_RefreshRewards();
			}
		});
	}

	public void OnBtn_SimualteLogout() {
		Branch.logout();
		lblSetUserID.text = "Set User ID";
		lblRewardPoints.text = "";
	}

	public void OnBtn_SendComplexEvent() {
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		parameters.Add("name", "Alex");
		parameters.Add("boolean", true);
		parameters.Add("int", 1);
		parameters.Add("double", 0.13415512301);

		Branch.userCompletedAction("buy", parameters);
		OnBtn_RefreshRewards();
	}

	public void OnBtn_ShareLink() {
		try {
			if (universalObject == null) {
				universalObject = new BranchUniversalObject();
				universalObject.canonicalIdentifier = "id12345";
				universalObject.title = "id12345 title";
				universalObject.contentDescription = "My awesome piece of content!";
				universalObject.imageUrl = "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png";
				universalObject.metadata.Add("foo", "bar");
			}

			if (linkProperties == null) {
				linkProperties = new BranchLinkProperties();
				linkProperties.tags.Add("tag1");
				linkProperties.tags.Add("tag2");
				linkProperties.feature = "invite";
				linkProperties.channel = "Twitter";
				linkProperties.stage = "2";
				linkProperties.controlParams.Add("$desktop_url", "http://example.com");
			}

			Branch.shareLink(universalObject, linkProperties, "hello there with short url", (parameters, error) => {

				if (error != null) {
					Debug.LogError("Branch.shareLink failed: " + error);
				} else if (parameters != null) {
					Debug.Log("Branch.shareLink: " + parameters["sharedLink"].ToString() + " " + parameters["sharedChannel"].ToString());
				}
			});
		} catch(Exception e) {
			Debug.Log(e);
		}
	}

	public void OnBtn_ViewFirstReferringParams() {
		lblLog.text =  Branch.getFirstReferringBranchUniversalObject().ToJsonString() + "\n\n";
		lblLog.text += Branch.getFirstReferringBranchLinkProperties().ToJsonString();

		logPanel.SetActive(true);
		mainPanel.SetActive(false);
	}

	public void OnBtn_ViewLatestReferringParams() {
		lblLog.text =  Branch.getLatestReferringBranchUniversalObject().ToJsonString() + "\n\n";
		lblLog.text += Branch.getLatestReferringBranchLinkProperties().ToJsonString();

		logPanel.SetActive(true);
		mainPanel.SetActive(false);
	}

	public void OnBtn_SimulateContentAccess() {
		if (universalObject != null) {
			Branch.registerView(universalObject);
		}
	}

	public void OnBtn_RegisterForSpotlight() {
		if (universalObject != null) {
			universalObject.metadata.Add("deeplink_text", "This link was generated for Spotlight registration");
			Branch.listOnSpotlight(universalObject);
		}
	}

	public void OnBtn_LogPanel() {
		lblLog.text = logString;
		logPanel.SetActive(true);
		mainPanel.SetActive(false);
	}

	#endregion

	#region RewardsHistoryPanel

	public void OnBtn_RewardsHistoryPanel_Back() {
		rewardsHistoryPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

	#endregion

	#region LogPanel

	public void OnBtn_LogPanel_Back() {
		logPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

	#endregion
}
