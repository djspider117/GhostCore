using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;

namespace GhostCore.Math
{
    public class TransformData
    {
        [XmlAttribute]
        public double TranslateX { get; set; }

        [XmlAttribute]
        public double TranslateY { get; set; }

        [XmlAttribute]
        public double Rotation { get; set; }

        [XmlAttribute]
        public double ScaleX { get; set; } = 1;

        [XmlAttribute]
        public double ScaleY { get; set; } = 1;
    }
}
