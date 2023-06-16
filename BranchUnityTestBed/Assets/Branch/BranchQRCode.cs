public class BranchQRCode 
{
    //Primary color of the generated QR code itself
    public string codeColor;

    //Secondary color used as the QR Code background
    public string backgroundColor;

    //The number of pixels for the QR code's border.  Min 1px. Max 20px
    public int margin;

    //Output size of QR Code image. Min 300px. Max 2000px
    public int width;

    //Image Format of the returned QR code. Can be a JPEG or PNG
    public BranchImageFormat imageFormat;

    //A URL of an image that will be added to the center of the QR code. Must be a PNG or JPEG
    public string centerLogoUrl;

    //The style of code pattern used to generate the QR-Code
    public BranchQRCodePattern codePattern;

    // The style of finder pattern used to generate the QR-Code
    public BranchQRCodeFinderPattern finderPattern;

    // Hex string used to change the color of the Qr-Code’s finder pattern
    public string finderPatternColor;

    // Direct link to an image to be used as the background image that is set behind the QR Code
    public string backgroundImageUrl;

    // Adjusts the opacity of the background image
    public int backgroundImageOpacity;

    // Direct link to an image to be used as the code-pattern itself on the QR Code
    public string codePatternUrl;

    // Hex string used to change the color of the interior part of a Qr-Code’s finder pattern
    public string finderEyeColor;

    public BranchQRCode()
    {
        Init();
    }

    private void Init(string codeColor = "#000000", string backgroundColor = "#FFFFFF", int margin = 1, int width = 512,
                      BranchImageFormat imageFormat = BranchImageFormat.PNG, string centerLogoUrl = "", BranchQRCodePattern codePattern = BranchQRCodePattern.Default,
                      BranchQRCodeFinderPattern finderPattern = BranchQRCodeFinderPattern.Square, string finderPatternColor = "#000000",
                      string backgroundImageUrl = "", int backgroundImageOpacity = 255, string codePatternUrl = "", string finderEyeColor = "")
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
}
