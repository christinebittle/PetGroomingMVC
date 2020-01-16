# PetGroomingMVC

## Updating an existing repo steps
- Delete the Contents of the Migrations Folder (If you modified the DB)
- Run an SQL command (going to sql server > mssqllocaldb > pet grooming context (right click) > run query)
- run query "truncate table pets;"
- Use git cmd to run git pull origin master
- Tools > Nuget Package Manager > Package Manager Console
- run command "update-database"

## Cloning a new repo steps
- use git cmd to clone repository
- create "App_Data" Folder
- right click project "Build" -> 1 successful
- run "Enable-Migrations" in package manager console
- run "Add-Migration initial" in package manager console
- run "Update-Database"
- navigate to View/Pet/List.cshtml
- run List.cshtml and click yes to SSL self signed prompts
- > should just show list of pets with no data

- insert data into pets table (see screenshots)

- run List.cshtml
