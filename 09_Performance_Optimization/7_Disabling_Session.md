# Disabling Session

The last optimization technique we will talk about in the Applications Tier is disabling the session. We haven't covered the session in this course because it should be avoided. A **Session** is a piece of memory in the web server allocated to each user. We can use it to store temporary data during the user session.

The more users you have, the more memory of the web server your application is going to use. **This kills scalability of your application**. The days of using session are gone because we can use cloud based solutions and it's more likely that the traffic to a web application increases exponentially. To make our web applications scalable we should make them stateless - which means our request comes in, gets processed, and we are done. We don't maintain state. We don't use Session.

To disable Session - in solution explorer we can open **Web.config**. In the system.web tag we can add the sessionState tag and set the mode to "Off":

```
  <system.web>
    <sessionState mode="Off"></sessionState>
    <authentication mode="None" />
    <compilation debug="true" targetFramework="4.5.2" />
```

This is how we disable session state, which improves the scalability and the performance of our applications

It is safe to implement this application tier optimization without performance profiling because they don't add any complexity and it is simple
