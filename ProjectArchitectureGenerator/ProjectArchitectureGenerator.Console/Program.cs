using MasaArchitectureGenerator.Cores;
using System.CommandLine;
using System.Diagnostics;
using System.Runtime.InteropServices;

async Task<int> InitCommand(string[] args)
{
    Option<DirectoryInfo> folderOption = BuildFolderOption();
    Option<string> projectNameOption = BuildProjectNameOption();
    Option<string> entityNameOption = BuildEntityNameOption();
    Option<bool> localUsingOption = BuildLocalUsingOption();
    Option<string> idTypeOption = BuildIdTypeOption();
    var rootCommand = new RootCommand("MasaFramework项目架构实体文件快速生成工具");
    rootCommand.AddOption(folderOption);
    rootCommand.AddOption(projectNameOption);
    rootCommand.AddOption(entityNameOption);
    rootCommand.AddOption(localUsingOption);
    rootCommand.AddOption(idTypeOption);

    rootCommand.SetHandler(async (folder,projectName, entityName,localUsing, idType) =>
    {
        await EmbbedArchGenerator.GenerateAsync(projectName, entityName, folder, localUsing, idType);
        OpenFolder(folder);
    }, folderOption,projectNameOption, entityNameOption,localUsingOption, idTypeOption);
    return await rootCommand.InvokeAsync(args);
}

void OpenFolder(DirectoryInfo directoryInfo)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        Process.Start("explorer.exe", directoryInfo.FullName);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        Process.Start("xdg-open", directoryInfo.FullName);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        Process.Start("xdg-open", directoryInfo.FullName);
    }
}

Option<DirectoryInfo> BuildFolderOption()
{
    return new Option<DirectoryInfo>(
        name: "--output",
        description: "生成文件保存路径,不传默认为当前文件夹下的Output文件夹",
        isDefault: true,
        parseArgument: result =>
        {
            if (result.Tokens.Count == 0)
            {
                return new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Output");
            }
            string filePath = result.Tokens.FirstOrDefault()?.Value;
            if (!IsValidFolderPath(filePath))
            {
                result.ErrorMessage = $"传入的文件输出路径{filePath}不合法";
                return null;
            }
            return new DirectoryInfo(filePath);
        }){ IsRequired=true};
}

Option<string> BuildProjectNameOption()
{
    return new Option<string>(
        name: "--pjname",
        description: "项目名称",
        parseArgument: result =>
        {
            if (result.Tokens.Count == 0)
            {
                result.ErrorMessage = $"项目名称不能为空（--pjname 项目名称）";
                return null;
            }
            string projectName = result.Tokens.FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(projectName))
            {
                result.ErrorMessage = $"项目名称不能为空（--pjname 项目名称）";
                return null;
            }
            if (!IsValidProjectName(projectName))
            {
                result.ErrorMessage = $"项目名称{projectName}不合法";
                return null;
            }
            return projectName;
        }){ IsRequired = true };
}

Option<string> BuildEntityNameOption()
{
    return new Option<string>(
        name: "--entityname",
        description: "实体名称",
        parseArgument: result =>
        {
            if (result.Tokens.Count == 0)
            {
                result.ErrorMessage = $"实体名称不能为空";
                return null;
            }
            string entityName = result.Tokens.FirstOrDefault()?.Value;
            if (string.IsNullOrWhiteSpace(entityName))
            {
                result.ErrorMessage = $"实体名称不能为空";
                return null;
            }
            if (!IsValidClassName(entityName))
            {
                result.ErrorMessage = $"实体名称{entityName}不合法";
                return null;
            }
            return entityName;
        }){ IsRequired = true };
}

Option<bool> BuildLocalUsingOption()
{
    return new Option<bool>(
        name: "--localusing",
        description: "添加本地命名空间引用，默认为true，如果为false需要手动添加或者在全局引用类_Imports.cs中添加",
        getDefaultValue: () => true);
}

Option<string> BuildIdTypeOption()
{
    return new Option<string>(
        name: "--idtype",
        description: "实体类唯一标识类型，默认为Guid",
        getDefaultValue: () => "Guid");
}

bool IsValidFolderPath(string path)
{
    if (!Path.IsPathRooted(path))
    {
        return false;
    }

    char[] invalidChars = Path.GetInvalidPathChars();
    foreach (char invalidChar in invalidChars)
    {
        if (path.Contains(invalidChar))
        {
            return false;
        }
    }

    return true;
}

bool IsValidProjectName(string name)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        return false;
    }

    if (!char.IsLetter(name[0]) && name[0] != '_' && name[0] != '.')
    {
        return false;
    }

    string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.";
    if (name.Any(c => !allowedChars.Contains(c)))
    {
        return false;
    }

    return true;
}
bool IsValidClassName(string name)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        return false;
    }

    if (!char.IsLetter(name[0]) && name[0] != '_')
    {
        return false;
    }

    string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
    if (name.Any(c => !allowedChars.Contains(c)))
    {
        return false;
    }

    return true;
}

await InitCommand(args);