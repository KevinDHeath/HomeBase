## Common Theme

### Branding

 | GitHub<br/>[Orange brand](https://www.brandcrowd.com/maker/logo/cyber-software-technology-462592?text=KDH&colorPalette=orange&isVariation=True) | NuGet<br/>[Blue brand](https://www.brandcrowd.com/maker/logo/cyber-software-technology-462592?text=KDH&colorPalette=blue&isVariation=True)
| -------------- | -------------- |
| <img src="logo-orange.png" alt="drawing" height=100 width="100"/> | <img src="logo-blue.png" alt="drawing" height="100" width="100"/> |

---
### Local SHFB Web Site
The root output path of the Sandcastle Help File Builder website help format builds are configured as: `C:\Temp\Documentation`\
To setup a single website with links to each project follow these steps:

- In Internet Information Services (IIS) Manager:
  - Add an Application Pool: `Documentation`\
  `.NET CLR=v4.0, Managed Pipeline=Integrated`
  - Add a Website named: `Documentation`\
  `Application pool=Documentation, Physical path=C:\Temp\Documentation, Binding Type=http, Port=8080`
  - Set IIS Authentication:\
  `Anonymous Authentication=Disabled, Windows Authentication=Enabled`
- In Windows Explorer:
  - Grant normal permissions to the `C:\Temp\Documentation` folder\
  for the local user: `IIS APPPOOL\Documentation`.

Finally, copy the following items from this folder to `C:\Temp\Documentation`
``` console
CommonProjects
icon-orange.ico
index.html
```
> **Note**: The index.html page _(and SHFB)_ uses [Bulma: the modern
CSS framework that
just works.](https://bulma.io/)