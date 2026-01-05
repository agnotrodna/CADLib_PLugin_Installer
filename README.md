# CADLib Plugin Installer

A simple Windows Forms application that allows users to install plugins for CADLib with a single click. The application requires administrator privileges to function properly.

## Features

- Scans all available plugins from the application's directory
- Compares available plugins with installed ones in CADLib
- One-click installation of selected plugins
- Visual indication of already installed plugins
- Ability to select the main DLL file for each plugin before installation
- Automatic modification of plugins.xml to register installed plugins

## Requirements

- Windows operating system
- .NET Framework 4.7.2 or higher
- Administrator privileges to install plugins to CADLib directory
- CADLib Model Studio CS 3.1 installed at the default location (C:\\Program Files\\CSoft\\Model Studio CS\\3.1\\MIA\\bin)

## Installation

1. Download the compiled executable or build from source
2. Place the application in a directory with a "Plugins" subdirectory containing your plugin folders
3. Run the application as an administrator

## Usage

1. Launch the application with administrator rights
2. The application will scan for available plugins in the "Plugins" subdirectory
3. Plugins that are already installed will be marked as such
4. Select the plugins you want to install by checking the checkboxes
5. For each selected plugin, click "Выбрать DLL" (Select DLL) to choose the main DLL file for the plugin
6. Click the Install button to install all selected plugins
7. The application will copy plugin files to the CADLib plugins directory and update plugins.xml accordingly

## Project Structure

- `Form1.cs`: Main form implementation with plugin scanning and installation logic
- `Program.cs`: Application entry point
- `PluginInstaller.csproj`: Project configuration file
- `Plugins` directory: Contains plugin folders (to be created by user)

## How It Works

The application works by:
1. Reading plugin folders from the local "Plugins" directory
2. Checking if plugins are already installed in the CADLib directory
3. Allowing users to select plugins and specify their main DLL files
4. Copying selected plugin directories to the CADLib plugins folder
5. Updating the plugins.xml file to register the new plugins

## Screenshots

Main application window:
<img width="508" height="348" alt="image" src="https://github.com/user-attachments/assets/eedd51f9-16bf-4c05-8e48-063fb777f539" />

DLL selection dialog:
<img width="516" height="340" alt="image" src="https://github.com/user-attachments/assets/82cfa1f3-58d9-43f5-8fe4-f44fc8effbbd" />

## Troubleshooting

- Make sure to run the application as an administrator
- Ensure CADLib is installed in the default location
- Verify that plugin folders contain the necessary files
- Check that the "Plugins" directory exists in the application folder

## License

This project includes a LICENSE file. Please see the LICENSE file for licensing information.
