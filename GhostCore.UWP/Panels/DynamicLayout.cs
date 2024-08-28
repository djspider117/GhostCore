using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace GhostCore.UWP.Panels
{
    [ContentProperty(Name = nameof(LayoutGroups))]
    public class DynamicLayout : Panel
    {
        public LayoutGroupCollection LayoutGroups
        {
            get { return (LayoutGroupCollection)GetValue(LayoutGroupsProperty); }
            set { SetValue(LayoutGroupsProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty LayoutGroupsProperty =
            DependencyProperty.Register("LayoutGroups", typeof(LayoutGroupCollection), typeof(DynamicLayout), new PropertyMetadata(new LayoutGroupCollection()));


        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(DynamicLayout), new PropertyMetadata(Orientation.Horizontal));

        protected override Size MeasureOverride(Size availableSize)
        {
            int i = 0;

            double w = 0;
            double h = 0;

            foreach (var child in Children)
            {
                var (workingGroup, offset, localItemIndex) = GetLayoutInfo(i);
                var (localUV, localExtent) = GetLocalUV(workingGroup, localItemIndex);

                w = System.Math.Max(localUV.X + localExtent.Width + offset.X, w);
                h = System.Math.Max(localUV.Y + localExtent.Height + offset.Y, h);

                i++;
            }

            return new Size(w, h);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int i = 0;
            foreach (var child in Children)
            {
                var (workingGroup, offset, localItemIndex) = GetLayoutInfo(i);
                var (pos, size) = GetLocalUV(workingGroup, localItemIndex);

                child.Arrange(new Rect(new Point(pos.X + offset.X, pos.Y + offset.Y), size));

                i++;
            }

            return finalSize;
        }

        private (LayoutGroup workingGroup, Point globalCoords, int localItemIndex) GetLayoutInfo(int globalItemIndex)
        {
            var gidx = globalItemIndex;
            var layoutItemCount = LayoutGroups.Sum(x => x.Items.Count);

            var fullW = LayoutGroups.Sum(x => x.CalculatedWidth);
            var fullH = LayoutGroups.Sum(x => x.CalculatedHeight);

            gidx %= layoutItemCount;

            LayoutGroup workingGroup = null;

            int cc = 0;
            int grpIdxHelper = 0;
            int groupidx = 0;
            foreach (var group in LayoutGroups)
            {
                if (gidx < cc)
                {
                    break;
                }

                workingGroup = group;
                cc += group.Items.Count;

                if (gidx < cc)
                {
                    grpIdxHelper = cc - group.Items.Count;
                }
                groupidx++;
            }

            int localItemIndex = gidx - grpIdxHelper;

            double sum = 0;
            for (int i = 0; i < LayoutGroups.IndexOf(workingGroup); i++)
            {
                sum += LayoutGroups[i].CalculatedHeight;
            }

            var yOffset = sum + globalItemIndex / layoutItemCount * fullH;

            return (workingGroup, new Point(0, yOffset), localItemIndex);

        }

        private (Point pos, Size extent) GetLocalUV(LayoutGroup group, int localIndex)
        {
            var itm = group.Items[localIndex];

            var x = 0d;
            var y = 0d;
            var w = 0d;
            var h = 0d;

            // TODO : optimize this

            for (int i = 0; i < itm.Column; i++)
            {
                x += group.ColumnDefinitions[i].Width.Value;
            }

            for (int i = 0; i < itm.Row; i++)
            {
                y += group.RowDefinitions[i].Height.Value;
            }

            for (int i = 0; i < itm.ColumnSpan; i++)
            {
                w += group.ColumnDefinitions[i + itm.Column].Width.Value;
            }

            for (int i = 0; i < itm.RowSpan; i++)
            {
                h += group.RowDefinitions[i + itm.Row].Height.Value;
            }

            return (new Point(x, y), new Size(w, h));
        }
    }

    public class LayoutItemCollection : List<LayoutItem> { }
    public class LayoutGroupCollection : List<LayoutGroup> { }

    public class LocalColumnDefinitionCollection : List<ColumnDefinition> { }
    public class LocalRowDefinitionCollection : List<RowDefinition> { }

    public class LayoutItem
    {
        internal int Width { get; set; }
        internal int Height { get; set; }

        public int Row { get; set; } = 0;
        public int Column { get; set; } = 0;
        public int RowSpan { get; set; } = 1;
        public int ColumnSpan { get; set; } = 1;
    }

    [ContentProperty(Name = nameof(Items))]
    public class LayoutGroup
    {
        public LocalColumnDefinitionCollection ColumnDefinitions { get; set; }
        public LocalRowDefinitionCollection RowDefinitions { get; set; }

        public int DefaultItemWidth { get; set; }
        public int DefaultItemHeight { get; set; }

        public LayoutItemCollection Items { get; set; }

        public double CalculatedWidth => ColumnDefinitions == null ? 0 : ColumnDefinitions.Sum(x => x.Width.GridUnitType == GridUnitType.Pixel ? x.Width.Value : 0);
        public double CalculatedHeight => RowDefinitions == null ? 0 : RowDefinitions.Sum(x => x.Height.GridUnitType == GridUnitType.Pixel ? x.Height.Value : 0);

        internal double UOffset { get; set; }
        internal double VOffset { get; set; }

        public LayoutGroup()
        {
            ColumnDefinitions = new LocalColumnDefinitionCollection();
            RowDefinitions = new LocalRowDefinitionCollection();
            Items = new LayoutItemCollection();
        }
    }

}
