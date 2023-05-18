# MasaFramework框架工具集

## MasaFramework项目架构实体文件快速生成工具

使用[MasaFramework框架](https://github.com/masastack/MASA.Framework)开发时，通常会使用DDD、读写分离、仓储、事件总线等理念将代码进行拆分，拆分必然涉及创建文件夹和文件；创建一个实体相关的文件夹和文件也是一笔不小的时间开销。

本工具通过预定义的模板文件，帮助开发者快速创建实体对应的文件夹和文件。

```
XXXXXXXXXXXXX> .\MasaPAG -h
Description:
  MasaFramework项目架构实体文件快速生成工具

Usage:
  MasaPAG [options]

Options:
  --output <output> (REQUIRED)          生成文件保存路径,不传默认为当前文件夹下的Output文件夹 [default:XXXXXX\Output]
  --pjname <pjname> (REQUIRED)          项目名称
  --entityname <entityname> (REQUIRED)  实体名称
  --localusing                          添加本地命名空间引用，默认为true，如果为false需要手动添加或者在全局引用类_Imports.cs中添加 [default: True]
  --version                             Show version information
  -?, -h, --help                        Show help and usage information
```


工具基于.Net 6开发，有运行环境可以下载源代码直接生成对应的执行程序。
如需要生成单独可运行的exe，可使用以下命令：
```
dotnet publish -c Release -r [runtime identifier] /p:PublishSingleFile=true /p:PublishTrimmed=true --self-contained true
```
在这个示例命令中，您需要将 [runtime identifier] 替换为目标操作系统的标识符。例如，要为 Windows x64 生成单独运行的可执行文件，您可以使用 win-x64 作为运行时标识符。


***使用示例：***
```
MasaPAG --pjname ABC.EShop --entityname Phone --output "C:\ProjectFiles"
```

