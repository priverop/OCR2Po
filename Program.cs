using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing; 
using Tesseract;
using Yarhl.FileSystem;
using Yarhl.IO;

namespace OCR2PO
{
    internal class Program
    {
        static readonly string inputPath = "input";
        static string extractedTextImageSharp = string.Empty;

        static void Main()
        {
            Node inputFiles = NodeFactory.FromDirectory(inputPath);
            foreach (Node file in inputFiles.Children)
            {
                Console.WriteLine(file.Name);
                GetText(file.Stream);
                Console.WriteLine("Without Any Modification of Image");
                Console.WriteLine(extractedTextImageSharp);

                GetText(ProcessImage(file.Stream));
                Console.WriteLine("With Modification of Image");
                Console.WriteLine(extractedTextImageSharp);
            }
            
        }

        static void GetText(DataStream stream)
        {  
            stream.Position = 0;
            var reader = new DataReader(stream);
            byte[] buffer = reader.ReadBytes((int)stream.Length);

            using (var engine = new TesseractEngine(@"./tessdata", "jpn", EngineMode.LstmOnly))
            {
                using var img = Pix.LoadFromMemory(buffer);
                using var page = engine.Process(img);
                extractedTextImageSharp = page.GetText();
            } 
        }

        static DataStream ProcessImage(DataStream stream)
        {
            stream.Position = 0;
            var processedImage = new DataStream();
            using (Image<Rgba32> image = Image.Load<Rgba32>(stream))
            {
                // Invertir colores para que el texto sea oscuro sobre fondo claro
                image.Mutate(x => x.Invert());
                
                // Escalar si es necesario
                image.Mutate(x => x.Resize(image.Width * 20, image.Height * 20));
                
                image.SaveAsJpeg(processedImage);
            } 

            return processedImage;
        }

        // static void ImagSharpMethod(string path)
        // {
        //     using (Image<Rgba32> image = Image.Load<Rgba32>(path))
        //     {
        //         image.Mutate(x => x.Resize(image.Width * times, image.Height * times));
        //         // Iterate through each pixel
        //         for (int y = 0; y < image.Height; y++)
        //         {
        //             for (int x = 0; x < image.Width; x++)
        //             {
        //                 // Get the pixel color
        //                 Rgba32 pixelColor = image[x, y];

        //                 // Check if the pixel color matches the target colors
        //                 if (IsTargetColor(pixelColor))
        //                 {
        //                     // Set to absolute white
        //                     image[x, y] = Rgba32.ParseHex("#fff");
        //                 }
        //             }
        //         }
                 
        //         // Save the modified image
        //         image.Save(modifiedImagePath);
        //     } 
        // }

        // static bool IsTargetColor(Rgba32 color)
        // { 

        //     // Blue and light blue range
        //     if (color.B > 128 && color.R < 128 && color.G < 128)
        //         return true;

        //     // Grey range
        //     if (color.R > 128 && color.R < 200 && color.G > 128 && color.G < 200 && color.B > 128 && color.B < 200) return true;

        //     // Yellow range
        //     if (color.R > 200 && color.G > 200 && color.B < 128)
        //         return true;

        //     // Add more color checks as needed

        //     return false;
        // }
    }
}
