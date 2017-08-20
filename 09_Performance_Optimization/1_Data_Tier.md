# Data Tier

Performance problems in this tier are often because of database schema and or queries. In terms of the database schema, make sure that every table in your database has a primary key and that there a relationships between tables. We should also use indexes on columns that you use for filtering records in your queries.

In terms of our application, since we are using Entity Framework Code-first, our migrations automatically add the primary key and indexes in our tables. So we don't have to work about it much for our application.

#### First priority is to fix your database schema

Another schema-related issue is the implementation of a pattern called **EAV** which is an acronym for **Entities, Attributes, and Values**. Instead of having concrete tables our database has only a handful of tables called entities, attributes, values, and potentially a few other supporting tables. People who like this kind of schema argue that with this, no matter how many classes we have in the applications domain model we're going to have to change the database schema because everything can be modeled in generic way using Entity's Attributes and Values. While this argument is technically correct, there is a huge cost to this approach. Firstly - we cannot use any object relational mappers like Entity Framework and you have to write all your queries by hand. These queries can easily grow to several hundred or even thousands of lines. On top of this you will have huge performance problems.

#### Second priority is to write queries

In Vidly, we have let Entity Framework do all the queries for us. It is a good idea to pay close attention to these generated queries to avoid problems. Sometimes if it is too complex it is better to create a stored procedure and write an optimized query to get the same result. 

In SQL Server we have a feature called **Execution Plan** which shows us how SQL Server executes our query. By using this execution plan we can see what parts of your query have the biggest cost and then we can optimize the query by rewriting it or we may want to add additional indexes in our database.

Once we do all we can to optimize our queries, if it is still slow then we should consider creating a separate database for our queries. There are architectural patterns like CQRM or Command Query Responsibility Segregation that guide us in that direction. Since we read data more often then we modify, we can create a separate database optimized for reading data. In that database we pre-join some of our tables to speed up the queries because joining tables is one of the areas that impacts the performance of a query. This approach comes with the cost of maintaining two databases in sync.

Another approach is to use caching. With caching we run a slow query and then store the resolves in the memory. The subsequent requests will be served from this cache in the memory.
