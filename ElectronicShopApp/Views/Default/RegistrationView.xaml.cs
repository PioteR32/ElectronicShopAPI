namespace ElectronicShopApp.Views.Default;

public partial class RegistrationView : ContentView
{
	public RegistrationView()
	{
		InitializeComponent();
	}
    public static readonly BindableProperty EntryEmailProperty = BindableProperty.Create(
        propertyName: nameof(EntryEmail),
        returnType: typeof(string),
        declaringType: typeof(RegistrationView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public string EntryEmail
    {
        get => (string)GetValue(EntryEmailProperty);
        set => SetValue(EntryEmailProperty, value);
    }
    public static readonly BindableProperty EntryPasswordProperty = BindableProperty.Create(
        propertyName: nameof(EntryPassword),
        returnType: typeof(string),
        declaringType: typeof(RegistrationView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public string EntryPassword
    {
        get => (string)GetValue(EntryPasswordProperty);
        set => SetValue(EntryPasswordProperty, value);
    }
    public static readonly BindableProperty PasswordAgainProperty = BindableProperty.Create(
        propertyName: nameof(PasswordAgain),
        returnType: typeof(string),
        declaringType: typeof(RegistrationView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public string PasswordAgain
    {
        get => (string)GetValue(PasswordAgainProperty);
        set => SetValue(PasswordAgainProperty, value);
    }
    public static readonly BindableProperty PhoneNumProperty = BindableProperty.Create(
        propertyName: nameof(PhoneNum),
        returnType: typeof(string),
        declaringType: typeof(RegistrationView),
        defaultValue: string.Empty,
        defaultBindingMode: BindingMode.TwoWay);

    public string PhoneNum
    {
        get => (string)GetValue(PhoneNumProperty);
        set => SetValue(PhoneNumProperty, value);
    }
}