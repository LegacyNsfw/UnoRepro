using System.Threading.Tasks;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace UnoRepro.Presentation;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        this.TextBox.GettingFocus += TextBox_GettingFocus;
//        this.TabView.AddHandler(UIElement.KeyDownEvent, new KeyEventHandler(HandleKeyDown), false);
    }

    private async void TextBox_GettingFocus(UIElement sender, GettingFocusEventArgs args)
    {
        object dataContext = DataContext;
        if (dataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                if (args.OldFocusedElement == null)
                {
                    await mainViewModel.Model.Name.SetAsync("OldFocusedElement is null");
                }
                else if (this.XamlRoot == null)
                {
                    await mainViewModel.Model.Name.SetAsync("No Xaml Root");
                }
                else
                {
                    var focusedElement = FocusManager.GetFocusedElement(this.XamlRoot);
                    if (focusedElement == null)
                    {
                        await mainViewModel.Model.Name.SetAsync("FocusManager.GetFocusedElement(xamlRoot) returned null");
                    }
                    else
                    {
                        string currentFocus = focusedElement?.GetType()?.Name ?? "None";
                        await mainViewModel.Model.Name.SetAsync(currentFocus);
                    }
                }
            }
        }
    }

    private async void HandleKeyDown(object sender, KeyRoutedEventArgs e)
    {
        object dataContext = DataContext;
        if (dataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                bool handled = await mainViewModel.Model.HandleAccelerator("HandleKeyDown", e.Key);
                if (handled)
                {
                    e.Handled = true;
                }
            }
        }
    }

    public async void HandleAccelerator(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
    {
        if (args.KeyboardAccelerator.Modifiers != VirtualKeyModifiers.Control)
        {
            return;
        }

        if (DataContext is MainViewModel mainViewModel)
        {
            if (mainViewModel.Model != null)
            {
                bool handled = await mainViewModel.Model.HandleAccelerator(sender, args.KeyboardAccelerator.Key);
                if (handled)
                {
                    args.Handled = true;
                }
            }
        }
    }

    public async void OnTabCloseRequested(object sender, TabViewTabCloseRequestedEventArgs args)
    {
        if (DataContext is MainViewModel mainViewModel)
        {
            TabModel? tabModel = args.Tab?.DataContext as TabModel;
            if (tabModel != null)
            {
                await mainViewModel.Model.OnCloseTabRequested(tabModel);
            }
        }
    }

}
