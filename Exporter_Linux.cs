using Flurl.Http;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using PdfSharpCore.Pdf.IO;
using System.Reflection;


namespace PdfGenerator
{
    public class Exporter_Linux
    {
        public static PdfDocument ExportDocxByObject(Stream templateStream, object data)
        {
            var doc = PdfReader.Open(templateStream, PdfDocumentOpenMode.Modify);
            ReplaceKeyObjetAsync(doc, data);
            return doc;
        }

        public static PdfDocument ExportDocxByObject(string templatePath, object data)
        {
            var doc = PdfReader.Open(templatePath, PdfDocumentOpenMode.Modify);
            ReplaceKeyObjetAsync(doc, data);
            return doc;
        }

        private static void ReplaceKeyObjetAsync(PdfDocument doc, object model)
        {
            PdfAcroForm form = doc.AcroForm;
            if (form.Elements.ContainsKey("/NeedAppearances"))
            {
                form.Elements["/NeedAppearances"] = new PdfBoolean(true);
            }
            else
            {
                form.Elements.Add("/NeedAppearances", new PdfBoolean(true));
            }
            string text = "";
            Type t = model.GetType();
            PropertyInfo[] pi = t.GetProperties();

            foreach (var fieldName in form.Fields.Names)
            {
                var run = form.Fields[fieldName] as PdfTextField;
                text = run.Name;
                XPdfFontOptions options = new XPdfFontOptions(PdfFontEncoding.Unicode);

                XFont font = new XFont(GlobalFontSettings.FontResolver.DefaultFontName, 18, XFontStyle.Regular, options);
                run.Font=font;
                foreach (PropertyInfo p in pi)
                {
                    string key = $"${p.Name}$";
                    if (text.Contains(key))
                    {
                        var value = "";
                        try
                        {
                            value =  p.GetValue(model, null).ToString();
                        }
                        catch (Exception ex)
                        {
                        }

                        if (value.Contains('\n'))
                        {
                            run.MultiLine = true;

                        }

                        run.Value = new PdfString(value,PdfStringEncoding.Unicode);
                        run.ReadOnly = true;
                    }
                    else if (text.Contains($@"#{p.Name}#"))
                    {

                        try
                        {
                            var filePath = p.GetValue(model, null) as string;

                            if (string.IsNullOrEmpty(filePath))
                            {
                                continue;
                            }
                            if (File.Exists(filePath))
                            {
                                using (var fileStream = new FileStream(filePath.ToString(), FileMode.Open, FileAccess.Read))
                                {
                                    var rectangle = run.Elements.GetRectangle("/Rect");
                                    var xForm = new XForm(doc, rectangle.Size);
                                    using (var xGraphics = XGraphics.FromPdfPage(doc.Pages[0]))
                                    {
                                        var image = XImage.FromStream(()=>fileStream);
                                        xGraphics.DrawImage(image, rectangle.ToXRect() +new XPoint(0, 400));
                                        var state = xGraphics.Save();
                                        xGraphics.Restore(state);
                                    }

                                }
                            }
                            else
                            {

                                using (var fileStream = filePath.ToString().GetStreamAsync().Result)
                                {

                                    var rectangle = run.Elements.GetRectangle("/Rect");
                                    var xForm = new XForm(doc, rectangle.Size);
                                    using (var xGraphics = XGraphics.FromPdfPage(doc.Pages[0]))
                                    {
                                        var image = XImage.FromStream(() => fileStream);
                                        xGraphics.DrawImage(image, rectangle.ToXRect() +new XPoint(0, 400));
                                        var state = xGraphics.Save();
                                        xGraphics.Restore(state);
                                    }
                                }

                            }
                            run.ReadOnly = true;


                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }



            }
        }
    }
}