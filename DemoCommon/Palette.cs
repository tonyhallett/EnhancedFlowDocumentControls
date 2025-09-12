using System.Windows.Media;

namespace DemoCommon
{
    public sealed class Palette
    {
        public Brush MainBackground { get; set; }

        public Brush MainForeground { get; set; }

        public Brush Background { get; set; }

        public Brush Text { get; set; }

        public Brush Border { get; set; }

        public Brush PrevNext { get; set; } = Brushes.Transparent;

        public Brush DropDownGlyphBrush { get; set; }

        public Brush DropDownStateBackground { get; set; }

        public Brush DropDownStateBorderBrush { get; set; }

        public Brush DropDownStateGlyphBrush { get; set; }

        public Brush MenuItemBackground { get; set; }

        public Brush MenuItemBorderBrush { get; set; }

        public Brush MenuItemForeground { get; set; }

        public Brush MenuBackground { get; set; }

        public Brush MenuBorderBrush { get; set; }

        public Brush MenuItemHighlightedBackground { get; set; }

        public Brush MenuItemHighlightedBorderBrush { get; set; }

        public Brush MenuItemHighlightedForeground { get; set; }

        public Brush SelectedGlyphBackground { get; set; }

        public Brush SelectedGlyphBorderBrush { get; set; }

        public Brush SelectedGlyphBrush { get; set; }

        public Brush TooltipBackground { get; set; }

        public Brush TooltipForeground { get; set; }

        public Brush TooltipBorder { get; set; }
    }
}
