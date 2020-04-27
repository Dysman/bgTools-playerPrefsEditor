# PlayerPrefs Editor for Unity 3D

[![Flattr this git repo](http://api.flattr.com/button/flattr-badge-large.png)](https://flattr.com/@dysman)

Tool extension for the Unity Editor that enables easy access to the player preferences over a simple UI. Allows to view, add, remove and modify entries on the development machine.

![Preference editor window](https://www.bgranzow.de/downloads/PlayerPrefsEditorV1.png)

## Features

* Add, remove and edit PlayerPrefs
* Intuitive visual editor
* Works with standard Unity PlayerPrefs
* Monitors changes from code
* Supports all editors (windows, linux, mac)
* Lightweight dockable for full integration in your workflow
* Supports both skins (personal, professional)

## Requirements

Unity Version: 2017.4.3 (LTS) or higher
Editor Version: Windows, MacOS, Linux

## Installation

The plugin provides *manual* and *UPM* installation.

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

The entry to open the _PlayerPrefs Editor_ is located in the menubar at Tools/BG Tools/Player Preferences Editor. It's a standard dockable window, so place it wherever it helps to be productive.
Into the settings it's possible to activate the 'monitoring of player preference changes'. This will detect changes and refresh the entries automatically to keep the view up-to-date.