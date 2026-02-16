
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
            if (args.Length < 2) {
                Console.WriteLine("Wrong number of arguments. Usage: OCR2PO <dirPath> <outputPath>");
                return 1;
        }

            string inputPath = args[0];
            string outputPath = args[1];

            OCR2Po(inputPath, outputPath);
            return 0;
        }

        static void OCR2Po(string inputPath, string outputPath)
        {
            using var inputFiles = NodeFactory.FromDirectory(inputPath, "*.png", FileOpenMode.Read);

            // Transform each node to text
            inputFiles.Children
                .TransformCollectionWith(new Binary2Jpeg())
                .TransformCollectionWith(new Jpeg2Text());

            // Group the collection into a single PO and write to disk
            inputFiles.TransformWith(new Text2Po())
                .TransformWith(new Po2Binary())
                .Stream!.WriteTo(Path.Combine(outputPath, "OCR.po"));

            Console.WriteLine("Done!");
        }
    }
}
