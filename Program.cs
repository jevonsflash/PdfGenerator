using PdfGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerator
{
    public class Program
    {

        private static readonly string templatePath_Pdf = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"RecipeTemplate.pdf");
        private static readonly string outputPath_Pdf = System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, $"Recipe.pdf");
        public static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("Generator begin");

            var docinfo = GetRecipeDocInfo();
            var result = Exporter.ExportDocxByObject(templatePath_Pdf, docinfo);
            byte[] fileContent;
            result.Save(outputPath_Pdf);
            Console.WriteLine(outputPath_Pdf);
        }

        public static RecipeDocInfo GetRecipeDocInfo()
        {
            var docinfo = new RecipeDocInfo();
            docinfo.Id=202247589;
            docinfo.HospitalName="广东省某互联网医院";
            docinfo.DepartmentName="心血管内科";
            docinfo.AuditEmployeeName="王五";
            docinfo.AuditEmployeeSignature=System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"TestPic.jpg");
            docinfo.DraftEmployeeName="李四";
            docinfo.DraftEmployeeSignature=System.IO.Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "Assets", $"TestPic.jpg");
            docinfo.ClientName="张三";
            docinfo.ClientAge="35";
            docinfo.Name="良性高血压的处方";
            docinfo.ClientSex="男";
            docinfo.Price=12.0M;
            docinfo.RpList=new List<string>()
            {
                "1.苯磺酸氨氯地平片(京新)(省采)\n 规格：2mg*28片\t×10片\n用法用量：2片/次，每日三次，口服。",
                "2.苯磺酸氨氯地平片(络活喜)(非省采)\n 规格：5mg*28片\t×20片\n用法用量：1片/次，每日两次，口服。",
            };
            docinfo.Rps=string.Join('\n', docinfo.RpList);


            docinfo.StartTimeString="2022-9-21 12:00";
            return docinfo;
        }

    }
}
