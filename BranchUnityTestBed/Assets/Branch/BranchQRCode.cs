public class BranchQRCode 
{
    private BranchQRCodeSettings branchQRCodeSettings;

    public BranchQRCode(BranchQRCodeSettings branchQRCodeSettings = null)
    {
        if(branchQRCodeSettings == null)
        {
            branchQRCodeSettings = new BranchQRCodeSettings();
        }

        Init(branchQRCodeSettings);
    }

    private void Init(BranchQRCodeSettings branchQRCodeSettings)
    {
        this.branchQRCodeSettings = branchQRCodeSettings;
    }
}
