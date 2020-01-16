# PetGroomingMVC

## Updating an existing repo steps
- run git stash to avoid conflicts between remote and local repository
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
