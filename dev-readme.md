# EnhancedFlowControls

Adds behaviour to `FlowDocumentReader`, `FlowDocumentPageViewer` and `FlowDocumentScrollViewer`.

The main feature is to allow consumers to provide their own find toolbar and this is the purpose of this readme.
`FlowDocumentReader` is discussed in detail. `FlowDocumentPageViewer` and `FlowDocumentScrollViewer` behave similarly
and leads to a common solution.

# For a derived FlowDocumentReader to provide its own find toolbar it is necessary to know

a. When and how the original find toolbar is added and removed from the wpf tree.

b. When find occurs and how does it work.

c. Any other UI behaviour of the find toolbar.

## When and how the original find toolbar is added and removed from the wpf tree.

### When

```C#
private void ToggleFindToolBar(bool enable){
    //...
    DocumentViewerHelper.ToggleFindToolBar(this._findToolBarHost, new EventHandler(this.OnFindInvoked), enable);
}
```

where findToolBarHost is a part from the FlowDocumentReader control template ( a Border by default ) and retrieved in OnApplyTemplate

### How

**The host has the Child set to the FindToolBar when enabled.**

DocumentViewerHelper

```C#
internal static void ToggleFindToolBar(
    Decorator findToolBarHost,
    EventHandler handlerFindClicked,
    bool enable)
{
    if (enable)
    {
        FindToolBar findToolBar = new FindToolBar();

        findToolBarHost.Child = (UIElement) findToolBar;
        findToolBarHost.Visibility = Visibility.Visible;

        KeyboardNavigation.SetTabNavigation((DependencyObject) findToolBarHost, KeyboardNavigationMode.Continue);
        FocusManager.SetIsFocusScope((DependencyObject) findToolBarHost, true);

        findToolBar.SetResourceReference(FrameworkElement.StyleProperty, (object) DocumentViewerHelper.FindToolBarStyleKey);

        findToolBar.FindClicked += handlerFindClicked;

        findToolBar.DocumentLoaded = true;

        findToolBar.GoToTextBox();
    }
    else
    {
        FindToolBar child = findToolBarHost.Child as FindToolBar;
        child.FindClicked -= handlerFindClicked;
        child.DocumentLoaded = false;
        findToolBarHost.Child = (UIElement) null;
        findToolBarHost.Visibility = Visibility.Collapsed;

        KeyboardNavigation.SetTabNavigation((DependencyObject) findToolBarHost, KeyboardNavigationMode.None);
        findToolBarHost.ClearValue(FocusManager.IsFocusScopeProperty);
    }
}

```

When ToggleFindToolBar occurs.

| When                                | Condition                                  | enable              |
| ----------------------------------- | ------------------------------------------ | ------------------- |
| OnApplyTemplate                     | FindToolBar != null                        | false               |
| DocumentChanged                     | !CanShowFindToolBar && FindToolBar != null | false               |
| IsFindEnabled changed               | !CanShowFindToolBar && FindToolBar != null | false               |
| OnKeyDown Esc                       | FindToolBar != null                        | false               |
| OnKeyDown F3                        | CanShowFindToolBar && FindToolBar == null  | true                |
| **protected virtual** OnFindCommand | !CanShowFindToolBar                        | FindToolBar == null |

**This is important.**

```C#
private FindToolBar FindToolBar => this._findToolBarHost == null ? (FindToolBar) null : this._findToolBarHost.Child as FindToolBar;
```

```C#
private bool CanShowFindToolBar => this._findToolBarHost != null && this.IsFindEnabled && this.Document != null;
```

OnFindCommand is invoked from.

```C#
public void Find() => this.OnFindCommand();
```

and ApplicationCommands.Find

```C#
args.CanExecute = flowDocumentReader.CanShowFindToolBar;
```

```xaml
    <ToggleButton x:Name="FindButton"
        Command="Find"
```

---

```C#
findToolBar.DocumentLoaded = true;
```

This sets IsEnabled of the FindNextButton and FindPreviousButton, ( FindEnabled will be true )

**This focuses the text box.**

```C#
findToolBar.GoToTextBox();
```

# When find occurs and how it works

```C#
private void OnFindInvoked(object sender, EventArgs e)
{
    TextEditor textEditor = this.TextEditor;
    FindToolBar findToolBar = this.FindToolBar;
    if (findToolBar == null || textEditor == null)
        return;

    if (this.CurrentViewer != null && this.CurrentViewer is UIElement)
        ((UIElement) this.CurrentViewer).Focus();

    // ***********************************************
    ITextRange findResult = DocumentViewerHelper.Find(findToolBar, textEditor, textEditor.TextView, textEditor.TextView);
    if (findResult != null && !findResult.IsEmpty)
    {
        if (this.CurrentViewer == null)
            return;
        this.CurrentViewer.ShowFindResult(findResult);
    }
    else
        DocumentViewerHelper.ShowFindUnsuccessfulMessage(findToolBar);
}
```

The FindToolBar ( from the child of the host) contains properties for what is necessary

```C#
public string SearchText => this.FindTextBox.Text;
public bool SearchUp
{
    get => this._searchUp;
    set
    {
        if (this._searchUp == value)
            return;
        this._searchUp = value;
    }
}

public bool MatchCase => this.OptionsCaseMenuItem.IsChecked;

public bool MatchWholeWord => this.OptionsWholeWordMenuItem.IsChecked;

public bool MatchDiacritic => this.OptionsDiacriticMenuItem.IsChecked;

public bool MatchKashida => this.OptionsKashidaMenuItem.IsChecked;

public bool MatchAlefHamza => this.OptionsAlefHamzaMenuItem.IsChecked;
```

**When OnFindInvoked is invoked.**

| When                       | Condition                                      | SearchUp property setting          |
| -------------------------- | ---------------------------------------------- | ---------------------------------- |
| OnKeyDown F3               | CanShowFindToolBar && FindToolBar != null      | SearchUp true if Shift key pressed |
| (FindToolBar.FindClicked ) |                                                |                                    |
| FindNextButton.Click       |                                                | SearchUp false                     |
| FindPreviousButton.Click   |                                                | SearchUp true                      |
| FindTextBox.PreviewKeyDown | FindEnabled && ( Key.Return \|\| Key.Execute ) |                                    |

---

# Additional framework code to consider

`FlowDocumentReader` registers a class key down handler. Logic provided by the `DocumentViewerHelper`.

````C#
EventManager.RegisterClassHandler(typeof (FlowDocumentReader), Keyboard.KeyDownEvent, (Delegate) new KeyEventHandler(FlowDocumentReader.KeyDownHandler), true);
'''

```C#
private static void KeyDownHandler(object sender, KeyEventArgs e) => DocumentViewerHelper.KeyDownHelper(e, (DependencyObject) ((FlowDocumentReader) sender)._findToolBarHost);
````

Note that this code has access to framework internal members.

```C#
internal static void KeyDownHelper(KeyEventArgs e, DependencyObject findToolBarHost)
{
    if (e.Handled || findToolBarHost == null || e.Key != Key.Left && e.Key != Key.Right && e.Key != Key.Up && e.Key != Key.Down || !(Keyboard.FocusedElement is DependencyObject focusedElement) || !(focusedElement is Visual) || !VisualTreeHelper.IsAncestorOf(findToolBarHost, focusedElement))
        return;
    FocusNavigationDirection traversalDirection = KeyboardNavigation.KeyToTraversalDirection(e.Key);
    DependencyObject dependencyObject = KeyboardNavigation.Current.PredictFocusedElement(focusedElement, traversalDirection);
    if (dependencyObject == null || !(dependencyObject is IInputElement) || !VisualTreeHelper.IsAncestorOf(findToolBarHost, focusedElement))
        return;
    ((IInputElement) dependencyObject).Focus();
    e.Handled = true;
}
```

# The two flow document viewer classes, `FlowDocumentPageViewer` and `FlowDocumentScrollViewer` are similar.

They use the same `KeyDownHelper.KeyDownHelper`.

They have the same Decorator PART_FindToolBarHost and the FindToolBar variable retrieves from it.

`DocumentViewerHelper.Find` is invoked by the same actions.

`DocumentViewerHelper.ToggleFindToolBar` is invoked the same except there is no IsFindEnabled ( and no find button to invoke ApplicationCommands.Find).
The FlowDocumentScrollViewer also will `ToggleFindToolBar(false)` when `AttachTextEditor()`

```C#
      if (this._textEditor != null || this.FindToolBar == null)
        return;
      this.ToggleFindToolBar(false);
```

```C#
this._findToolBarHost = this.GetTemplateChild("PART_FindToolBarHost") as Decorator; // again by default is a Border

private FindToolBar FindToolBar => this._findToolBarHost == null ? (FindToolBar) null : this._findToolBarHost.Child as FindToolBar;
```

This differs to the FlowDocumentReader

```C#
internal bool CanShowFindToolBar => this._findToolBarHost != null && this.Document != null && this._textEditor != null;
```

---

# Solution notes

Regardless of the solution, executing find should probably be with OnFindInvoked.

**This requires**

```C#
private FindToolBar FindToolBar => this._findToolBarHost == null ? (FindToolBar) null : this._findToolBarHost.Child as FindToolBar;
```

with a `FindToolBar` that has had the find properties set based upon the provided replacement find tool bar.

**OnKeyDown different behaviour for OnKeyDown**
FlowDocumentPageViewer does not immediately exit when e.Handled is true.

# Implementation

The work is done by the `FindToolBarManager`.

Each of the flow controls create an instance and call into it or call its static method.

## Setup from OnApplyTemplate

```C#
_findToolBarManager.Setup(this, FindToolBar);
```

This will use reflection to change the `_findToolBarHost` field to a custom `Decorator`, `AlertingFindToolBarHost`, that will raise an event when the `Child` ( `FindToolBar` ) changes.
This allows for replicating the `DocumentViewerHelper.ToggleFindToolBar` behaviour without having to deal with the conditions behind it.
This also is in line with

```C#
private FindToolBar FindToolBar => this._findToolBarHost == null ? (FindToolBar) null : this._findToolBarHost.Child as FindToolBar;
```

The original host is kept.

## The base flow control toggles to show.

The `AlertingFindToolBarHost` raises the event and the following happens.

The `FindToolBar` property from the enhanced control is set as the child of the original host and a `FindToolBarViewModel` is created and linked to the FindToolBar in one of two ways.
If the FindToolBar is `IFindToolBarViewModelAware` then its `FindToolBarViewModel` property is set otherwise it is set as the `DataContext` with the `FindToolBarViewModel` retaining the `OriginalDataContext`.

The `FindToolBarViewModel` is passed a `FindToolBarWrapper` that wraps the original `FindToolBar`.
The wrapper has `public void Find(IFindParameters findParameters)` whichs sets the `FindToolBar` properties pertaining to search and invokes `OnFindClick` with reflection.

The `FindToolBarViewModel` implements `IFindParameters`, these properties are bound to in the tree of the replacement find toolbar.
The wrapper `Find` method will be invoked from the `FindToolBarViewModel` `NextCommand` and `PreviousCommand` and also invoked from
`OnKeyDown` when F3 or Shift F3 is pressed or when then the required find text box with x:Name `findTextBox` `TextBox.PreviewKeyDown` with enter key.

The relevant internal `DocumentViewerHelper.ToggleFindToolBar` code is replicated in the `DocumentViewerHelper.ToggleFindToolBarHost`.

For F3 / Shift F3 functionality to work similarly across all 3 types they all pass the base method and the `FindToolBarManager` invokes base when necessary.

```
protected override void OnKeyDown(KeyEventArgs e) => _findToolBarManager.KeyDown(e, base.OnKeyDown);
```

```C#
        internal void KeyDown(KeyEventArgs e, Action<KeyEventArgs> baseKeyDown)
        {
            if (IsShowingFindToolbar && e.Key == Key.F3)
            {
                bool shiftPressed = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
                _findToolBarViewModel.Find(shiftPressed);
                e.Handled = true;
                return;
            }

            baseKeyDown(e);
        }
```

## The base flow control toggles to hide.

The original host is cleared and `DocumentViewerHelper.ToggleFindToolBarHost`.

## Other internal code replication

The `Keyboard.KeyDownEvent` class handler that calls `DocumentViewerHelper.KeyDownHelper` is replicated with reflection.
Each enhanced flow control adds a class handler, e.g

```C#
        static EnhancedFlowDocumentReader() => EventManager.RegisterClassHandler(
                typeof(EnhancedFlowDocumentReader),
                Keyboard.KeyDownEvent,
                new KeyEventHandler(FindToolBarManager.KeyDownHandler),
                true);

```

Each enhanced flow control implements below so that the static `FindToolBarManager` can get the `FindToolBarManager` instance.

```C#
    internal interface IEnhancedFlowDocumentControl
    {
        FindToolBarManager FindToolBarManager { get; }
    }
```

# Solution configuration

The UITests should run against the release version of EnhancedFlowControls.  As such there are two solution configurations.

Solution config | Demo   | EnhancedFlowControls | UITests | Tests  | VideoRecorder
----------------|--------|----------------------|---------|--------|---------------
Debug           | Debug  | Debug                | Release | Debug  | Debug
UITests         | Release| Release              | Debug   | Debug  | Debug

UITests should be run with the UITests solution configuration.

UITests.csproj defines UITestsSolutionConfig
```xml
  <PropertyGroup Condition="'$(Configuration)|$(Platform)' == 'Debug|AnyCPU'">
    <DefineConstants>$(DefineConstants);UITestsSolutionConfig</DefineConstants>
   </PropertyGroup>
```

accessible in code
```cs
    internal static class SolutionConfiguration
    {
#if UITestsSolutionConfig
    public const bool IsUITests = true;
#else
        public const bool IsUITests = false;
#endif
    }
```

which will throw in the base test class - FindToolBarTestsBase
```cs
        protected override Application StartApplication()
        {
            if (!SolutionConfiguration.IsUITests)
            {
                throw new Exception("UITests must be run with the UITests solution configuration.");
            }

            _ = NativeMethods.SetProcessDPIAware();
            IsNormal = windowTypeName.StartsWith("Normal");
            Application application = DemoApplicationLauncher.Launch(frameworkVersion, windowTypeName);
            _window = application.GetMainWindow(Automation);
            return application;
        }
```

Building the solution in the UITests configuration will produce the EnhancedFlowControls nuget package.