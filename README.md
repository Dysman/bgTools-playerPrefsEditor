# PlayerPrefs Editor for Unity 3D

[![Flattr this git repo](http://api.flattr.com/button/flattr-badge-large.png)](https://flattr.com/@dysman)

Tool extension for the Unity Editor that enables easy access to the player preferences over a simple UI. Allows to view, add, remove and modify entries on the development machine.

![Preference editor window](https://www.bgranzow.de/downloads/PlayerPrefsEditorV1_0_1.png)

## Features

* Add, remove and edit PlayerPrefs
* Intuitive visual editor
* Works with standard Unity PlayerPrefs
* Monitors changes from code
* Supports all editors (Windows, Linux, MacOS)
* Lightweight dockable for full integration in your workflow
* Supports both skins (Personal, Professional)

## Requirements

Unity Version: 2017.4.3 (LTS) or higher  
Editor Version: Windows, MacOS, Linux

## Installation

The plugin provides *manual* and *UPM* installation.


Additionally it's available on the [Unity Asset Store](http://u3d.as/1RLa).

### Manual
Place the PlayerPrefsEditor folder somewhere in your project. It's not relevant where it's located, the plugin will find all of its files by itself.

### Unity Package Manager (UPM)
Through the Unity Plugin Manager it's possible to install the plugin direct from this git repository.
The UPM need a specific structure what will be provided into the *upm* branch.

Use following direct URL for the configuration:
```
git@github.com:Dysman/bgTools-playerPrefsEditor.git#upm
```
See official Unity documentation for more informations: [UI](https://docs.unity3d.com/Manual/upm-ui-giturl.html) or [manifest.json](https://docs.unity3d.com/Manual/upm-git.html)

## Usage

The entry to open the _PlayerPrefs Editor_ is located in the top menu at Tools/BG Tools/PlayerPrefs Editor. It's a standard dockable window, so place it wherever it helps to be productive.
A more detailed manual can be fund in following locations:
* GitHub (Manual)- [Manual page](Packages/PlayerPrefsEditor/Documentation~/PlayerPrefsEditor.md)
* GitHub (UPM) - Press the _Documentation_ link on the UPM description.
* Unity Asset Store Package - [MANUAL.html](MANUAL.html)