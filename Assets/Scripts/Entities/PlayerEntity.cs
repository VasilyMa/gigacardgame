public class PlayerEntity : SourceEntity
{
    public string PlayFabUniqueID;
    private PlayerEntity instance;

    public override  SourceEntity Instance => instance;

    public override SourceEntity Init()
    {
        instance = this;
        return this;
    }
}
