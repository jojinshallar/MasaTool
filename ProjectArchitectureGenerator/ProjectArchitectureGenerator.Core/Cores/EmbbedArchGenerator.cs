using ProjectArchitectureGenerator.Core.Cores.TemplateDatas;
using System.Reflection;

namespace MasaArchitectureGenerator.Cores
{
    public class EmbbedArchGenerator
    {
        const string LiquidExtend = ".liquid";
        const string EntityFlag = "$Entity$";
        const string EmbbedEntityFlag = "_Entity_";

        public static async Task GenerateAsync(string projectName, string entityName, DirectoryInfo outputFolder, bool includeLocalUsing = true, string idtype = nameof(Guid), string useridtype = "int")
        {
            EntityTemplateData data = new EntityTemplateData(projectName, entityName, idtype, useridtype, includeLocalUsing);
            Assembly assembly = typeof(EmbbedArchGenerator).Assembly;
            List<string> files = assembly.GetManifestResourceNames().Where(p => p.EndsWith(LiquidExtend)).ToList();
            foreach (string file in files)
            {
                //读取模板文件内容
                string templateContent = await ReadEmbbedLiquidFileAsync(file, assembly);
                //将数据应用到模板文件，获取输出文本
                string result = await RenderLiquidContent(templateContent, data);
                //保存内容
                FileInfo outputFileInfo = GetOutputFileInfo(file, outputFolder, entityName, assembly);
                if (!outputFileInfo.Directory.Exists)
                {
                    outputFileInfo.Directory.Create();
                }
                await File.WriteAllTextAsync(outputFileInfo.FullName, result.ToString());
            }
        }

        static FileInfo GetOutputFileInfo(string embbedFilePath, DirectoryInfo outputFolder, string entityname, Assembly assembly)
        {
            string relativeFolderFilePath = GetRelativeFolderFilePath(embbedFilePath, assembly);
            //嵌入资源的路径中，会把文件夹名中的$转成_，所以$Entity$会变为_Entity_
            string outputFile = Path.Combine(outputFolder.FullName, relativeFolderFilePath.Replace(LiquidExtend, ".cs")).Replace(EntityFlag, entityname).Replace(EmbbedEntityFlag, entityname);
            return new FileInfo(outputFile);
        }

        static async Task<string> RenderLiquidContent(string templateContent, EntityTemplateData data)
        {
            var template = Scriban.Template.Parse(templateContent);
            return await template.RenderAsync(new { model = data });
        }

        static async Task<string> ReadEmbbedLiquidFileAsync(string filePath, Assembly assembly)
        {
            using (Stream stream = assembly.GetManifestResourceStream(filePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        static string GetRelativeFolderFilePath(string embbedFilePath, Assembly assembly)
        {
            string rn = assembly.GetName().Name;
            string relativeFilePath = embbedFilePath.Replace(rn + ".FileTemplates.", "");
            //获取倒数第二个.的位置
            int index = relativeFilePath.LastIndexOf(".", relativeFilePath.LastIndexOf(".") - 1);
            return relativeFilePath.Substring(0, index).Replace('.', Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar.ToString() + relativeFilePath.Substring(index + 1);
        }
    }
}
