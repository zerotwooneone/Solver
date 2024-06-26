using Solver.Domain.Board;

Console.WriteLine("Hello, World!");

var builder = new BoardBuilder();
var b = builder.CreateFrom9x9Csv("input.txt");

Console.WriteLine();
Console.WriteLine($"isValid: {b.IsValid} isSolved:{b.IsSolved}");