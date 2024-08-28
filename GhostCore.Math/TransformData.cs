namespace GhostCore.Math
{
    public struct TransformData
    {
        public readonly double TranslateX;
        public readonly double TranslateY;

        public readonly double Rotation;

        public readonly double ScaleX;
        public readonly double ScaleY;

        public TransformData(double translateX = 0, double translateY = 0, double rotation = 0, double scaleX = 1, double scaleY = 1)
        {
            TranslateX = translateX;
            TranslateY = translateY;
            Rotation = rotation;
            ScaleX = scaleX;
            ScaleY = scaleY;
        }

        public override bool Equals(object obj)
        {
            return obj is TransformData data &&
                   TranslateX == data.TranslateX &&
                   TranslateY == data.TranslateY &&
                   Rotation == data.Rotation &&
                   ScaleX == data.ScaleX &&
                   ScaleY == data.ScaleY;
        }

        public override int GetHashCode()
        {
            int hashCode = 1874165445;
            hashCode = hashCode * -1521134295 + TranslateX.GetHashCode();
            hashCode = hashCode * -1521134295 + TranslateY.GetHashCode();
            hashCode = hashCode * -1521134295 + Rotation.GetHashCode();
            hashCode = hashCode * -1521134295 + ScaleX.GetHashCode();
            hashCode = hashCode * -1521134295 + ScaleY.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{TranslateX},{TranslateY} pos - {Rotation} rads - {ScaleX},{ScaleY} scale";
        }
    }
}
