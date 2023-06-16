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

    public BranchQRCode(string codeColor = "#000000", string backgroundColor = "#FFFFFF", int margin = 1, int width = 512,
                        BranchImageFormat imageFormat = BranchImageFormat.PNG, string centerLogoUrl = "", BranchQRCodePattern codePattern = BranchQRCodePattern.Default,
                        BranchQRCodeFinderPattern finderPattern = BranchQRCodeFinderPattern.Square, string finderPatternColor = "#000000",
                        string backgroundImageUrl = "", int backgroundImageOpacity = 255, string codePatternUrl = "", string finderEyeColor = "")
    {
        Init(codeColor, backgroundColor, margin, width, imageFormat,  centerLogoUrl,  codePattern, finderPattern,  finderPatternColor, backgroundImageUrl,  backgroundImageOpacity,  codePatternUrl,  finderEyeColor);
    }

    private void Init(string codeColor, string backgroundColor, int margin, int width,
                      BranchImageFormat imageFormat, string centerLogoUrl, BranchQRCodePattern codePattern,
                      BranchQRCodeFinderPattern finderPattern, string finderPatternColor,
                      string backgroundImageUrl, int backgroundImageOpacity, string codePatternUrl, string finderEyeColor)
    {
        this.codeColor = codeColor;
        this.backgroundColor = backgroundColor;
        this.margin = margin;
        this.width = width;
        this.imageFormat = imageFormat;
        this.centerLogoUrl = centerLogoUrl;
        this.codePattern = codePattern;
        this.finderPattern = finderPattern;
        this.finderPatternColor = finderPatternColor;
        this.backgroundImageUrl = backgroundImageUrl;
        this.backgroundImageOpacity = backgroundImageOpacity;
        this.codePatternUrl = codePatternUrl;
        this.finderEyeColor = finderEyeColor;
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
        if (data.ContainsKey("code_pattern"))
        {
            codePattern = (BranchQRCodePattern) Convert.ToInt32(data["code_pattern"].ToString());
        }
        if (data.ContainsKey("finder_pattern"))
        {
            finderPattern = (BranchQRCodeFinderPattern)Convert.ToInt32(data["finder_pattern"].ToString());
        }
        if (data.ContainsKey("finder_pattern_color") && data["finder_pattern_color"] != null)
        {
            finderPatternColor = data["finder_pattern_color"].ToString();
        }
        if (data.ContainsKey("background_image_url") && data["background_image_url"] != null)
        {
            backgroundImageUrl = data["background_image_url"].ToString();
        }
        if (data.ContainsKey("background_image_opacity"))
        {
            backgroundImageOpacity = Convert.ToInt32(data["background_image_opacity"].ToString());
        }
        if (data.ContainsKey("code_pattern_url") && data["code_pattern_url"] != null)
        {
            codePatternUrl = data["code_pattern_url"].ToString();
        }
        if (data.ContainsKey("finder_eye_color") && data["finder_eye_color"] != null)
        {
            finderEyeColor = data["finder_eye_color"].ToString();
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
        data.Add("code_pattern", codePattern.ToString());
        data.Add("finder_pattern", finderPattern.ToString());
        data.Add("finder_pattern_color", finderPatternColor);
        data.Add("background_image_url", backgroundImageUrl);
        data.Add("background_image_opacity", backgroundImageOpacity.ToString());
        data.Add("code_pattern_url", codePatternUrl);
        data.Add("finder_eye_color", finderEyeColor);

        return BranchThirdParty_MiniJSON.Json.Serialize(data);
    }
}
