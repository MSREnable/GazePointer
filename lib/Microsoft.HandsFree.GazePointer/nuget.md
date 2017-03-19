Package using the following commands (naturally, update the filename accordingly):

    nuget pack -symbols Microsoft.HandsFree.Mouse.csproj -IncludeReferencedProjects
    nuget push -Source https://www.nuget.org/api/v2/package Microsoft.HandsFree.Mouse.1.0.2.nupkg