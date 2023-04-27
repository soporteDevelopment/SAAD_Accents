using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADSystem.Helpers
{
    public static class LinqExtensions
    {
        public static IEnumerable<List<T>> Batch<T>(this IEnumerable<T> source, int batchSize)
        {
            var batch = new List<T>();
            foreach (T item in source)
            {
                batch.Add(item);
                if (batch.Count == batchSize)
                {
                    yield return batch;
                    batch = new List<T>();
                }
            }
            if (batch.Count > 0)
                yield return batch;
        }
    }

    /// <summary>
    /// Helper Class ITextSharp
    /// </summary>
    public class HelperTextSharp
    {
        public readonly iTextSharp.text.Font _largeFont = new iTextSharp.text.Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        public readonly iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        public readonly iTextSharp.text.Font _smallFont = new iTextSharp.text.Font(Font.FontFamily.HELVETICA, 4, Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
        public readonly iTextSharp.text.Font fontCell = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.NORMAL);
        public readonly iTextSharp.text.Font fontRedCell = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, new BaseColor(System.Drawing.Color.Red));
        public readonly iTextSharp.text.Font fontNormalHeader = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
        public readonly iTextSharp.text.Font fontHeader = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
        public readonly iTextSharp.text.Font fontFooter = new Font(Font.FontFamily.TIMES_ROMAN, 9, Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
        public readonly iTextSharp.text.Font fontsmall = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
        public readonly iTextSharp.text.Font fontsmallwhite = new Font(Font.FontFamily.TIMES_ROMAN, 7, Font.NORMAL, new BaseColor(System.Drawing.Color.White));
        public readonly iTextSharp.text.Font fontlarge = new Font(Font.FontFamily.TIMES_ROMAN, 50, Font.NORMAL, new BaseColor(System.Drawing.Color.White));

        /// <summary>
        /// Set margins and page size for the document
        /// </summary>
        /// <param name="doc"></param>
        public void SetStandardPageSize(iTextSharp.text.Document doc)
        {
            // Set margins and page size for the document
            doc.SetMargins(5, 5, 5, 5);
            // There are a huge number of possible page sizes, including such sizes as
            // EXECUTIVE, POSTCARD, LEDGER, LEGAL, LETTER_LANDSCAPE, and NOTE
            doc.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.LETTER.Width,
                iTextSharp.text.PageSize.LETTER.Height));
        }

        /// <summary>
        /// Add a paragraph object containing the specified element to the PDF document.
        /// </summary>
        /// <param name="doc">Document to which to add the paragraph.</param>
        /// <param name="alignment">Alignment of the paragraph.</param>
        /// <param name="font">Font to assign to the paragraph.</param>
        /// <param name="content">Object that is the content of the paragraph.</param>
        public void AddParagraph(Document doc, int alignment, iTextSharp.text.Font font, iTextSharp.text.IElement content)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.SetLeading(0f, 1.2f);
            paragraph.Alignment = alignment;
            paragraph.Font = font;
            paragraph.Add(content);
            doc.Add(paragraph);
        }
    }
}