using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class BranchDemo : MonoBehaviour {
	
	public InputField inputShortLink;
	public Text lblCreditsValue;
	public Text lblInstallCountValue;
	public Text lblBuyCountValue;
	public Text lblIdentifyUser;
	public GameObject mainPanel;
	public GameObject creditsHistoryPanel;
	public GameObject referralCodePanel;

	void Start()
	{
		//init debug mode
		Branch.setDebug();

		//init Branch
		Branch.initSession( (parameters, error) => {
			if (error != null)
			{
				Debug.LogError("Branch Error: " + error);
			}
			else
			{
				Debug.Log("Branch initialization completed:");
				foreach (var key in parameters.Keys)
				{
					Debug.Log("key: " + key.ToString() + " = " + parameters[key].ToString());
				}
			}
		});
	}

	#region MainPanel

	public void OnBtn_RefreshShortUrl()
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		parameters.Add("name", "test name");
		parameters.Add("message", "hello there with short url");
		parameters.Add("$og_title", "this is a title");
		parameters.Add("$og_description", "this is a description");
		parameters.Add("$og_image_url", "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png");

		List<string> tags = new List<string>();
		tags.Add("tag1");
		tags.Add("tag2");

		Branch.getShortURLWithTags(parameters, tags, "channel1", "feature1", "1", (url, error) => {
			if (error != null)
			{
				Debug.LogError("Branch.getShortURL failed: " + error);
			}
			else
			{
				Debug.Log("Branch.getShortURL url: " + url);
				inputShortLink.text = url;
			}
		});
	}


	public void OnBtn_RefreshCounts()
	{
		lblInstallCountValue.text = "updating...";
		lblBuyCountValue.text = "updating...";

		Branch.loadActionCounts( (changed, error) => {
			if (error != null)
			{
				Debug.LogError("Branch.loadActionCounts failed: " + error);
				lblInstallCountValue.text = "error";
				lblBuyCountValue.text = "error";
			}
			else
			{
				Debug.Log("Branch.loadActionCounts changed: " + changed);

				lblInstallCountValue.text = "install total - " + Branch.getTotalCountsForAction("install").ToString() + ", unique - " + Branch.getUniqueCountsForAction("install").ToString();
				lblBuyCountValue.text = "buy total - " + Branch.getTotalCountsForAction("buy").ToString() + ", unique - " + Branch.getUniqueCountsForAction("buy").ToString();
			}
		});
	}


	public void OnBtn_RefreshRewards()
	{
		lblCreditsValue.text = "updating...";

		Branch.loadRewards( (changed, error) => {
			if (error != null)
			{
				Debug.LogError("Branch.loadRewards failed: " + error);
				lblCreditsValue.text = "error";
			}
			else
			{
				Debug.Log("Branch.loadRewards changed: " + changed);
				
				lblCreditsValue.text = Branch.getCredits().ToString();
			}
		});
	}
	
	
	public void OnBtn_Redeem5()
	{
		lblCreditsValue.text = "updating...";
		Branch.redeemRewards(5);
		OnBtn_RefreshRewards();
	}
	
	
	public void OnBtn_ExecuteBuy()
	{
		Branch.userCompletedAction("buy");
		OnBtn_RefreshCounts();
	}


	public void OnBtn_IdentifyUser()
	{
		lblIdentifyUser.text = "Identify User";

		Branch.setIdentity("test_user_10", (parameters, error) => {
			if (error != null)
			{
				Debug.LogError("Branch.setIdentity failed: " + error);
			}
			else
			{
				Debug.Log("Branch.setIdentity install params: " + parameters.ToString());
				lblIdentifyUser.text = "test_user_10";
			}
		});
	}


	public void OnBtn_ClearUser()
	{
		Branch.logout();

		lblCreditsValue.text = "";
		lblInstallCountValue.text = "";
		lblBuyCountValue.text = "";
		lblIdentifyUser.text = "Identify User";
	}


	public void OnBtn_PrintInstallParam()
	{
		Dictionary<string, object> parameters = Branch.getFirstReferringParams();
		Debug.Log("Install params: " + parameters.ToString());
	}


	public void OnBtn_BuyWithMetadata()
	{
		Dictionary<string, object> parameters = new Dictionary<string, object>();
		parameters.Add("name", "Alex");
		parameters.Add("boolean", true);
		parameters.Add("int", 1);
		parameters.Add("double", 0.13415512301);

		Branch.userCompletedAction("buy", parameters);
		OnBtn_RefreshCounts();
	}


	public void OnBtn_ShareLink()
	{
		try
		{
			Dictionary<string, object> shareParameters = new Dictionary<string, object>();
			shareParameters.Add("name", "test name");
			shareParameters.Add("auto_deeplink_key_1", "This is an auto deep linked value");
			shareParameters.Add("message", "hello there with short url");
			shareParameters.Add("$og_title", "this is new sharing title");
			shareParameters.Add("$og_description", "this is new sharing description");
			shareParameters.Add("$og_image_url", "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png");

			List<string> tags = new List<string>();
			tags.Add("tag1");
			tags.Add("tag2");

			Branch.shareLink(shareParameters, tags, "Test message", "feature1", "1", "https://play.google.com/store/apps/details?id=com.kindred.android", (parameters, error) => {
				if (error != null)
				{
					Debug.LogError("Branch.shareLink failed: " + error);
				}
				else
				{
					Debug.Log("Branch.shareLink shared params: " + parameters.ToString());
				}
			});
		}
		catch(Exception e)
		{
			Debug.Log(e);
		}
	}


	public void OnBtn_GetCreditHistory()
	{
		creditsHistoryPanel.SetActive(true);
		mainPanel.SetActive(false);
	}
	
	
	public void OnBtn_ReferralCode()
	{
		referralCodePanel.SetActive(true);
		mainPanel.SetActive(false);
	}

	#endregion


	#region CreditsHistoryPanel

	public void OnBtn_CreditsHistoryPanel_Back()
	{
		creditsHistoryPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

	#endregion


	#region ReferralCodePanel
	
	public void OnBtn_ReferralCodePanel_Back()
	{
		referralCodePanel.SetActive(false);
		mainPanel.SetActive(true);
	}
	
	#endregion
}
