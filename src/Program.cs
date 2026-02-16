
using OCR2PO.Converters;
using Yarhl.Media.Text;
using Yarhl.FileSystem;
using Yarhl.IO;

namespace OCR2PO
{
    public class Program
    {
        static int Main(string[] args)
        {

//TODO:  outputPath
            if (args.Length < 1) {
                Console.WriteLine("Wrong number of arguments. Usage: OCR2PO dirPath");
                return 1;
        }

            string inputPath = args[0];

            OCR2Po(inputPath);
            return 0;
        }

        static void OCR2Po(string inputPath)
        {
            var texts = new Node("ncf", new NodeContainerFormat());
            var inputFiles = NodeFactory.FromDirectory(inputPath, "*.png");

            foreach (Node file in inputFiles.Children.ToArray())
            {
                Console.WriteLine($"Processing {file.Name}:");

                file.TransformWith<Binary2Jpeg>()
                    .TransformWith<Jpeg2Text>();

                Console.WriteLine($"Extracted text from {file.Name}.");

                texts.Add(file);
                Console.WriteLine("");
            }

            BinaryFormat binaryPo = texts.TransformWith<Text2Po>()
                .TransformWith<Po2Binary>()
                .GetFormatAs<BinaryFormat>()!;
            
            binaryPo!.Stream.WriteTo("OCR.po");
        }
    }
}
