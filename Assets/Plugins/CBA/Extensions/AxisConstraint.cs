namespace CBA.Extensions
{
    public static class AxisConstraint
    {
        public static UnityEngine.Vector3Int ToVector3Int(this DG.Tweening.AxisConstraint axisConstraint)
        {
            UnityEngine.Vector3Int result = UnityEngine.Vector3Int.zero;

            if (axisConstraint == DG.Tweening.AxisConstraint.None)
            {
                return result;
            }

            if (axisConstraint.HasFlag(DG.Tweening.AxisConstraint.X))
            {
                result.x = 1;
            }

            if (axisConstraint.HasFlag(DG.Tweening.AxisConstraint.Y))
            {
                result.y = 1;
            }

            if (axisConstraint.HasFlag(DG.Tweening.AxisConstraint.Z))
            {
                result.z = 1;
            }

            return result;
        }

        public static UnityEngine.Vector3Int AllowedAxes(this DG.Tweening.AxisConstraint axisConstraint)
        {
            return Vector3Int.InverseAxes(axisConstraint.ToVector3Int());
        }
    }
}
