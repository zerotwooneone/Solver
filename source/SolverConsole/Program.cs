// See https://aka.ms/new-console-template for more information

using Solver.Domain.Board;
using Solver.Domain.Cell;

Console.WriteLine("Hello, World!");

var builder = new BoardBuilder();
var b = builder.CreateFrom9x9Csv("input.txt");

for (int rowIndex = 0; rowIndex < b.Rows.Length; rowIndex++)
{
    Console.WriteLine($"row {rowIndex} isValid/isSolved: {b.Rows[rowIndex].Check()}");
}
Console.WriteLine();
for (int columnIndex = 0; columnIndex < b.Columns.Length; columnIndex++)
{
    Console.WriteLine($"column {columnIndex} isValid/isSolved: {b.Columns[columnIndex].Check()}");
}
Console.WriteLine();
for (int regionIndex = 0; regionIndex < b.Regions.Length; regionIndex++)
{
    Console.WriteLine($"region {regionIndex} isValid/isSolved: {b.Regions[regionIndex].Check()}");
}
Console.WriteLine();
Console.WriteLine($"isValid: {b.IsValid} isSolved:{b.IsSolved}");