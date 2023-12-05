![Static Badge](https://img.shields.io/badge/repo-homebase-blue?style=for-the-badge)\
![GitHub issues](https://img.shields.io/github/issues/KevinDHeath/HomeBase?style=plastic)
![GitHub last commit (by committer)](https://img.shields.io/github/last-commit/KevinDHeath/HomeBase?label=last%20commit&style=plastic)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/KevinDHeath/HomeBase?style=plastic)\
[![.NET Build](https://github.com/KevinDHeath/HomeBase/actions/workflows/dotnet.yml/badge.svg)](https://github.com/KevinDHeath/HomeBase/actions/workflows/dotnet.yml)

## Contents
- [Common](src/Common/README.md)
- [EFCore](src/EFCore/README.md)
- [MVVM](src/MVVM/README.md)

>:warning: This repository uses the Sandcastle Help File Builder (SHFB) to generate documentation. You may experience issues opening the solution if you do not have the extension installed.\
See [Installation Instructions](https://ewsoftware.github.io/SHFB/html/8c0c97d0-c968-4c15-9fe9-e8f3a443c50a.htm) for details on how to install it.

References:
- [Customize the build by folder](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory)
- [Central Package Management (CPM)](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)
- [Markdown Emojis](https://github.com/ikatyang/emoji-cheat-sheet/blob/master/README.md) :high_brightness:

## GitHub Actions
The available Actions are only triggered _on-demand_ and allow for the selection of branch, runner _(i.e. Operating System)_, and working folder. Each run must be provided with a name.\
To build the entire solution the working folder should be set as '`.`', to build a single project set it can be the location of the project file, e.g. '`./src/Common/Core`'.

- **.NET Build**
1. Checks out the repository for the selected branch.
2. Sets up the .NET 8.0 SDK.
3. Creates a temporary `packages` folder.
4. Restores any dependencies for the project _(or solution)_ based on the working directory provided as an input variable.
5. Executes the .NET CLI command to perform the build.
6. Uploads the package artifacts as `HomeBase-Packages` with a retention period of 2 days.

- **Log Environment**\
_This demonstrates the different ways to retrieve environment variables depending on the runner platform._
1. Gets the selected branch details.
2. Echoes the variables when running on a Linux platform.
3. Echoes the variables when running on a Windows platform.
