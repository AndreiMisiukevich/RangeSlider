# IMPORTANT: THIS CONTROL WAS MERGED INTO XamarinCommuninyToolkit

**I recommend you to use it**
https://github.com/xamarin/XamarinCommunityToolkit/pull/166

## RangeSlider
Range Slider control allows to select ranges in Xamarin.Forms apps.

![sample](https://github.com/AndreiMisiukevich/RangeSlider/blob/master/images/sample.gif?raw=true)

## Setup
* Available on NuGet: [RangeSlider](http://www.nuget.org/packages/RangeSlider) [![NuGet](https://img.shields.io/nuget/v/RangeSlider.svg?label=NuGet)](https://www.nuget.org/packages/RangeSlider)
* Since this plugin is 100% crossplatform, you need to add the nuget package to your Xamarin.Forms .NETSTANDARD project only.

```xaml
<ContentPage xmlns:range="clr-namespace:RangeSelection;assembly=RangeSelection">
  <range:RangeSlider />
</ContentPage>
```

## Sample
The sample you can find [here](https://github.com/AndreiMisiukevich/RangeSlider/tree/master/RangeSelectorViewSample/RangeSelectorViewSample)

## Supported Properties

| Name                        | Default Value | Description |
| --------------------------- | ----------- | ----------- |
| LowerValue                  | 0.0         | Current lower value |
| UpperValue                  | 1.0         | Current upper value |
| MinimumValue                | 0.0         | Minimum value |
| MaximumValue                | 1.0         | Maximum value |
| ThumbSize                   | 30.0        | Thumb size *(if you want to set the same value for Lower & Upper Thumbs)* |
| LowerThumbSize              | -1.0        | Lower Thumb size |
| UpperThumbSize              | -1.0        | Upper Thumb size |
| TrackSize                   | 3.0         | Track size |
| ThumbColor                  | Default     | Thumb color *(if you want to set the same value for Lower & Upper Thumbs)* |
| LowerThumbColor             | Default     | Lower Thumb color |
| UpperThumbColor             | Default     | Upper Thumb color |
| TrackColor                  | Default     | Track color |
| TrackHighlightColor         | Default     | Track highlight color |
| ThumbBorderColor            | Default     | Thumb border color *(if you want to set the same value for Lower & Upper Thumbs)* |
| LowerThumbBorderColor       | Default     | Lower Thumb border color |
| UpperThumbBorderColor       | Default     | Upper Thumb border color |
| TrackBorderColor            | Default     | Track border color |
| TrackHighlightBorderColor   | Default     | Track highlight border color |
| ValueLabelStyle             | Default     | Value label style *(if you want to set the same value for Lower & Upper Thumbs)* |
| LowerValueLabelStyle        | null        | Lower Value label style |
| UpperValueLabelStyle        | null        | Upper Value label style |
| ValueLabelStringFormat      | {0:0.##}    | Value Label String Format |
| LowerThumbView              | null        | Lower Thumb View |
| UpperThumbView              | null        | Upper Thumb View |
| ValueLabelSpacing           | 5.0         | Value label spacing *(from thumb to value label)* |
| ThumbRadius                 | -1.0        | Thumb radius *(if you want to set the same value for Lower & Upper Thumbs)* |
| LowerThumbRadius            | -1.0        | Lower Thumb radius |
| UpperThumbRadius            | -1.0        | Upper Thumb radius |
| TrackRadius                 | -1.0        | Track radius |

## License
The MIT License (MIT) see [License file](LICENSE)

## Contribution
Feel free to create issues and PRs 😃
