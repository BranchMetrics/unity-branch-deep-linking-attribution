using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ReferralCodePanel : MonoBehaviour {

    public Text lblCodeState;
    public InputField inputReferralCode;
    public InputField inputAmount;
    public Toggle toggleUnlimited;
    public Toggle toggleUnique;
    public Toggle toggleReferreeUser;
    public Toggle toggleReferrerUser;
    public Toggle toggleBoth;
    public InputField inputCodePrefix;
    public InputField inputExpiration;


    public void OnBtn_GetCode() {
        // amount
        int amount = 5;
        if (inputAmount.text.Length > 0) {
            try {
                amount = Convert.ToInt32(inputAmount.text);
            } catch (Exception e) {
                Debug.Log(e.ToString());
                inputAmount.text = "Invalid value";
                return;
            }
        }

        // prefix
        string prefix = inputCodePrefix.text.Trim();
        inputCodePrefix.text = prefix;

        // date
        DateTime? expiration = null;
        if (inputExpiration.text.Length > 0) {
            try {
                expiration = Convert.ToDateTime(inputExpiration.text);
            } catch (Exception e) {
                Debug.Log(e.ToString());
                inputExpiration.text = "Invalid value";
                return;
            }
        }

        // calculation type
        int calcType = 0;
        if (toggleUnique.isOn) {
            calcType = 1;
        }

        // location
        int location = 0;
        if (toggleReferrerUser.isOn) {
            location = 2;
        } else if (toggleBoth.isOn) {
            location = 3;
        }

        Branch.getReferralCode(prefix, amount, expiration, "", calcType, location, (referralCode, error) => {

            lblCodeState.text = "updating...";

            if (error != null) {
                Debug.Log("Branch.getReferralCode error: " + error);
                lblCodeState.text = "error";
            } else {
                if (referralCode.ContainsKey("error_message")) {
                    inputReferralCode.text = "";
                    lblCodeState.text = referralCode["error_message"].ToString();
                } else {
                    if (referralCode.ContainsKey("referral_code")) {
                        inputReferralCode.text = referralCode["referral_code"].ToString();
                    } else if (referralCode.ContainsKey("promo_code")) {
                        inputReferralCode.text = referralCode["promo_code"].ToString();
                    }

                    lblCodeState.text = "";
                }
            }
        });
    }


    public void OnBtn_Validate() {
        string referral_code = inputReferralCode.text.Trim();

        if (referral_code.Length > 0) {
            Branch.validateReferralCode(referral_code, (referralCode, error) => {

                if (error != null) {
                    Debug.Log("Branch.validateReferralCode error: " + error);
                    lblCodeState.text = "error";
                } else {
                    if (referralCode.ContainsKey("error_message")) {
                        lblCodeState.text = "Invalid";
                    } else {
                        string code = "";

                        if (referralCode.ContainsKey("referral_code")) {
                            code = referralCode["referral_code"].ToString();
                        } else if (referralCode.ContainsKey("promo_code")) {
                            code = referralCode["promo_code"].ToString();
                        }

                        if (referral_code.Equals(code)) {
                            lblCodeState.text = "valid";
                        } else {
                            lblCodeState.text = "mismatch";
                        }
                    }
                }
            });
        }
    }


    public void OnBtn_Redeem() {
        string referral_code = inputReferralCode.text.Trim();

        if (referral_code.Length > 0) {
            Branch.applyReferralCode(referral_code, (referralCode, error) => {

                if (error != null) {
                    Debug.Log("Branch.applyReferralCode error: " + error);
                    lblCodeState.text = "error";
                } else {
                    if (referralCode.ContainsKey("error_message")) {
                        lblCodeState.text = "Invalid";
                    } else {
                        string code = "";

                        if (referralCode.ContainsKey("referral_code")) {
                            code = referralCode["referral_code"].ToString();
                        } else if (referralCode.ContainsKey("promo_code")) {
                            code = referralCode["promo_code"].ToString();
                        }

                        if (referral_code.Equals(code)) {
                            lblCodeState.text = "applied";
                        } else {
                            lblCodeState.text = "mismatch";
                        }
                    }
                }
            });
        }
    }
}
