# Solver
A place to practice puzzle solving

Yet another sudoku solver.
This project currently supports reading puzzles in via CSV file where each line is a row in the puzzle.

## Domain Driven Design
This project attempts to demonstrate reasonable separation of code. The code required to solve any sudoku puzzle can be found in Solver.Domain. There should be no code related to soling puzzles in Solver.Console as this is just the app to run the solver.
The goal is for the domain library to depend upon the smallest number of external libraries possible, in hopes of remaining pure business logic.

### Example Models
- ICell represent a cell on the board. May (or may not) have a solved value. May (or may not) know which values remain as valid choices for this cell.
- IRow represents the 9 cells that make up a row
- IColumn represents the 9 cells that make up a column
- IRegion represents the 9 cells that make up a region (or grid)
- GameBoard represents an immutable state of the game. Any method to mutate the board produces a new immutable board with the new state. A game board should never be invalid.

## Unit Testing
This project avoids the common practice of mocking every different dependency a class uses, and instead favors only mocking things that would be varient during testing (such as file systems). This is convenient because there are very little dependencies on the "outside world".
This means that generally speaking, we get almost 100% code coverage with a smaller number of tests, and spend less time re-writing tests when interfaces change.