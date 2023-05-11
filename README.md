## Backend - CDN Dev Technical Test Solution
#### About this solution:
- This solution was developed by Werner Moecke for Agile Content.
- The binary and required `.config` files can be found in the `..\bin\Release\net6.0\win-x64\publish` folder. Both files need to be placed together in order for the application to run successfully.
- Should you want to generate a self-contained executable yourself, the solution must be *published* - To achieve this, you can either:
    - Go to *Build* -> *Publish Selection* after selecting the main project under *Solution Explorer* and first create a profile to the *Folder* target. After that, you can *Publish*.
    - Use the CLI: `dotnet publish -c Release`.
- The solution is comprised of 2 projects:
    - **CandidateTesting.WernerMoecke:** This is the main project.
    - **CandidateTesting.WernerMoecke.UnitTests:** Contains the unit tests for the main project.

#### Implementation details:
- This solution targets the *.NET 6.0* framework.
- It was decided to use the [`log4net`](https://logging.apache.org/log4net/) library (by Apache Foundation) for the purposes of writing output log statements to both console and file targets.
    - `log4net` offers distinct features such as dynamic configuration and ease of use.
    - `log4net` was chosen because it takes away from the main code the responsibility of formatting the output according to the specifications. Should something change (basic layout changes), the required changes are easy and quick to perform.
