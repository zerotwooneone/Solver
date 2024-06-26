// See https://aka.ms/new-console-template for more information

using Solver.Domain.Board;
using Solver.Domain.Cell;

Console.WriteLine("Hello, World!");

var builder = new BoardBuilder();
var b = builder.CreateFrom9x9Csv("input.txt");

Console.WriteLine();
Console.WriteLine($"isValid: {b.IsValid} isSolved:{b.IsSolved}");