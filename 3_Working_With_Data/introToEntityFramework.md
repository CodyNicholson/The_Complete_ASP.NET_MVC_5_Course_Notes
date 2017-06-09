# Intro To Entity Framework

**Entity Framework** is a tool we use to access a database, more accurately it is classified as an object relational mapper (or ORM) which means it maps data in a relational database into objects of our application

Entity framework provides a class called DbContext which is a gateway to our database DbContext has one of more DbSets which represent tables in our database

We use LINQ to query these DbSets and entity framework will translate our LINQ queries into SQL queries at runtime. It opens a connection to the database, reads the data, maps them to objects, and adds them to DbSets in our DbContext.

As we add, modify, or remove objects in these DbSets entity framework keeps track of of these changes. Then when we ask it to persist the changes again it will automatically generate SQL queries and execute them on our database.

There are two workflows for using entity framework: **Code-First** and **Database-First**
