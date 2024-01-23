using System;
using System.Collections.Generic;

public class BranchQRCode 
{
    //Primary color of the generated QR code itself
    private string codeColor;

    //Secondary color used as the QR Code background
    private string backgroundColor;

    //The number of pixels for the QR code's border.  Min 1px. Max 20px
    private int margin;

    //Output size of QR Code image. Min 300px. Max 2000px
    private int width;

    //Image Format of the returned QR code. Can be a JPEG or PNG
    private BranchImageFormat imageFormat;

    //A URL of an image that will be added to the center of the QR code. Must be a PNG or JPEG
    private string centerLogoUrl;

    //The style of code pattern used to generate the QR-Code
    private BranchQRCodePattern codePattern;

    // The style of finder pattern used to generate the QR-Code
    private BranchQRCodeFinderPattern finderPattern;

    // Hex string used to change the color of the Qr-Code’s finder pattern
    private string finderPatternColor;

    // Direct link to an image to be used as the background image that is set behind the QR Code
    private string backgroundImageUrl;

    // Adjusts the opacity of the background image
    private int backgroundImageOpacity;

    // Direct link to an image to be used as the code-pattern itself on the QR Code
    private string codePatternUrl;

    // Hex string used to change the color of the interior part of a Qr-Code’s finder pattern
    private string finderEyeColor;

    public BranchQRCode(string codeColor = "#000000", string backgroundColor = "#FFFFFF", int margin = 1, int width = 512, BranchImageFormat imageFormat = BranchImageFormat.PNG, string centerLogoUrl = "")
    {
        Init(codeColor, backgroundColor, margin, width, imageFormat,  centerLogoUrl);
    }

    private void Init(string codeColor, string backgroundColor, int margin, int width, BranchImageFormat imageFormat, string centerLogoUrl)
    {
        this.codeColor = codeColor;
        this.backgroundColor = backgroundColor;
        this.margin = margin;
        this.width = width;
        this.imageFormat = imageFormat;
        this.centerLogoUrl = centerLogoUrl;
    }

    public void loadFromJson(string json)
    {
        if (string.IsNullOrEmpty(json))
            return;

        var data = BranchThirdParty_MiniJSON.Json.Deserialize(json) as Dictionary<string, object>;
        loadFromDictionary(data);
    }

    public void loadFromDictionary(Dictionary<string, object> data)
    {
        if (data == null)
            return;

        if (data.ContainsKey("code_color") && data["code_color"] != null)
        {
            codeColor = data["code_color"].ToString();
        }
        if (data.ContainsKey("background_color") && data["background_color"] != null)
        {
            backgroundColor = data["background_color"].ToString();
        }
        if (data.ContainsKey("margin"))
        {
            margin = Convert.ToInt32(data["margin"].ToString());
        }
        if (data.ContainsKey("width"))
        {
            width = Convert.ToInt32(data["width"].ToString());
        }
        if (data.ContainsKey("image_format"))
        {
            imageFormat = (BranchImageFormat) Convert.ToInt32(data["imageFormat"].ToString());
        }
        if (data.ContainsKey("center_logo_url") && data["center_logo_url"] != null)
        {
            centerLogoUrl = data["center_logo_url"].ToString();
        }
    }

    public string ToJsonString()
    {
        var data = new Dictionary<string, object>();

        data.Add("code_color", codeColor);
        data.Add("background_color", backgroundColor);
        data.Add("margin", margin.ToString());
        data.Add("width", width.ToString());
        data.Add("image_format", imageFormat.ToString());
        data.Add("center_logo_url", centerLogoUrl);

        return BranchThirdParty_MiniJSON.Json.Serialize(data);
    }
}
