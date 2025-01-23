﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class BranchDemo : MonoBehaviour {
	
	public InputField inputShortLink;
	public Text lblSetUserID;
	public Text lblLog;
	public GameObject mainPanel;
	public GameObject rewardsHistoryPanel;
	public GameObject logPanel;
    public GameObject quitButton;
	public RawImage qrCodeRawImage;

	private BranchUniversalObject universalObject = null;
	private BranchLinkProperties linkProperties = null;
	private BranchQRCode qrCode = null;
	private string logString = "";

	#region Init

	void Awake() {
		Application.logMessageReceived += OnLogMessage;
	}

	void Start() {

        #if UNITY_IOS
        quitButton.SetActive(false);
        #endif

        // set debug if need
        if (BranchData.Instance.enableLogging) {
			Branch.enableLogging();
		}

		// disable tracking of analytics for the user
		Branch.setTrackingDisabled(false);

        //init Branch with Dictionary
        //Branch.initSession(CallbackWithParams);

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
			universalObject.canonicalUrl = "https://branch.io";
			universalObject.title = "id12345 title";
			universalObject.contentDescription = "My awesome piece of content!";
			universalObject.imageUrl = "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png";
			universalObject.contentIndexMode = 0;
			universalObject.localIndexMode = 0;
			universalObject.expirationDate = new DateTime(2020, 12, 30);
			universalObject.keywords.Add("keyword01");
			universalObject.keywords.Add("keyword02");

			universalObject.metadata.contentSchema = BranchContentSchema.COMMERCE_BUSINESS;
			universalObject.metadata.quantity = 100f;
			universalObject.metadata.price = 1000f;
			universalObject.metadata.currencyType = BranchCurrencyType.USD;
			universalObject.metadata.sku = "my_sku";
			universalObject.metadata.productName = "my_product_name";
			universalObject.metadata.productBrand = "my_product_brand";
			universalObject.metadata.productCategory = BranchProductCategory.BUSINESS_AND_INDUSTRIAL;
			universalObject.metadata.condition = BranchCondition.EXCELLENT;
			universalObject.metadata.productVariant = "my_product_variant";

			universalObject.metadata.setAddress("my_street", "my_city", "my_region", "my_comuntry", "my_postal_code");
			universalObject.metadata.setLocation(40.0f, 40.0f);
			universalObject.metadata.setRating(4.0f, 5.0f, 10);

			universalObject.metadata.AddImageCaption("https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png");
			universalObject.metadata.AddCustomMetadata("foo", "bar");


			// register a view to add to the index
			Branch.registerView(universalObject);

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

	public void OnBtn_GenerateQRCode()
	{
		try
		{
			universalObject = new BranchUniversalObject();
			universalObject.canonicalIdentifier = "qrcodeid12345";
			linkProperties = new BranchLinkProperties();
			qrCode = new BranchQRCode("#FF0000", "#00FF00", 2, 1024, BranchImageFormat.JPEG, "https://play-lh.googleusercontent.com/gJ22vsKfh-dU592AI9GzI4OX9dkyzYPlsGSyr019dQv6cyAvfuRkUtzl9KJADGdTIlQ");
			Branch.generateQRCode(universalObject, linkProperties, qrCode, (data, error) =>
			{
				if (error != null)
				{
					Debug.LogError("Branch.generateQRCode failed: " + error);
				}
				else
				{
					Debug.Log("QR Code Successfully Generated!");
					byte[] decodedBytes = Convert.FromBase64String(data);
					Texture2D texture2D = new Texture2D(1, 1);
					texture2D.LoadImage(decodedBytes);
					texture2D.Apply();
					qrCodeRawImage.texture = texture2D;
				}
			});
		}
		catch (Exception e)
		{
			Debug.Log("QR Code Generating Error" + e);
		}
    }

	public void OnBtn_ValidateSDKIntegration() {
		Branch.validate();
	}

	public void OnBtn_SendBuyEvent() {
        BranchEvent e = new BranchEvent (BranchEventType.PURCHASE);

        e.SetAlias("my_alias");
        e.SetAffiliation("my_affilation");
        e.SetCoupon("my_coupon");
        e.SetCurrency(BranchCurrencyType.USD);
        e.SetTax(10.0f);
        e.SetRevenue(100.0f);
        e.SetShipping(1000.0f);
        e.SetDescription("my_description");
        e.SetSearchQuery("my_search_query");

        Branch.sendEvent(e);

	}

	public void OnBtn_SetUserID() {
		Branch.setIdentity("test_user_10", (parameters, error) => {
			if (error != null) {
				Debug.LogError("Branch.setIdentity failed: " + error);
				lblSetUserID.text = "Set User ID";
			} else {
				Debug.Log("Branch.setIdentity install params: " + parameters.ToString());
				lblSetUserID.text = "test_user_10";
			}
		});
	}

	public void OnBtn_SimualteLogout() {
		Branch.logout();
		lblSetUserID.text = "Set User ID";
	}

	public void OnBtn_SendComplexEvent() {
        BranchEvent e = new BranchEvent("MY_COMPLEX_EVENT");

        e.SetAlias("my_alias");
        e.SetAffiliation("my_affilation");
        e.SetCoupon("my_coupon");
        e.SetCurrency(BranchCurrencyType.USD);
        e.SetTax(10.0f);
        e.SetRevenue(100.0f);
        e.SetShipping(1000.0f);
        e.SetDescription("my_description");
        e.SetSearchQuery("my_search_query");
        e.AddCustomData("custom_data_key01", "custom_data_value01");
        e.AddCustomData("custom_data_key02", "custom_data_value02");

        if (universalObject != null)
            e.AddContentItem(universalObject);

        Branch.sendEvent(e);

	}

	public void OnBtn_ShareLink() {
		try {
			if (universalObject == null) {
				universalObject = new BranchUniversalObject();
				universalObject.canonicalIdentifier = "id12345";
				universalObject.canonicalUrl = "https://branch.io";
				universalObject.title = "id12345 title";
				universalObject.contentDescription = "My awesome piece of content!";
				universalObject.imageUrl = "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png";
				universalObject.contentIndexMode = 0;
				universalObject.localIndexMode = 0;
				universalObject.expirationDate = new DateTime(2020, 12, 30);
				universalObject.keywords.Add("keyword01");
				universalObject.keywords.Add("keyword02");

				universalObject.metadata.contentSchema = BranchContentSchema.COMMERCE_BUSINESS;
				universalObject.metadata.quantity = 100f;
				universalObject.metadata.price = 1000f;
				universalObject.metadata.currencyType = BranchCurrencyType.USD;
				universalObject.metadata.sku = "my_sku";
				universalObject.metadata.productName = "my_product_name";
				universalObject.metadata.productBrand = "my_product_brand";
				universalObject.metadata.productCategory = BranchProductCategory.BUSINESS_AND_INDUSTRIAL;
				universalObject.metadata.condition = BranchCondition.EXCELLENT;
				universalObject.metadata.productVariant = "my_product_variant";

				universalObject.metadata.setAddress("my_street", "my_city", "my_region", "my_comuntry", "my_postal_code");
				universalObject.metadata.setLocation(40.0f, 40.0f);
				universalObject.metadata.setRating(4.0f, 5.0f, 10);

				universalObject.metadata.AddImageCaption("https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png");
				universalObject.metadata.AddCustomMetadata("foo", "bar");

				// register a view to add to the index
				Branch.registerView(universalObject);
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
				} else {
                    Debug.Log("Branch.shareLink success: ");

                    if (parameters != null) {
                        foreach (string key in parameters.Keys) {
                            Debug.Log(key + "=" + parameters[key].ToString());
                        }
                    }
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
			universalObject.metadata.AddCustomMetadata("deeplink_text", "This link was generated for Spotlight registration");
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

	public void OnBtn_Quit() {
		Application.Quit ();
	}
}
