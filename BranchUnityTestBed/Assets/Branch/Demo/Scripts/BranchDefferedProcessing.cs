using UnityEngine;
using System.Collections;

public class BranchDefferedProcessing : MonoBehaviour {

	public BranchDemo branchListener;

	IEnumerator Start () {
		yield return new WaitForSeconds(5.0f);
		Debug.Log("Wait 10 secs and start defered processing");
		yield return new WaitForSeconds(10.0f);
		 
		branchListener.onBranchCallback += BranchListener_onBranchCallback;
	}

	void BranchListener_onBranchCallback (BranchUniversalObject arg1, BranchLinkProperties arg2)
	{
		Debug.Log("We receive callback data");
		Debug.Log("Universal Object: " + arg1.ToJsonString());
		Debug.Log("Link Properties: " + arg2.ToJsonString());
	}
}
