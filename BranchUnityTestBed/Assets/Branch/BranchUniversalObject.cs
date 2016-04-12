using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BranchUniversalObject {

	// Canonical identifier for the content referred. Normally the canonical path for your content in the app or web
	public string canonicalIdentifier;
	
	// Title for the content referred by BranchUniversalObject
	public string title;
	
	// Description for the content referred
	public string contentDescription;
	
	// An image url associated with the content referred
	public string imageUrl;
	
	// Meta data provided for the content. This meta data is used as the link parameters for links created from this object
	public Dictionary<String, String> metadata;
	
	// Mime type for the content referred
	public string type;
	
	// Content index mode: 0 - private mode, 1 - public mode
	public int contentIndexMode;
	
	// Any keyword associated with the content. Used for indexing
	public List<String> keywords;
	
	// Expiry date for the content and any associated links
	public DateTime? expirationDate = null;


	public BranchUniversalObject() {
		canonicalIdentifier = "";
		title = "";
		contentDescription = "";
		imageUrl = "";
		metadata = new Dictionary<string, string>();
		type = "";
		contentIndexMode = 0;
		keywords = new List<string>();
		expirationDate = null;
	}

	public BranchUniversalObject(string json) {
		canonicalIdentifier = "";
		title = "";
		contentDescription = "";
		imageUrl = "";
		metadata = new Dictionary<string, string>();
		type = "";
		contentIndexMode = 0;
		keywords = new List<string>();
		expirationDate = null;

		loadFromJson(json);
	}

	public BranchUniversalObject(Dictionary<string, object> data) {
		canonicalIdentifier = "";
		title = "";
		contentDescription = "";
		imageUrl = "";
		metadata = new Dictionary<string, string>();
		type = "";
		contentIndexMode = 0;
		keywords = new List<string>();
		expirationDate = null;
		
		loadFromDictionary(data);
	}

	public void loadFromJson(string json) {
		if (string.IsNullOrEmpty(json))
			return;

		var data = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
		loadFromDictionary(data);
	}

	public void loadFromDictionary(Dictionary<string, object> data) {
		if (data == null)
			return;
		
		if (data.ContainsKey("$canonical_identifier")) {
			canonicalIdentifier = data["$canonical_identifier"] as string;
		}
		if (data.ContainsKey("$og_title")) {
			title = data["$og_title"] as string;
		}
		if (data.ContainsKey("$og_description")) {
			contentDescription = data["$og_description"] as string;
		}
		if (data.ContainsKey("$og_image_url")) {
			imageUrl = data["$og_image_url"] as string;
		}
		if (data.ContainsKey("$content_type")) {
			type = data["$content_type"] as string;
		}
		if (data.ContainsKey("$publicly_indexable")) {
			if (!string.IsNullOrEmpty(data["$publicly_indexable"] as string)) {
				contentIndexMode = Convert.ToInt32(data["$publicly_indexable"] as string);
			}
		}
		if (data.ContainsKey("$exp_date")) {
			if (!string.IsNullOrEmpty(data["$exp_date"] as string)) {
				expirationDate = new DateTime(Convert.ToInt64(data["$exp_date"] as string) * 10000); // milliseconds to ticks
			}
		}
		if (data.ContainsKey("$keywords")) {
			if (data["$keywords"] != null) {
				List<object> keywordsTemp = data["$keywords"] as List<object>;

				if (keywordsTemp != null) {
					foreach(object obj in keywordsTemp) {
						keywords.Add(obj as string);
					}
				}
			}
		}
		if (data.ContainsKey("metadata")) {
			if (data["metadata"] != null) {
				Dictionary<string, object> metaTemp = data["metadata"] as Dictionary<string, object>;

				if (metaTemp != null) {
					foreach(string key in metaTemp.Keys) {
						metadata.Add(key, metaTemp[key] as string);
					}
				}
			}
		}
	}

	public string ToJsonString() {
		var data = new Dictionary<string, object>();

		data.Add("$canonical_identifier", canonicalIdentifier);
		data.Add("$og_title", title);
		data.Add("$og_description", contentDescription);
		data.Add("$og_image_url", imageUrl);
		data.Add("$content_type", type);
		data.Add("$publicly_indexable", contentIndexMode.ToString());
		data.Add("$exp_date", expirationDate.HasValue ? (expirationDate.Value.Ticks / 10000).ToString() : ""); // ticks to milliseconds
		data.Add("$keywords", keywords);
		data.Add("metadata", metadata);

		return MiniJSON.Json.Serialize(data);
	}
}
