using Microsoft.UI.Xaml.Input;

namespace UnoRepro.Presentation;

public partial record MainModel
{
    private int counter = 0;

    private INavigator _navigator;

    private ImmutableList<TabModel> _tabs = ImmutableList<TabModel>.Empty;

    public IListState<TabModel> Tabs => ListState.Value(this, () => _tabs);

    public IState<TabModel> SelectedTab => State<TabModel>.Value(this, () => default!);

    public MainModel(
        IStringLocalizer localizer,
        IOptions<AppConfig> appInfo,
        INavigator navigator)
    {
        _navigator = navigator;
        Title = "Main";
        Title += $" - {localizer["ApplicationName"]}";
        Title += $" - {appInfo?.Value?.Environment}";
    }

    public string? Title { get; }

    public IState<string> Name => State<string>.Value(this, () => string.Empty);

    public async Task<bool> HandleHotKey(object sender, Windows.System.VirtualKey key)
    {
        TabModel? selectedTab = await this.SelectedTab.Value();
        int selectedTabIndex = (selectedTab == null) ? -1 : _tabs.IndexOf(selectedTab);

        switch (key)
        {
            case Windows.System.VirtualKey.Q:
                await AddTab();
                return true;

            case Windows.System.VirtualKey.W:
                if (selectedTabIndex != -1)
                {
                    _tabs = _tabs.RemoveAt(selectedTabIndex);
                    await Tabs.UpdateAsync(_ => _tabs);
                    return true;
                }
                break;
        
            case Windows.System.VirtualKey.PageDown:
                if ((selectedTabIndex != -1) && (selectedTabIndex > 0))
                {
                    TabModel nextSelectedTab = _tabs[selectedTabIndex - 1];
                    if (nextSelectedTab != null)
                    {
                        await SelectedTab.UpdateAsync(_ => nextSelectedTab);
                        return true;
                    }
                }
                break;

            case Windows.System.VirtualKey.PageUp:
                if ((selectedTabIndex != -1) && (selectedTabIndex < _tabs.Count - 1))
                {
                    TabModel nextSelectedTab = _tabs[selectedTabIndex + 1];
                    if (nextSelectedTab != null)
                    {
                        await SelectedTab.UpdateAsync(_ => nextSelectedTab);
                        return true;
                    }
                }
                break;
        }
        return false;
    }

    public async Task AddTab()
    {
        TabModel newModel = new TabModel($"Tab {counter++}");
        _tabs = _tabs.Add(newModel);
        await Tabs.UpdateAsync(_ => _tabs);
    }
}
