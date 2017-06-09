# Database-First Vs Code-First

### Database-First

The traditional way to design a database is to create the tables first and then have Entity Framework generate corresponding domain classes - this approach is called **Database-First** or DB-First

Database -> Entity Framework -> Domain Classes

-

### Code-First

In the **Code-First** workflow approach, we switch the order of events around by starting with the Domain Classes and have Entity Framework generate the database tables for us:

Domain Classes -> Entity Framework -> Database

***

### Which Is Better?

##### The reasons you could say Code-First is better is because:

- It increases our productivity because we don't need to waste our time with table designers, it is much faster to write code

    - Also, when we create the tables using a designer we need to manually create a change script in order to deploy the database - and we can have that done for us if we use the Code-First workflow

- We get the full versioning of our database and we can migrate to any version at any point in time

- Much easier to build integration test database

##### A Myth About Code-First

Myth: Some people claim that if you have an application with an existing database then Code-First doesn't work

Truth: The benefits of using Code-First on existing databases is that you get the full version in your database from the moment you switch to code-first, plus you won't need to waste time creating tables and migration scripts
