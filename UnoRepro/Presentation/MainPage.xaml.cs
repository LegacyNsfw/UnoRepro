using System.Threading.Tasks;
using Microsoft.UI.Xaml.Input;

namespace UnoRepro.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        this.TextBox.GettingFocus += TextBox_GettingFocus;
        this.TabView.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(HandleHotKey2), false);
    }

    private async void TextBox_GettingFocus(UIElement sender, GettingFocusEventArgs args)
    {
        object dataContext = DataContext;
        if (dataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                // string currentFocus = args.OldFocusedElement?.GetType().Name ?? "None";
                string currentFocus = FocusManager.GetFocusedElement(this.XamlRoot)?.GetType()?.Name ?? "None";
                await mainViewModel.Model.Name.SetAsync(currentFocus);
            }
        }
    }

    private async void HandleHotKey2(object sender, KeyRoutedEventArgs e)
    {
        object dataContext = DataContext;
        if (dataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                bool handled = await mainViewModel.Model.HandleHotKey(sender, e.Key);
                if (handled)
                {
                    e.Handled = true;
                }
            }
        }
    }

    public async void HandleHotKey(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        object dataContext = DataContext;
        if (dataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                bool handled = await mainViewModel.Model.HandleHotKey(sender, args.KeyboardAccelerator.Key);
                if (handled)
                {
                    args.Handled = true;
                }
            }
        }
    }
}
