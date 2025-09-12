using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace DemoCommon
{
    public sealed class PaletteSwitcher : INotifyPropertyChanged
    {
        private readonly Palette _bluePalette = new Palette
        {
            MainBackground = Brushes.DarkBlue,
            MainForeground = Brushes.LightBlue,
            Background = Brushes.LightBlue,
            Text = Brushes.DarkBlue,
            Border = Brushes.DarkBlue,
            PrevNext = Brushes.DarkBlue,
            DropDownGlyphBrush = Brushes.BlueViolet,
            DropDownStateBackground = Brushes.DarkSlateBlue,
            DropDownStateBorderBrush = Brushes.AliceBlue,
            DropDownStateGlyphBrush = Brushes.AliceBlue,
            MenuBackground = Brushes.LightBlue,
            MenuBorderBrush = Brushes.SteelBlue,
            MenuItemBackground = Brushes.DodgerBlue,
            MenuItemBorderBrush = Brushes.DarkBlue,
            MenuItemForeground = Brushes.DarkBlue,
            MenuItemHighlightedBackground = Brushes.DarkBlue,
            MenuItemHighlightedForeground = Brushes.DodgerBlue,
            MenuItemHighlightedBorderBrush = Brushes.DodgerBlue,
            SelectedGlyphBrush = Brushes.RoyalBlue,
            SelectedGlyphBackground = Brushes.LightSkyBlue,
            SelectedGlyphBorderBrush = Brushes.DarkBlue,
            TooltipBackground = Brushes.LightBlue,
            TooltipForeground = Brushes.DarkBlue,
            TooltipBorder = Brushes.DarkBlue,
        };

        private readonly Palette _pinkPalette = new Palette
        {
            MainBackground = Brushes.DeepPink,
            MainForeground = Brushes.LightPink,
            Background = Brushes.Pink,
            Text = Brushes.DeepPink,
            Border = Brushes.DeepPink,
            PrevNext = Brushes.DeepPink,
            DropDownGlyphBrush = Brushes.HotPink,
            DropDownStateBackground = Brushes.LightPink,
            DropDownStateBorderBrush = Brushes.DeepPink,
            DropDownStateGlyphBrush = Brushes.DeepPink,
            MenuBackground = Brushes.LightSalmon,
            MenuBorderBrush = Brushes.DeepPink,
            MenuItemBackground = Brushes.DeepPink,
            MenuItemBorderBrush = Brushes.LightPink,
            MenuItemForeground = Brushes.LightPink,
            MenuItemHighlightedBackground = Brushes.LightPink,
            MenuItemHighlightedForeground = Brushes.DeepPink,
            MenuItemHighlightedBorderBrush = Brushes.DeepPink,
            SelectedGlyphBrush = Brushes.MistyRose,
            SelectedGlyphBackground = Brushes.HotPink,
            SelectedGlyphBorderBrush = Brushes.LightPink,
            TooltipBackground = Brushes.LightPink,
            TooltipForeground = Brushes.DeepPink,
            TooltipBorder = Brushes.DeepPink,
        };

        private readonly Palette _leftPalette;
        private readonly Palette _rightPalette;

        public PaletteSwitcher()
            : this(true)
        {
        }

        public PaletteSwitcher(bool bluePaletteLeft)
        {
            if (bluePaletteLeft)
            {
                _leftPalette = _bluePalette;
                _rightPalette = _pinkPalette;
            }
            else
            {
                _rightPalette = _bluePalette;
                _leftPalette = _pinkPalette;
            }

            _selectedPalette = _leftPalette;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public static PaletteSwitcher Instance { get; } = new PaletteSwitcher();

        private Palette _selectedPalette;

        public Palette SelectedPalette
        {
            get => _selectedPalette;
            set
            {
                if (_selectedPalette == value)
                {
                    return;
                }

                _selectedPalette = value;
                OnPropertyChanged();
            }
        }

        public bool LeftPaletteSelected
        {
            get => _selectedPalette == _leftPalette;
            set
            {
                SelectedPalette = value ? _leftPalette : _rightPalette;
                OnPropertyChanged();
            }
        }

        public bool RightPaletteSelected
        {
            get => _selectedPalette == _rightPalette;
            set
            {
                SelectedPalette = value ? _rightPalette : _leftPalette;
                OnPropertyChanged();
            }
        }
    }
}
