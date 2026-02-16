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
using Yarhl.FileFormat;
using Yarhl.FileSystem;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace OCR2PO.Converters
{
    /// <summary>
    /// Converts a container of ImageText into a Po format.
    /// </summary>
    public class Text2Po :
        IConverter<NodeContainerFormat, Po>
    {
        /// <summary>
        /// Converts the NCF into a Po.
        /// </summary>
        /// <param name="source">NCF node.</param>
        /// <returns>Po format.</returns>
        public Po Convert(NodeContainerFormat source)
        {
            ArgumentNullException.ThrowIfNull(source);

            var po = new Po(new PoHeader("Your Project Name", "contact_email", "language"));

            foreach (Node item in source.Root.Children)
            {
                ImageText imageText = item.GetFormatAs<ImageText>()!;

                if (imageText == null || string.IsNullOrWhiteSpace(imageText.Text))
                {
                    continue;
                }

                po.Add(new PoEntry(imageText.Text) {
                    Context = item.Name,
                });
            }

            return po;
        }
    }
}