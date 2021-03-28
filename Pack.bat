@echo off
echo BUILD Cadmus Pura Packages
del .\Cadmus.Pura.Parts\bin\Debug\*.nupkg
del .\Cadmus.Pura.Parts\bin\Debug\*.snupkg
del .\Cadmus.Pura.Services\bin\Debug\*.nupkg
del .\Cadmus.Pura.Services\bin\Debug\*.snupkg
del .\Cadmus.Seed.Pura.Parts\bin\Debug\*.nupkg
del .\Cadmus.Seed.Pura.Parts\bin\Debug\*.snupkg

cd .\Cadmus.Pura.Parts
dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
cd..

cd .\Cadmus.Pura.Services
dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
cd..

cd .\Cadmus.Seed.Pura.Parts
dotnet pack -c Debug -p:IncludeSymbols=true -p:SymbolPackageFormat=snupkg
cd..

pause
