using GhostCore.Animations.Layers;
using Microsoft.Graphics.Canvas;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Markup;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI.Text;

namespace GhostCore.Animations
{
    public class FoundationCompositionReader
    {
        private static volatile FoundationCompositionReader _instance;
        private static object _syncRoot = new object();
        private static Dictionary<Type, Func<string, object>> _factoryMapping;

        internal FoundationCompositionReader()
        {
            _factoryMapping = new Dictionary<Type, Func<string, object>>
            {
                { typeof(string), x => x },
                { typeof(bool), x => bool.Parse(x) },
                { typeof(float), x => float.Parse(x) },
                { typeof(Rect), x => x.ToRect() },
                { typeof(Color), x => x.ToColor() },
                { typeof(Size), x => x.ToSize() },
                { typeof(Vector2), x => x.ToVector2() },
            };

            AddEnum(typeof(CanvasTextDirection));
            AddEnum(typeof(FontStretch));
            AddEnum(typeof(FontStyle));
            AddEnum(typeof(CanvasLineSpacingMode));
            AddEnum(typeof(CanvasHorizontalAlignment));
            AddEnum(typeof(CanvasVerticalAlignment));
            AddEnum(typeof(CanvasWordWrapping));
            AddEnum(typeof(CanvasDrawTextOptions));
            AddEnum(typeof(CanvasCapStyle));
            AddEnum(typeof(CanvasDashStyle));
            AddEnum(typeof(CanvasLineJoin));
            AddEnum(typeof(CanvasStrokeTransformBehavior));
            AddEnum(typeof(ImageType));
            AddEnum(typeof(CanvasBlend));
            AddEnum(typeof(CompositionWrapMode));
            AddEnum(typeof(FoundationCompositionLayerSourceType));

        }

        public static FoundationCompositionReader GetDefault()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new FoundationCompositionReader();
                }
            }

            return _instance;
        }

        public async Task<FoundationComposition> ReadFile(IStorageFile file)
        {
            var xml = await FileIO.ReadTextAsync(file);
            return await ReadXmlString(xml);
        }
        public async Task<FoundationComposition> ReadFile(string path)
        {
            if (path.Contains("ms-appx"))
            {
                return await ReadFile(await StorageFile.GetFileFromApplicationUriAsync(new Uri(path)));
            }

            var file = await StorageFile.GetFileFromPathAsync(path);
            return await ReadFile(file);
        }

        public Task<FoundationComposition> ReadXmlString(string xml)
        {
            return Task.Run(() =>
               {
                   FoundationComposition rv = new FoundationComposition();

                   XmlDocument doc = new XmlDocument();
                   doc.LoadXml(xml);

                   var root = doc.DocumentElement;

                   if (root.Name != "FoundationComposition")
                       throw new InvalidDataException("Not a valid FANS xml string");

                   if (root.HasAttributes)
                   {
                       ParseAttributes(rv, root); //parse root attribs
                   }

                   if (root.HasChildNodes)
                   {
                       var lls = new List<ILayer>();

                       ParseLayers(root, lls);

                       rv.Layers = lls;

                   }

                   return rv;
               });
        }

        private static void ParseLayers(XmlNode node, IList<ILayer> lls)
        {
            //parse layers
            foreach (XmlNode child in node.ChildNodes)
            {
                var layer = CreateLayer(child);
                ParseAttributes(layer, child); //parse layer attribs

                if (layer is CompositeLayer compLayer)
                {
                    compLayer.Children = new List<ILayer>();
                    ParseLayers(child, compLayer.Children);
                }

                var head = $"{child.Name}.";
                var expandedXmlComponents = child.ChildNodes.Cast<XmlNode>().Where(x => x.Name.Contains(head));

                //parse expanded props (xxxxx.Transform or xxxx.Animations)
                var layerType = layer.GetType();
                foreach (var comp in expandedXmlComponents)
                {
                    var propName = comp.Name.Split('.')[1];
                    var prop = layerType.GetProperty(propName);

                    if (propName == nameof(TextLayer.TextKeyframes))
                    {
                        //parse text keyframes for text layers
                        var textkfs = new List<TextKeyframe>();
                        foreach (XmlNode x in comp.ChildNodes)
                        {
                            var kf = new TextKeyframeSerializationHelper();
                            ParseAttributes(kf, x);
                            textkfs.Add(kf.ToKeyFrame());
                        }

                        (layer as TextLayer).TextKeyframes = textkfs;
                    }
                    else
                    if (propName == nameof(LayerBase.Animations))
                    {
                        //parse animations
                        var anims = new List<AnimationCurve>();
                        foreach (XmlNode x in comp.ChildNodes)
                        {
                            var curve = CreateAnimCurve(x);
                            ParseAttributes(curve, x);

                            if (x.HasChildNodes)
                            {
                                var keyframes = new List<Keyframe>();
                                foreach (XmlNode kfs in x.ChildNodes)
                                {
                                    var kf = new KeyframeSerializationHelper();
                                    ParseAttributes(kf, kfs);
                                    keyframes.Add(kf.ToKeyFrame());
                                }

                                curve.Keyframes = keyframes.ToArray();
                            }

                            anims.Add(curve);
                        }

                        layer.Animations = anims;
                    }
                    else
                    {
                        //parse simple expanded props
                        var expTarget = prop.PropertyType.GetConstructor(Type.EmptyTypes).Invoke(null);

                        ParseAttributes(expTarget, comp.FirstChild);
                        prop.SetValue(layer, expTarget);
                    }
                }

                lls.Add(layer);
            }
        }

        public static ILayer CreateLayer(XmlNode node)
        {
            switch (node.Name)
            {
                case "ImageLayer":
                    return new ImageLayer();
                case "PathLayer":
                    return new PathLayer();
                case "TextLayer":
                    return new TextLayer();
                case "CompositeLayer":
                    return new CompositeLayer();
                case "FoundationCompositionLayer":
                    return new FoundationCompositionLayer();
                default:
                    return null;
            }
        }
        public static AnimationCurve CreateAnimCurve(XmlNode node)
        {
            switch (node.Name)
            {
                case "AnimationCurve":
                    return new AnimationCurve();
                case "CubicCurve":
                case "BounceCurve":
                case "LinearCurve":
                case "SpringCurve":
                    return null;
                default:
                    return null;
            }
        }

        private static void ParseAttributes(object target, XmlNode root)
        {
            foreach (XmlAttribute attrib in root.Attributes)
            {
                SetProperty(target, attrib);
            }
        }
        private static void SetProperty(object target, XmlNode attrib)
        {
            var type = target.GetType();
            var prop = type.GetProperty(attrib.Name);
            var strval = attrib.Value;

            SetProperty(target, prop, strval);
        }
        private static void SetProperty(object target, PropertyInfo prop, string strval)
        {
            if (_factoryMapping.TryGetValue(prop.PropertyType, out var fact))
            {
                var toSet = fact.Invoke(strval);
                prop.SetValue(target, toSet);
            }

        }
        private static void AddEnum(Type enumType)
        {
            _factoryMapping.Add(enumType, v => Enum.Parse(enumType, v));
        }
    }

    public static class StringConverters
    {
        public static Rect ToRect(this string str)
        {
            var split = str.Split(',');
            if (split.Length == 1)
                split = str.Split(' ');

            return new Rect(double.Parse(split[0]), double.Parse(split[1]), double.Parse(split[2]), double.Parse(split[3]));
        }

        public static Vector2 ToVector2(this string str)
        {
            var split = str.Split(',');
            if (split.Length == 1)
                split = str.Split(' ');

            if (split.Length == 1)
                return new Vector2(float.Parse(split[0]), float.Parse(split[0]));

            return new Vector2(float.Parse(split[0]), float.Parse(split[1]));
        }

        public static Size ToSize(this string str)
        {
            var split = str.Split(',');
            if (split.Length == 1)
                split = str.Split(' ');

            return new Size(double.Parse(split[0]), double.Parse(split[1]));
        }

        public static Color ToColor(this string colorString)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                throw new ArgumentException(nameof(colorString));
            }

            if (colorString[0] == '#')
            {
                switch (colorString.Length)
                {
                    case 9:
                        {
                            var cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                            var a = (byte)(cuint >> 24);
                            var r = (byte)((cuint >> 16) & 0xff);
                            var g = (byte)((cuint >> 8) & 0xff);
                            var b = (byte)(cuint & 0xff);

                            return Color.FromArgb(a, r, g, b);
                        }

                    case 7:
                        {
                            var cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                            var r = (byte)((cuint >> 16) & 0xff);
                            var g = (byte)((cuint >> 8) & 0xff);
                            var b = (byte)(cuint & 0xff);

                            return Color.FromArgb(255, r, g, b);
                        }

                    case 5:
                        {
                            var cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                            var a = (byte)(cuint >> 12);
                            var r = (byte)((cuint >> 8) & 0xf);
                            var g = (byte)((cuint >> 4) & 0xf);
                            var b = (byte)(cuint & 0xf);
                            a = (byte)(a << 4 | a);
                            r = (byte)(r << 4 | r);
                            g = (byte)(g << 4 | g);
                            b = (byte)(b << 4 | b);

                            return Color.FromArgb(a, r, g, b);
                        }

                    case 4:
                        {
                            var cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                            var r = (byte)((cuint >> 8) & 0xf);
                            var g = (byte)((cuint >> 4) & 0xf);
                            var b = (byte)(cuint & 0xf);
                            r = (byte)(r << 4 | r);
                            g = (byte)(g << 4 | g);
                            b = (byte)(b << 4 | b);

                            return Color.FromArgb(255, r, g, b);
                        }

                    default:
                        throw new FormatException(string.Format("The {0} string passed in the colorString argument is not a recognized Color format.", colorString));
                }
            }

            if (colorString.Length > 3 && colorString[0] == 's' && colorString[1] == 'c' && colorString[2] == '#')
            {
                var values = colorString.Split(',');

                if (values.Length == 4)
                {
                    var scA = double.Parse(values[0].Substring(3));
                    var scR = double.Parse(values[1]);
                    var scG = double.Parse(values[2]);
                    var scB = double.Parse(values[3]);

                    return Color.FromArgb((byte)(scA * 255), (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
                }

                if (values.Length == 3)
                {
                    var scR = double.Parse(values[0].Substring(3));
                    var scG = double.Parse(values[1]);
                    var scB = double.Parse(values[2]);

                    return Color.FromArgb(255, (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
                }

                throw new FormatException(string.Format("The {0} string passed in the colorString argument is not a recognized Color format (sc#[scA,]scR,scG,scB).", colorString));
            }

            var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(colorString);

            if (prop != null)
            {
                return (Color)prop.GetValue(null);
            }

            throw new FormatException(string.Format("The {0} string passed in the colorString argument is not a recognized Color.", colorString));
        }

    }
}
