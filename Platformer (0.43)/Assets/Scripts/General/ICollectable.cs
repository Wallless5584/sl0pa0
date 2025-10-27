public enum CollectableType
{
    None,
    Coin,
}

public interface ICollectable
{
    public CollectableType Type { get; set; } 
    public void Collect();
    public void ResetObject();
}
