# Deploying The Database

Unless you are using Web Deploy and reading code first migrations as part of your deployment you have to deploy your database manually. In package manager console you can run: **update-database -script**. Instead of updating our database, the script provided we will get a SQL script that includes all migrations in our application. This is useful when you want to deploy a database for the first time.

After that you don't want to run all these migrations again. In that case you can provide another switch: **update-database -script -SourceMigration:MigrationNameGoesHere**. These migrations we have include both our schema and data changes. If we want to add new reference data or modify or delete some existing reference data as a part of deploying a new version, they are all included in our migrations. If you don't use code-first you have to manage all these scripts manually. 

How do we know what migrations we have run on a database? In our database we have this table called **MigrationHistory**. This table contains the whole version history of your database. Testing, staging, and production.
