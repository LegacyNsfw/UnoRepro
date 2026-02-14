using Microsoft.UI.Xaml.Input;

namespace UnoRepro.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        this.TextBox.GettingFocus += TextBox_GettingFocus;
    }

    private async void TextBox_GettingFocus(UIElement sender, GettingFocusEventArgs args)
    {
        object dataContext = DataContext;
        if (dataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                string currentFocus = args.OldFocusedElement?.GetType().Name ?? "None";
                await mainViewModel.Model.Name.SetAsync(currentFocus);
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
                await mainViewModel.Model.HotKey(args.KeyboardAccelerator.Key.ToString());
            }
        }
    }
}
