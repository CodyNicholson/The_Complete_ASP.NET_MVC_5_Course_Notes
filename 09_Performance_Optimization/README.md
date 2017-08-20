# Performance Optimization

In this section we go over three-tiered architecture and techniques that you can apply in each tier to improve the performance of applications

***

### "Premature optimization is the root of all evils" - Donald Knuth

Making code ugly and unmaintainable in an effort to improve performance should not be done. Never sacrifice maintainability for performance optimization. **Profile first, then optimize**. *Performance profiling* is the process of analyzing the performance of your application to find ways to make it faster. Optimize only when you need to. Otherwise, we increase the development and maintenance costs without gaining anything.

### How do we optimize?

Performance optimization is a very broad area, and there is no magic fix for it. Performance bottlenecks are like diseases. There is no medicine that can cure all diseases, only specialized treatments to to the best to fix each disease. There are however a few key areas that most commonly cause bottlenecks.

### Three Tier Architecture

Most web applications follow a three-tier architecture. A tier is where your code runs. We have the **Data Tier** where our database and queries reside. We have the **Application Tier** where we have a web server hosting our application. Lastly, we have the **Clients Tier** here which is the client's computer that is the frontend of our application running in the browser.

Generally speaking, most of the performance bottlenecks are in the **Data Tier**, so optimizing performance at this level will have the most gain. The **Application Tier** is the next place where we should look for bottlenecks, but it is not as high-priority as the **Data Tier**. Lastly, we should look in the **Client Tier** where the fewest number of bottlenecks are typically found.

***

Sometimes developers spend lots of time optimizing code in the **Application Tier** or **Client Tier** to change a 50ms wait time into a 20ms wait time. This is great, a 250% gain, but it is not observable to the user since it is such a short time. That is why our focus should be on the **Data Tier** where we can make more observable changes, like turning a 5 second wait time into a 0.5 second wait time. Work on observable changes.

***

### Mosh's Optimization Rules

- Do not sacrifice the maintainability of your code to premature optimization
- Be realistic and think like an engineer, not a computer
- Be pragmatic and ensure your efforts have observable results and give value
