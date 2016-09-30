
Karina Biancone, September 25, 2016


I updated PS2 on the 29th of September to be 'SpreadsheetUtilites' instead of 'DependencyGraph'
I am using the PS3 files that were last updated the 22nd of September.

My initial steps for the Spreadsheet project was making a Cell class that will hold the content of a string,
double, or Formula and be able to return that content as an object. I then need a Spreadsheet constructor that
will initiate my Dictionary(cellGraph) and DependencyGraph(cellDependents).

P.S.
 I do not use 'GetCellsToRecalculate' for my 'SetContent' for a string and double, only formula. This is mainly because I do not need
 to check for a circular dependency when creating those cells, since it is not possible and seemed like extra things to check for no reason.
 I simply made my own helper method that returns all direct and indirect dependents without checking for repeating cell names.
Also, I tried to update the folder that holds PS3 to say 'PS3' but it still says 'ClassLibrary1'. I'm hoping that because
I already was marked down for this I won't be again.