using Solver.Domain.Board;

Console.WriteLine("Hello, World!");

var builder = new BoardBuilder();
var b = builder.CreateFrom9x9Csv("input.txt");

Console.WriteLine($"Input board sValid:{b.IsValid} isSolved:{b.IsSolved}");

var solver = new BoardSolver();
var s = solver.GetSolvedBoard(b);

Console.WriteLine();
Console.WriteLine($"Solver isValid:{s.IsValid} isSolved:{s.IsSolved}");