﻿// See https://aka.ms/new-console-template for more information

using SolverConsole.Board;
using SolverConsole.Cell;

Console.WriteLine("Hello, World!");

var b = new GameBoard(
    new SolvedCell(1), new SolvedCell(1), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, new SolvedCell(2), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, UnsolvedCell.Instance, new SolvedCell(3), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, new SolvedCell(4), UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance, UnsolvedCell.Instance,
    new SolvedCell(2), new SolvedCell(1), new SolvedCell(3), new SolvedCell(4),new SolvedCell(5),new SolvedCell(6),new SolvedCell(7),new SolvedCell(8),new SolvedCell(9) 
    );

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