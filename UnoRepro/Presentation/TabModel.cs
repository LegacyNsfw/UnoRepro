using System;

namespace UnoRepro.Presentation;

public sealed class TabModel
{
    public TabModel(string text)
    {
        Text = text;
    }

    public string Text { get; }

    public override string ToString()
    {
        return "TabModel: " + Text;
    }
}
