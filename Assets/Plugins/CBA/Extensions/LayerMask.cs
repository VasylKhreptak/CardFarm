namespace CBA.Extensions
{
    public static class LayerMask
    {
        public static bool ContainsLayer(this UnityEngine.LayerMask layerMask, int layerID)
        {
            return (layerMask.value & (1 << layerID)) > 0;
        }
    }
}
