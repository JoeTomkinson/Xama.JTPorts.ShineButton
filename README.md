# Xama.JTPorts.ShineButton
[![platform](https://img.shields.io/badge/platform-Xamarin.Android-brightgreen.svg)](https://www.xamarin.com/)
[![API](https://img.shields.io/badge/API-14%2B-orange.svg?style=flat)](https://android-arsenal.com/api?level=14s)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
![Build: Passing](https://img.shields.io/badge/Build-Passing-green.svg)
[![NuGet](https://img.shields.io/nuget/v/Xama.JTPorts.ShineButton.svg?label=NuGet)](https://www.nuget.org/packages/Xama.JTPorts.ShineButton/)

>Xamarin.Android Native UI lib button effects like "shining".

_Xamarin.Android_ Native "Shine Button" control. This is a UI lib for Xamarin Android. Creates 'shining' effects. Initial port from [ShineButton](https://github.com/ChadCSong/ShineButton) by [Chad Song](https://github.com/ChadCSong)

This is a ported build, converted from Java to C# for use with the Xamarin MonoFramework. There are only a couple of new additions from the original library currently.

<br>

![!gif](https://github.com/DigitalSa1nt/Xama.JTPorts.ShineButton/blob/master/images/20190216_225431.gif?raw=true)

<br>

# Installation

![NuGetIcon](https://raw.githubusercontent.com/DigitalSa1nt/Xama.JTPorts.ShineButton/master/images/nugetIcon.png)

This can be installed to any Xamarin Android project by adding the [NuGet package](https://www.nuget.org/packages/Xama.JTPorts.ShineButton/) to your solution. This library has been migrated to AndroidX Support libraries so it may ask you to install those dependencies if you have not migrated across to AndroidX yet.

Package Manager:
> Install-Package Xama.JTPorts.ShineButton -Version 1.0.1

.NET CLI:
> dotnet add package Xama.JTPorts.ShineButton --version 1.0.1

# Basic usage

The Icon shape is made from png mask and the ported library preferred RAW formats, however you can use normal images, just bear in mind that as it masks an image you'll lose details, so use cut-out images instead, similar to the animated gif above.

Create programatically and then simply add to your view:

```cs
         ShineButtonControl shineButtonControl = new ShineButtonControl(this);
         shineButtonControl.ButtonColour = Color.Gray;
         shineButtonControl.ButtonFillColour = Color.Red;
         shineButtonControl.ShapeResource = Resource.Raw.heart;
         shineButtonControl.AllowRandomColour = true;

         var layoutParams = new LinearLayout.LayoutParams(100, 100);
         shineButtonControl.LayoutParameters = layoutParams;
```

Or simply define in AXML as such:

```cs
<ShineButton.Classes.ShineButtonControl
         android:layout_width="50dp"
         android:layout_height="50dp"
         android:layout_marginBottom="50dp"
         android:layout_centerInParent="true"
         android:id="@+id/po_like"
         app:btn_color="@android:color/darker_gray"
         app:btn_fill_color="#0f4cad"
         app:big_shine_color="#8094e5"
         app:click_animation_duration="200"
         app:shine_animation_duration="1500"
         app:shine_turn_angle="10"
         app:small_shine_offset_angle="20"
         app:shine_distance_multiple="1.5"
         app:small_shine_color="#CC9999"
         app:shine_count="8"
         app:shine_size="30dp"
         app:siShape="@raw/like" />
```

# Available Attributes

```cs
<attr name="btn_color" format="color" />
<attr name="btn_fill_color" format="color" />
<attr name="shine_count" format="integer" />
<attr name="shine_turn_angle" format="float" />
<attr name="small_shine_offset_angle" format="float" />
<attr name="enable_flashing" format="boolean" />
<attr name="allow_random_color" format="boolean" />
<attr name="small_shine_color" format="color" />
<attr name="big_shine_color" format="color" />
<attr name="shine_animation_duration" format="integer" />
<attr name="click_animation_duration" format="integer" />
<attr name="shine_distance_multiple" format="float" />
<attr name="shine_size" format="dimension" />
```

# Dependencies

[Xama.JTPorts.EasingInterpolator](https://github.com/DigitalSa1nt/Xama.JTPorts.EasingInterpolator)

# Useful?

<a href="https://www.buymeacoffee.com/JTT" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-red.png" alt="Buy Me A Coffee" tyle="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>

_You know, only if you want to._
