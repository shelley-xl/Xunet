﻿using System.Globalization;

namespace Xunet.MiniExcels.OpenXml.Constants
{
    internal class WorksheetXml
    {
        internal const string StartWorksheet = @"<?xml version=""1.0"" encoding=""utf-8""?><x:worksheet xmlns:x=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">";
        internal const string StartWorksheetWithRelationship = @"<?xml version=""1.0"" encoding=""utf-8""?><x:worksheet xmlns:r=""http://schemas.openxmlformats.org/officeDocument/2006/relationships"" xmlns:x=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"" >";
        internal const string EndWorksheet = "</x:worksheet>";

        internal const string StartDimension = @"<x:dimension ref=""";
        internal const string DimensionPlaceholder = "                              />";
        internal static string Dimension(string dimensionRef)
            => $"{StartDimension}{dimensionRef}\"/>";

        internal const string StartSheetData = "<x:sheetData>";
        internal const string EndSheetData = "</x:sheetData>";

        internal static string StartRow(int rowIndex)
            => $"<x:row r=\"{rowIndex}\">";
        internal const string EndRow = "</x:row>";

        internal const string StartCols = "<x:cols>";
        internal static string Column(int? colIndex, double? columnWidth)
            => $@"<x:col min=""{colIndex.GetValueOrDefault() + 1}"" max=""{colIndex.GetValueOrDefault() + 1}"" width=""{columnWidth?.ToString(CultureInfo.InvariantCulture)}"" customWidth=""1"" />";
        internal const string EndCols = "</x:cols>";

        internal static string EmptyCell(string cellReference, string styleIndex)
            => $"<x:c r=\"{cellReference}\" s=\"{styleIndex}\"></x:c>";
        //t check avoid format error ![image](https://user-images.githubusercontent.com/12729184/118770190-9eee3480-b8b3-11eb-9f5a-87a439f5e320.png)
        internal static string Cell(string cellReference, string cellType, string styleIndex, string cellValue, bool preserveSpace = false)
            => $"<x:c r=\"{cellReference}\"{(cellType == null ? string.Empty : $" t=\"{cellType}\"")} s=\"{styleIndex}\"{(preserveSpace ? " xml:space=\"preserve\"" : string.Empty)}><x:v>{cellValue}</x:v></x:c>";

        internal static string Autofilter(string dimensionRef)
            => $"<x:autoFilter ref=\"{dimensionRef}\" />";

        internal static string Drawing(int sheetIndex)
            => $"<x:drawing r:id=\"drawing{sheetIndex}\" />";

    }
}
