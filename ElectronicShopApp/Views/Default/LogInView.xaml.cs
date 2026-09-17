using Shared.DTO;

namespace ElectronicShopApp.Views.Default;

public partial class LogInView : ContentView
{
	public LogInView()
	{
		InitializeComponent();
	}
    public static readonly BindableProperty EntryNameProperty = BindableProperty.Create(
        propertyName: nameof(EntryName),
        returnType: typeof(string),
        declaringType: typeof(LogInView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public string EntryName
    {
        get => (string)GetValue(EntryNameProperty);
        set => SetValue(EntryNameProperty, value);
    }
    public static readonly BindableProperty EntryPasswordProperty = BindableProperty.Create(
        propertyName: nameof(EntryPassword),
        returnType: typeof(string),
        declaringType: typeof(LogInView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public string EntryPassword
    {
        get => (string)GetValue(EntryPasswordProperty);
        set => SetValue(EntryPasswordProperty, value);
    }
}