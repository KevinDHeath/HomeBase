![Static Badge](https://img.shields.io/badge/repo-homebase-blue?style=for-the-badge)
[![.NET Build](https://github.com/KevinDHeath/HomeBase/actions/workflows/dotnet.yml/badge.svg)](https://github.com/KevinDHeath/HomeBase/actions/workflows/dotnet.yml)\
![GitHub issues](https://img.shields.io/github/issues/KevinDHeath/HomeBase?style=plastic)
![GitHub last commit (by committer)](https://img.shields.io/github/last-commit/KevinDHeath/HomeBase?label=last%20commit&style=plastic)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/KevinDHeath/HomeBase?style=plastic)

# Contents
- [Common](src/Common/README.md)
- [EFCore](src/EFCore/README.md)
- [MVVM](src/MVVM/README.md)

References:
- [Customize the build by folder](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-by-directory)
- [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management)

>:warning: This repository uses the Sandcastle Help File Builder (SHFB) to generate documentation. You may experience issues opening the solution if you do not have the extension installed.\
See [Installation Instructions](https://ewsoftware.github.io/SHFB/html/8c0c97d0-c968-4c15-9fe9-e8f3a443c50a.htm) for details on how to install it and [Common.Theme](docs/Common/Theme/README.md) on how to setup a local website to view the builds output.

## GitHub Actions

### .NET Build
- Triggered _on-demand_ and allow for the selection of branch, runner _(i.e. Operating System)_, and working folder. Each run must be provided with a name.
- To build the entire solution the project _(or solution)_ folder should be set as `.`, to build a single project it must be the location of the project file, e.g. `./src/Json.Converters`
- Checks out the repository for the selected branch.
- Sets up the .NET 8.0 SDK.
- Restores any dependencies for the project _(or solution)_ based on the provided project folder.
- Executes the .NET CLI command to perform the build.