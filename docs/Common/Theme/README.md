## Common Theme

### Branding

| [Blue brand](https://www.brandcrowd.com/maker/logo/cyber-software-technology-462592?text=KDH&colorPalette=blue&isVariation=True) | [Orange brand](https://www.brandcrowd.com/maker/logo/cyber-software-technology-462592?text=KDH&colorPalette=orange&isVariation=True) |
| :------- | :-------------- |
| <img src="logo-blue.png" alt="drawing" height="100" width="100"/> | <img src="logo-orange.png" alt="drawing" height=100 width="100"/> |

---
### Local SHFB Web Site
The root output path of the Sandcastle Help File Builder Web Site documentation is: `C:\Temp\Documentation`.

- In Internet Information Services (IIS) Manager:
  - Add an Application Pool: **Documentation**\
  `.NET CLR=v4.0, Managed Pipeline=Integrated`
  - Add a Web Site named: **Documentation**\
  `Application pool=Documentation, Physical path=C:\Temp\Documentation, Binding Type=http, Port=8080`
  - Set IIS Authentication:\
  `Anonymous Authentication=Disabled, Windows Authentication=Enabled`
- In Windows Explorer:
  - Grant normal permissions to folder for local user:\
  `IIS APPPOOL\Documentation`.