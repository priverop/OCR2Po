// Copyright (c) 2026 Priverop
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using Tesseract;
using Yarhl.FileFormat;
using Yarhl.IO;

namespace OCR2PO.Converters
{
    /// <summary>
    /// Converts between an ImageSharp Jpeg (BinaryFormat), and a string.
    /// </summary>
    /// <remarks>Uses Tesseract OCR to read the images.</remarks>
    public class Jpeg2Text :
        IConverter<Jpeg, ImageText>
    {
        /// <summary>
        /// Converts the Jpeg to a string.
        /// </summary>
        /// <param name="source">Jpeg format.</param>
        /// <returns>String with the text.</returns>
        public ImageText Convert(Jpeg source)
        {
            ArgumentNullException.ThrowIfNull(source);

            source.Stream.Position = 0;
            var reader = new DataReader(source.Stream);
            var text = new ImageText();
            byte[] buffer = reader.ReadBytes((int)source.Stream.Length);

            using (var engine = new TesseractEngine(@"./tessdata", "jpn", EngineMode.LstmOnly))
            {
                using var img = Pix.LoadFromMemory(buffer);
                using var page = engine.Process(img);
                text.Text = page.GetText();
                Console.WriteLine(text.Text.TrimEnd());
            }

            return text;
        }
    }
}