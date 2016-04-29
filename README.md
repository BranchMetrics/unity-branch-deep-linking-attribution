# Branch Metrics Unity SDK Reference

This is a repository of our open source Unity SDK, which is a wrapper on top of our iOS and Android SDKs. See the table of contents below for a complete list of the content featured in this document.

## Migration warning for 12/12/15 and after

We released a completely revamped version of the Unity package today which automates a lot of the complexity of integrating Please rip out the old SDK and replace it with the new one at your earliest convenience.

## Get the Demo App

There's a full demo app embedded in this repository, which you can find in the *BranchUnityTestBed* folder. Please use that as a reference.

## Additional Resources
- [Integration guide](https://dev.branch.io/recipes/add_the_sdk/unity/) *Start Here*
- [Changelog](https://github.com/BranchMetrics/Unity-Deferred-Deep-Linking-SDK/blob/master/ChangeLog.md)
- [Testing](https://dev.branch.io/recipes/testing_your_integration/unity/)
- [Support portal, FAQ](http://support.branch.io)

## Installation

#### Download the raw files

Download code from here: [S3 Package](https://s3-us-west-1.amazonaws.com/branchhost/BranchUnityWrapper.unitypackage)

#### Or just clone this project!

After acquiring the `BranchUnityWrapper.unityPackage` through one of these choices, you can import it into your project by clicking `Assets -> Import Package`.

### Register you app

You can sign up for your own app id at [https://dashboard.branch.io](https://dashboard.branch.io)

## Configuration (for tracking)

#### Unity Scene and Branch Parameters

To allow Branch to configure itself, you must add a BranchPrefab asset to your first scene. Simply drag into your first scene, and then specify your `APP_KEY`, `PATH_PREFIX` and `APP_URI` in the properties.

* `APP_KEY`: This is your Branch key from the dashboard
* `PATH_PREFIX`: This is your Branch android path prefux [Read more](https://github.com/BranchMetrics/Android-Deferred-Deep-Linking-SDK/blob/master/README.md#leverage-android-app-links-for-deep-linking)
* `APP_URI`: This is the URI scheme you would like to use to open the app. This must be the same value as you entered in [the Branch link settings](https://dashboard.branch.io/#/settings/link) as well. Please do *not* include the `://` characters.

![Branch Unity Config](https://raw.githubusercontent.com/BranchMetrics/Unity-Deferred-Deep-Linking-SDK/master/Docs/Screenshots/branch-key.png)

#### iOS Note

After building iOS project:

1. All required frameworks will be added automatically
2. Objective C exceptions will be enabled automatically
3. URL Scheme will be added into .plist automatically

#### iOS + Unity 4.6 Note

Branch requires ARC, and we don’t intend to add if checks thoughout the SDK to try to support pre-ARC. However, you can add flags to the project to compile the Branch files with ARC, which should work fine for you.

Simple add **-fobjc-arc** to all Branch files.

*Note:* *we already have added this flag, but check it before building.*

#### Android Note

Click button "Update Android Manifest" to change or add a android manifest for support deep linking, or you can change android manifest by your hands. 

#### Changing android manifest manually

In your project's manifest file, you can register your app to respond to direct deep links (yourapp:// in a mobile browser) by adding the second intent filter block. Also, make sure to change **yourapp** to a unique string that represents your app name.

Typically, you would register some sort of splash activitiy that handles routing for your app.

```xml
<activity
	android:name="com.yourapp.SplashActivity"
	android:label="@string/app_name" >
	<intent-filter>
		<action android:name="android.intent.action.MAIN" />
		<category android:name="android.intent.category.LAUNCHER" />
	</intent-filter>

	<!-- Add this intent filter below, and change yourapp to your app name -->
	<intent-filter>
		<data android:scheme="yourapp" android:host="open" />
		<action android:name="android.intent.action.VIEW" />
		<category android:name="android.intent.category.DEFAULT" />
		<category android:name="android.intent.category.BROWSABLE" />
	</intent-filter>
	
	<!-- App Link your activity to Branch links-->
	<intent-filter android:autoVerify="true">
        <data android:scheme="https" android:host="bnc.lt" android:pathPrefix="/prefix" />
        <action android:name="android.intent.action.VIEW" />
        <category android:name="android.intent.category.DEFAULT" />
        <category android:name="android.intent.category.BROWSABLE" />
    </intent-filter>
</activity>
```

### Initialize SDK, Session And Register Deep Link Routing Function

#### Initialize SDK

Branch SDK will be initilized automatically, just add BranchPrefab to your scene and set parameters.

We recommend to add BranchPrefab to your first scene, before any calling of Branch SDK API.

Don't worry about several instances of Branch SDK even if your first scene is scene that will be launched several times (for example: content loading scene).


#### Initialize Session And Register Deep Linking Routing Function

When you created a custom link with your own custom dictionary data, you probably want to know which data is sent to your app and then check that data. For example, if your app opens with some data, you want to route the user depending on the data you passed in. To catch sent data, you need to register a callback. Think of this callback as your "deep link router". Important note: your callback must be visible from all your scenes, if you plan to process data in each scene.

**Very important note**: You must call Branch.InitSession(...) at the start of your app (in Start of your first scene) else Branch has not time to registry callback and you will receive nothing. If you need to process deep linking parameters later (for example after loading all asset bundles or from specific scene of after showing start video etc.) then you can use two ways:

- you can use methods for retrieving install/open parameters (see below),
- you can use callback listener (simple realization of callback listener you can see in our demo app).

This deep link routing callback is called 100% of the time on init, with your link params or an empty dictionary if none present.

```csharp
using UnityEngine;
using System.Collections.Generic;

public class MyCoolBehaviorScript : MonoBehaviour {
    void Start () {
        Branch.initSession(CallbackWithParams);
    }
     
    void CallbackWithParams(Dictionary<string, object> parameters, string error) {
        if (error != null) {
            System.Console.WriteLine("Oh no, something went wrong: " + error);
        }
        else if (parameters.Count > 0) {
            System.Console.WriteLine("Branch initialization completed with the following params: " + parameters.Keys);
        }
    }
}
```

**Initalization with BranchUniversalObject and LinkProperties**

```csharp
using UnityEngine;
using System.Collections.Generic;

public class MyCoolBehaviorScript : MonoBehaviour {
    void Start () {
        Branch.initSession(CallbackWithBranchUniversalObject);
    }
    
    void CallbackWithBranchUniversalObject(BranchUniversalObject universalObject, BranchLinkProperties linkProperties, string error) {
        if (error != null) {
			Debug.LogError("Branch Error: " + error);
		} else {
			Debug.Log("Branch initialization completed: ");

			Debug.Log("Universal Object: " + universalObject.ToJsonString());
			Debug.Log("Link Properties: " + linkProperties.ToJsonString());
		}
    }
}
```

##### Return Parameters

- _Dictionary<string, object> params_ : These params will contain any data associated with the Branch link that was clicked before the app session began. There are a few keys which are always present:
  - '+is_first_session' Denotes whether this is the first session (install) or any other session (open)
  - '+clicked_branch_link' Denotes whether or not the user clicked a Branch link that triggered this session
- _string error_ : This error will be nil unless there is an error such as connectivity or otherwise. Check !error to confirm it was a valid link.
    - BNCServerProblemError There was an issue connecting to the Branch service
    - BNCBadRequestError The request was improperly formatted

Branch returns explicit parameters every time. Here is a list, and a description of what each represents.

* `~` denotes analytics
* `+` denotes information added by Branch
* (for the curious, `$` denotes reserved keywords used for controlling how the Branch service behaves)

| **Parameter** | **Meaning**
| --- | ---
| ~channel | The channel on which the link was shared, specified at link creation time
| ~feature | The feature, such as `invite` or `share`, specified at link creation time
| ~tags | Any tags, specified at link creation time
| ~campaign | The campaign the link is associated with, specified at link creation time
| ~stage | The stage, specified at link creation time
| ~creation_source | Where the link was created ('API', 'Dashboard', 'SDK', 'iOS SDK', 'Android SDK', or 'Web SDK')
| +match_guaranteed | True or false as to whether the match was made with 100% accuracy
| +referrer | The referrer for the link click, if a link was clicked
| +phone_number | The phone number of the user, if the user texted himself/herself the app
| +is_first_session | Denotes whether this is the first session (install) or any other session (open)
| +clicked_branch_link | Denotes whether or not the user clicked a Branch link that triggered this session
| +click_timestamp | Epoch timestamp of when the click occurred 

#### Retrieve session (install or open) parameters

These session parameters will be available at any point later on with this command. If no params, the dictionary will be empty. This refreshes with every new session (app installs AND app opens)

```csharp
Dictionary<string, object> sessionParams = Branch.getLatestReferringParams();
```

**Retrive parameters with BranchUniversalObject and LinkProperties**

```csharp
BranchUniversalObject obj = Branch.getLatestReferringBranchUniversalObject();
BranchLinkProperties link = Branch.getLatestReferringBranchLinkProperties();
```

#### Retrieve install (install only) parameters

If you ever want to access the original session params (the parameters passed in for the first install event only), you can use this line. This is useful if you only want to reward users who newly installed the app from a referral link or something.

```csharp
Dictionary<string, object> installParams = Branch.getFirstReferringParams();
```

**Retrive parameters with BranchUniversalObject and LinkProperties**

```csharp
BranchUniversalObject obj = Branch.getFirstReferringBranchUniversalObject();
BranchLinkProperties link = Branch.getFirstReferringBranchLinkProperties();
```

### Persistent identities

Often, you might have your own user IDs, or want referral and event data to persist across platforms or uninstall/reinstall. It's helpful if you know your users access your service from different devices. This where we introduce the concept of an 'identity'.

To identify a user, just call:

```csharp
Branch.setIdentity("your user id");
```

#### Logout

If you provide a logout function in your app, be sure to clear the user when the logout completes. This will ensure that all the stored parameters get cleared and all events are properly attributed to the right identity.

**Warning** this call will clear the referral credits and attribution on the device.

```csharp
Branch.logout();
```

### Register custom events

```csharp
Branch.userCompletedAction("your_custom_event"); // your custom event name should not exceed 63 characters
```

OR if you want to store some state with the event

```csharp
Dictionary<string, object> stateItems = new Dictionary<string, object>
{
	{ "username", "Joe" },
	{ "description", "Joe likes long walks on the beach..." }
};
Branch.userCompletedAction("your_custom_event", stateItems); // same 63 characters max limit
```

## Branch Universal Object (for deep links, content analytics and indexing)

As more methods have evolved in iOS, we've found that it was increasingly hard to manage them all. We abstracted as many as we could into the concept of a Branch Universal Object. This is the object that is associated with the thing you want to share (content or user). You can set all the metadata associated with the object and then call action methods on it to get a link or index in Spotlight.

### Branch Universal Object

#### Methods

```csharp
BranchUniversalObject universalObject = new BranchUniversalObject();
universalObject.canonicalIdentifier = "id12345";
universalObject.title = "id12345 title";
universalObject.contentDescription = "My awesome piece of content!";
universalObject.imageUrl = "https://s3-us-west-1.amazonaws.com/branchhost/mosaic_og.png";
universalObject.metadata.Add("foo", "bar");
```

#### Parameters

**canonicalIdentifier**: This is the unique identifier for content that will help Branch dedupe across many instances of the same thing. If you have a website with pathing, feel free to use that. Or if you have database identifiers for entities, use those.

**title**: This is the name for the content and will automatically be used for the OG tags. It will insert $og_title into the data dictionary of any link created.

**contentDescription**: This is the description for the content and will automatically be used for the OG tags. It will insert $og_description into the data dictionary of any link created.

**imageUrl**: This is the image URL for the content and will automatically be used for the OG tags. It will insert $og_image_url into the data dictionary of any link created.

**metadata**: These are any extra parameters you'd like to associate with the Branch Universal Object. These will be made available to you after the user clicks the link and opens up the app. To add more keys/values, just use the method `addMetadataKey`.

**type**: This is a label for the type of content present. Apple recommends that you use uniform type identifier as [described here](https://developer.apple.com/library/prerelease/ios/documentation/MobileCoreServices/Reference/UTTypeRef/index.html). Currently, this is only used for Spotlight indexing but will be used by Branch in the future.

**contentIndexMode**: Can be set to the ENUM of either `ContentIndexModePublic` or `ContentIndexModePrivate`. Public indicates that you'd like this content to be discovered by other apps. Currently, this is only used for Spotlight indexing but will be used by Branch in the future.

**keywords**: Keywords for which this content should be discovered by. Just assign an array of strings with the keywords you'd like to use. Currently, this is only used for Spotlight indexing but will be used by Branch in the future.

**expirationDate**: The date when the content will not longer be available or valid. Currently, this is only used for Spotlight indexing but will be used by Branch in the future.

#### Returns

None

### Register Views for Content Analytics

If you want to track how many times a user views a particular piece of content, you can call this method whenthe page loads to tell Branch that this content was viewed.

#### Methods

```csharp
Branch.registerView(universalObject);
```

#### Parameters

**BranchUniversalObject ***: A completed Branch Universal Object with all of the above fields filled out.

#### Returns

None

### Shortened Links

Once you've created your `Branch Universal Object`, which is the reference to the content you're interested in, you can then get a link back to it with the mechanisms described below.

#### Methods

```csharp
// Define properties of the Branch link
BranchLinkProperties linkProperties = new BranchLinkProperties();
linkProperties.tags.Add("tag1");
linkProperties.tags.Add("tag2");
linkProperties.feature = "invite";
linkProperties.channel = "Twitter";
linkProperties.stage = "2";
linkProperties.controlParams.Add("$desktop_url", "http://example.com");
```

```csharp
Branch.getShortURL(universalObject, linkProperties, (url, error) => {
    if (error != null) {
        Debug.LogError("Branch.getShortURL failed: " + error);
    } else {
        Debug.Log("Branch.getShortURL shared params: " + url);
    }
});
```

#### Link Properties Parameters

**channel**: The channel for the link. Examples could be Facebook, Twitter, SMS, etc., depending on where it will be shared.

**feature**: The feature the generated link will be associated with. Eg. `sharing`.

**controlParams**: A dictionary to use while building up the Branch link. Here is where you specify custom behavior controls as described in the table below.

You can do custom redirection by inserting the following _optional keys in the dictionary_:

| Key | Value
| --- | ---
| "$fallback_url" | Where to send the user for all platforms when app is not installed.
| "$desktop_url" | Where to send the user on a desktop or laptop. By default it is the Branch-hosted text-me service.
| "$android_url" | The replacement URL for the Play Store to send the user if they don't have the app. _Only necessary if you want a mobile web splash_.
| "$ios_url" | The replacement URL for the App Store to send the user if they don't have the app. _Only necessary if you want a mobile web splash_.
| "$ipad_url" | Same as above, but for iPad Store.
| "$fire_url" | Same as above, but for Amazon Fire Store.
| "$blackberry_url" | Same as above, but for Blackberry Store.
| "$windows_phone_url" | Same as above, but for Windows Store.
| "$after_click_url" | When a user returns to the browser after going to the app, take them to this URL. _iOS only; Android coming soon_.

You have the ability to control the direct deep linking of each link by inserting the following _optional keys in the dictionary_:

| Key | Value
| --- | ---
| "$deeplink_path" | The value of the deep link path that you'd like us to append to your URI. For example, you could specify "$deeplink_path": "radio/station/456" and we'll open the app with the URI "yourapp://radio/station/456?link_click_id=branch-identifier". This is primarily for supporting legacy deep linking infrastructure.
| "$always_deeplink" | true or false. (default is not to deep link first) This key can be specified to have our linking service force try to open the app, even if we're not sure the user has the app installed. If the app is not installed, we fall back to the respective app store or $platform_url key. By default, we only open the app if we've seen a user initiate a session in your app from a Branch link (has been cookied and deep linked by Branch).

**alias**: The alias for a link. Eg. `myapp.com/customalias`

**matchDuration**: The attribution window in seconds for clicks coming from this link.

**stage**: The stage used for the generated link, indicating what part of a funnel the user is in.

**tags**: An array of tag strings to be associated with the link.

#### Get Short Url Parameters

**linkProperties**: The link properties created above that describe the type of link you'd like

**callback**: The callback that is called with url on success, or an error if something went wrong. Note that we'll return a link 100% of the time. Either a short one if network was available or a long one if it was not.

### UIActivityView/Android Share Sheet

UIActivityView and the Branch developed share sheet is the standard way of allowing users to share content from your app. Once you've created your `Branch Universal Object`, which is the reference to the content you're interested in, you can then automatically share it _without having to create a link_ using the mechanism below.

**Sample UIActivityView Share Sheet**

![UIActivityView Share Sheet](https://dev.branch.io/img/pages/getting-started/branch-universal-object/combined_share_sheet.png)

The Branch iOS and Android SDKs includes a wrapper on the share sheets, that will generate a Branch short URL and automatically tag it with the channel the user selects (Facebook, Twitter, etc.).

#### Methods

```csharp
// Define properties of the Branch link
BranchLinkProperties linkProperties = new BranchLinkProperties();
linkProperties.tags.Add("tag1");
linkProperties.tags.Add("tag2");
linkProperties.feature = "invite";
linkProperties.stage = "2";
linkProperties.controlParams.Add("$desktop_url", "http://example.com");
```

```csharp
Branch.shareLink(universalObject, linkProperties, "hello there with short url", (url, error) => {
    if (error != null) {
        Debug.LogError("Branch.shareLink failed: " + error);
    } else {
        Debug.Log("Branch.shareLink shared params: " + url);
    }
});
```

#### Show Share Sheet Parameters

**linkProperties**: The feature the generated link will be associated with.

**andShareText**: A dictionary to use while building up the Branch link.

**fromViewController**: 

**andCallback**: 

#### Returns

None


## Referral system rewarding functionality

In a standard referral system, you have 2 parties: the original user and the invitee. Our system is flexible enough to handle rewards for all users for any actions. Here are a couple example scenarios:

1) Reward the original user for taking action (eg. inviting, purchasing, etc)

2) Reward the invitee for installing the app from the original user's referral link

3) Reward the original user when the invitee takes action (eg. give the original user credit when their the invitee buys something)

These reward definitions are created on the dashboard, under the 'Reward Rules' section in the 'Referrals' tab on the dashboard.

Warning: For a referral program, you should not use unique awards for custom events and redeem pre-identify call. This can allow users to cheat the system.

### Get reward balance

Reward balances change randomly on the backend when certain actions are taken (defined by your rules), so you'll need to make an asynchronous call to retrieve the balance. Here is the syntax:



```csharp
Branch.loadRewards(delegate(bool changed, string error) {
    // changed boolean will indicate if the balance changed from what is currently in memory

    // will return the balance of the current user's credits
    int credits = Branch.getCredits();
});
```


### Redeem all or some of the reward balance (store state)

We will store how many of the rewards have been deployed so that you don't have to track it on your end. In order to save that you gave the credits to the user, you can call redeem. Redemptions will reduce the balance of outstanding credits permanently.



```csharp
// Save that the user has redeemed 5 credits
Branch.redeemRewards(5);
```

### Get credit history

This call will retrieve the entire history of credits and redemptions from the individual user. To use this call, implement like so:



```csharp
Branch.getCreditHistory(delegate(List<string> historyItems, string error) {
    if (error == null) {
        // process history
    }
});
```

The response will return an array that has been parsed from the following JSON:

```json
[
    {
        "transaction": {
                           "date": "2014-10-14T01:54:40.425Z",
                           "id": "50388077461373184",
                           "bucket": "default",
                           "type": 0,
                           "amount": 5
                       },
        "referrer": "12345678",
        "referree": null
    },
    {
        "transaction": {
                           "date": "2014-10-14T01:55:09.474Z",
                           "id": "50388199301710081",
                           "bucket": "default",
                           "type": 2,
                           "amount": -3
                       },
        "referrer": null,
        "referree": "12345678"
    }
]
```
**referrer**
: The id of the referring user for this credit transaction. Returns null if no referrer is involved. Note this id is the user id in developer's own system that's previously passed to Branch's identify user API call.

**referree**
: The id of the user who was referred for this credit transaction. Returns null if no referree is involved. Note this id is the user id in developer's own system that's previously passed to Branch's identify user API call.

**type**
: This is the type of credit transaction

1. _0_ - A reward that was added automatically by the user completing an action or referral
1. _1_ - A reward that was added manually
2. _2_ - A redemption of credits that occurred through our API or SDKs
3. _3_ - This is a very unique case where we will subtract credits automatically when we detect fraud

##Troubleshooting
###Several IMPL_APP_CONTROLLER_SUBCLASS

Unity Branch SDK plugin use own UnityAppController that expands default AppController.
We need to have our own AppController to catch Universal Links.

```
@interface BranchAppController : UnityAppController
{
}
@end

@implementation BranchAppController

- (BOOL)application:(UIApplication *)application continueUserActivity:(NSUserActivity *)userActivity restorationHandler:(void (^)(NSArray *))restorationHandler {
    BOOL handledByBranch = [[BranchUnityWrapper sharedInstance] continueUserActivity:userActivity];
    return handledByBranch;
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(BranchAppController)
```

But some plugins use the same way to expand default AppController, for example:

- Cardboard SDK plugin

#####Solving
In case when several plugins have custom AppController and expand default AppController through IMPL_APP_CONTROLLER_SUBCLASS you need to do the next:

1. Merge all custom AppControllers in one.
2. Comment code in other AppControllers (or delete other AppControllers).



