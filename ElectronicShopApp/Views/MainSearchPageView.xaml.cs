
using ElectronicShopApp.ViewModels;
using ElectronicShopApp.ViewModels.DTOs;
using Microsoft.Maui.Controls;
using Shared.DTO;
using System.Collections.Generic;
using System.Windows.Input;

namespace ElectronicShopApp.Views;

public partial class MainSearchPageView : ContentView
{
	public MainSearchPageView()
	{
		InitializeComponent();
	}

	// Bindable property to allow parent views or view models to set the products
	public static readonly BindableProperty ProductsProperty = BindableProperty.Create(
		propertyName: nameof(Products),
		returnType: typeof(List<ProductDTO>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: new List<ProductDTO>(),
		defaultBindingMode: BindingMode.OneWay);

	public List<ProductDTO> Products
	{
		get => (List<ProductDTO>)GetValue(ProductsProperty);
		set => SetValue(ProductsProperty, value);
	}

	// Bindable property for Categories
	public static readonly BindableProperty CategoriesProperty = BindableProperty.Create(
		propertyName: nameof(Categories),
		returnType: typeof(List<CategoryDTO>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: new List<CategoryDTO>(),
		defaultBindingMode: BindingMode.OneWay);

	public List<CategoryDTO> Categories
	{
		get => (List<CategoryDTO>)GetValue(CategoriesProperty);
		set => SetValue(CategoriesProperty, value);
	}

	// Bindable property for Filters (ViewModel wrapper type)
	public static readonly BindableProperty FiltersProperty = BindableProperty.Create(
		propertyName: nameof(Filters),
		returnType: typeof(List<FilterDTOViewModel>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: new List<FilterDTOViewModel>(),
		defaultBindingMode: BindingMode.OneWay);

	public List<FilterDTOViewModel> Filters
	{
		get => (List<FilterDTOViewModel>)GetValue(FiltersProperty);
		set => SetValue(FiltersProperty, value);
	}

	// Bindable property for CurrentState so parent can control which view is visible
	public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(
		propertyName: nameof(CurrentState),
		returnType: typeof(SearchPageState),
		declaringType: typeof(MainSearchPageView),
		defaultValue: SearchPageState.Categories,
		defaultBindingMode: BindingMode.TwoWay);

	public SearchPageState CurrentState
	{
		get => (SearchPageState)GetValue(CurrentStateProperty);
		set => SetValue(CurrentStateProperty, value);
	}
    public static readonly BindableProperty ProductClickCommandProperty = BindableProperty.Create(
        propertyName: nameof(ProductClickedCommand),
        returnType: typeof(ICommand),
        declaringType: typeof(MainSearchPageView),
        defaultValue: null,
        defaultBindingMode: BindingMode.OneWay);

    public ICommand ProductClickedCommand
    {
        get => (ICommand)GetValue(ProductClickCommandProperty);
        set => SetValue(ProductClickCommandProperty, value);
    }

    public static readonly BindableProperty CategoryClickCommandProperty = BindableProperty.Create(
		propertyName: nameof(CategoryClickCommand),
		returnType: typeof(Command<CategoryDTO>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay);

	public Command<CategoryDTO> CategoryClickCommand
	{
		get => (Command<CategoryDTO>)GetValue(CategoryClickCommandProperty);
		set => SetValue(CategoryClickCommandProperty, value);
	}
	public static readonly BindableProperty CategoryBackCommandProperty = BindableProperty.Create(
		propertyName: nameof(CategoryBackCommand),
		returnType: typeof(Command),
		declaringType: typeof(MainSearchPageView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay);

	public Command CategoryBackCommand
	{
		get => (Command)GetValue(CategoryBackCommandProperty);
		set => SetValue(CategoryBackCommandProperty, value);
	}
	public static readonly BindableProperty FilterClickedCommandProperty = BindableProperty.Create(
		propertyName: nameof(FilterClickedCommand),
		returnType: typeof(Command<FilterDTOViewModel>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay);

	public Command<FilterDTOViewModel> FilterClickedCommand
	{
		get => (Command<FilterDTOViewModel>)GetValue(FilterClickedCommandProperty);
		set => SetValue(FilterClickedCommandProperty, value);
	}

	public static readonly BindableProperty FilterOptionClickedCommandProperty = BindableProperty.Create(
		propertyName: nameof(FilterOptionClickedCommand),
		returnType: typeof(Command<FilterOptionDTOViewModel>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay);

	public Command<FilterOptionDTOViewModel> FilterOptionClickedCommand
	{
		get => (Command<FilterOptionDTOViewModel>)GetValue(FilterOptionClickedCommandProperty);
		set => SetValue(FilterOptionClickedCommandProperty, value);
	}
	public static readonly BindableProperty FilterSaveClickedCommandProperty = BindableProperty.Create(
		propertyName: nameof(FilterSaveClickedCommand),
		returnType: typeof(Command<FilterDTOViewModel>),
		declaringType: typeof(MainSearchPageView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay);

	public Command<FilterDTOViewModel> FilterSaveClickedCommand
	{
		get => (Command<FilterDTOViewModel>)GetValue(FilterSaveClickedCommandProperty);
		set => SetValue(FilterSaveClickedCommandProperty, value);
	}

	public static readonly BindableProperty FilterOptionsGridColumnDefinitionsProperty = BindableProperty.Create(
		propertyName: nameof(FilterOptionsGridColumnDefinitions),
		returnType: typeof(int[]),
		declaringType: typeof(MainSearchPageView),
		defaultValue: null,
		defaultBindingMode: BindingMode.OneWay);
	public int[] FilterOptionsGridColumnDefinitions
	{
		get => new int[] { 1, 1, 20, 20 };
	}
}
