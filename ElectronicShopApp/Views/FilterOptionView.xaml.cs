using ElectronicShopApp.ViewModels.DTOs;

namespace ElectronicShopApp.Views;

public partial class FilterOptionView : ContentView
{
	public FilterOptionView()
	{
		InitializeComponent();
	}

	public static readonly BindableProperty FilterOptionProperty =
		BindableProperty.Create(nameof(FilterOption), typeof(FilterOptionDTOViewModel), typeof(FilterOptionView), null);
	public FilterOptionDTOViewModel FilterOption
	{
		get => (FilterOptionDTOViewModel)GetValue(FilterOptionProperty);
		set => SetValue(FilterOptionProperty, value);
	}
	public static readonly BindableProperty ShowFilterOptionsCommandProperty 
		= BindableProperty.Create(nameof(ShowFilterOptionsCommand), typeof(Command<FilterOptionDTOViewModel>), typeof(FilterOptionView), null);

	public Command<FilterOptionDTOViewModel> ShowFilterOptionsCommand
	{
		get => (Command<FilterOptionDTOViewModel>)GetValue(ShowFilterOptionsCommandProperty);
		set => SetValue(ShowFilterOptionsCommandProperty, value);
	}

	public static readonly BindableProperty ShowFilterOptionsCommandParameterProperty
		= BindableProperty.Create(nameof(ShowFilterOptionsCommandParameter), typeof(FilterOptionDTOViewModel), typeof(FilterOptionView), null);

	public Command<FilterOptionDTOViewModel> ShowFilterOptionsCommandParameter
	{
		get => (Command<FilterOptionDTOViewModel>)GetValue(ShowFilterOptionsCommandParameterProperty);
		set => SetValue(ShowFilterOptionsCommandParameterProperty, value);
	}
	//public static readonly BindableProperty IsSelectedProperty =
	//	BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(FilterOptionView), false);
	//public bool IsSelected
	//{
	//	get => (bool)GetValue(IsSelectedProperty);
	//	set => SetValue(IsSelectedProperty, value);
	//}

	//public static readonly BindableProperty IsEnabledProperty =
	//	BindableProperty.Create(nameof(IsEnabled), typeof(bool), typeof(FilterOptionView), true);
	//public new bool IsEnabled
	//{
	//	get => (bool)GetValue(IsEnabledProperty);
	//	set => SetValue(IsEnabledProperty, value);
	//}

	//public static readonly BindableProperty HaveOptionsProperty =
	//	BindableProperty.Create(nameof(HaveOptions), typeof(bool), typeof(FilterOptionView), false);

	//public bool HaveOptions
	//{
	//	get => (bool)GetValue(HaveOptionsProperty);
	//	set => SetValue(HaveOptionsProperty, value);
	//}


}