# Basic Bind (for Unity)

![](example.gif)

*One-way MVVM style data-binding for Unity.*

Basic Bind is a simple data binding system which enables one-way data binding from observable model data to view components.


## Installation
### Manual Import
1. Clone or dowload this repository somewhere in your Unity project's `Assets` folder.

## Getting Started
* Explore the example scene in the `Example` folder.
  * The scene contains a View Model game object with a ViewModel component. Change the values on the component to observe changes in the scnee.
  * The scene contains a few sample bindings on single game objects as well as a slightly more complex list example which binds prefab instances to a list of configuration data.

## Usage
To get started data-binding, first define a new class which extends `ViewModel`.
ViewModels are Unity MonoBehaviours which should contain several `DataSource` or `DataSourceList` fields.

Example:
```
public class ProfileViewModel : ViewModel
{
    public StringDataSource Username;
    public SpriteDataSource Avatar;
}
```

At runtime, data source values can be accessed via the `Value` property.
```
var viewModel = gameObject.GetComponent<ProfileViewModel>();
viewModel.Username.Value = "Ben";
viewModel.Avatar.Value = someSprite;
```

To create a binding between a component and a ViewModel, create a new `DataBinding` component.
DataBinding should contain `DataSourceReference` fields which allow for the binding and serialization of data-binding links.
DataBinding classes use the `Setup` method to create bindings.
You can use the `[AllowedDataTypes]` attribute to help the inspector filter a ViewModel's data sources by type.

Example:
```
public class TextFieldDataBinding : DataBinding
{
    public Text TextField;
    [AllowedDataTypes(typeof(string))] public DataSourceReference StringSource;

    protected override void Setup()
    {
        Bind(StringSource, OnValueChanged);
    }

    private void OnValueChanged(object sender, EventArgs eventArgs)
    {
        if (StringSource != null && StringSource.TryGetValue<string>(out var value))
        {
            if (TextField) TextField.text = value;
        }
    }
}
```
