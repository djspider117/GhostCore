namespace GhostCore.ObjectModel
{
    public enum BindingMode
    {
        //from source to target
        OneWay = 0x01,

        OneTime = 0x02,

        //from source to target AND from target to source
        TwoWay = 0x03,

        //from target to source
        OneWayToSource = 0x04,
        None = 0
    }
}
