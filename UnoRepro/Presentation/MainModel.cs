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

    public async Task AddTab()
    {
        TabModel newModel = new TabModel($"Tab {counter++}");
        _tabs = _tabs.Add(newModel);
        await Tabs.UpdateAsync(_ => _tabs);
        await SelectedTab.UpdateAsync(_ => newModel);
    }
}
