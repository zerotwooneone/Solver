using Solver.Domain.Board;

Console.WriteLine("Hello, World!");

var builder = new BoardBuilder();
var b = builder.CreateFrom9x9Csv("input.txt");

Console.WriteLine($"Input board sValid:{b.GetIsValid()} isSolved:{b.GetIsSolved()}");

var solver = new BoardSolver();
var s = solver.GetSolvedBoard(b);

Console.WriteLine();
Console.WriteLine($"Solver isValid:{s.GetIsValid()} isSolved:{s.GetIsSolved()}");