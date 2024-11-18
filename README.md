# AlgorithmsDataStructures-Assignment2
To run the projects:
1. navigate to the main directory that contains the "ADS-A2.sln" file.
2. right click and select "Open in terminal" or Powershell for windows.
3. To run the first project: Task1-SortingBenchmarks (algorithm race) type into terminal:
   
dotnet run --project Task1-SortingBenchmarks/

4. To run the second project: Task2-Library (library database system) type into terminal:

dotnet run --project Task2-Library/

alternatively, just change the active directory to either project then type dotnet run.

NOTE: The input file for Task1-SortingBenchmarks has been placed in all possible root folders depending on
the entry point or executable location. If it is not found, it can be specified at runtime when prompted,
or changed in InputFile.cs in the BaseDirectory property (currently searching the root folder, or ".")
change to the absolute path if there are issues.
