# OAuth

In this section we will learn how to enable users to login using their Facebook account

Facebook and other external authentication providers like Google, Twitter, Microsoft, and so on use an authentication protocol called **OAuth** which is short for *Open Authorization*.

Let's say John is a new staff member that wants to login to Vidly using his Facebook account. First, we need to register our application with Facebook to create some kind of partnership. Facebook will give us an API key and a secret - like a username and a password. We'll use this to talk to Facebook in the background. John wants to login when he clicks on "Facebook Login" on our website. When this happens, we will redirect him to Facebook and we'll use our API key and secret so that Facebook knows that this request is coming from Vidly. To prevent malicious users from finding our secret, we use HTTPS so the data exchange between these parties will be encrypted and no one can intercept this communication. So he logs into Facebook. Once this happens, Facebook tells him that Vidly wants access to some basic information about his account. Facebook will know the address of Vidly at this time because we registered Facebook with Vidly. Facebook will pass back an authentication token which tells Vidly that his has logged in. On Vidly we get this authorization token and send it back to Facebook with our API key and secret. We do this because a hacker may send a random authorization token to Vidly, so we need to verify that it came from Facebook. This is Vidly asking Facebook if it really sent the request, and Facebook will say yes by giving us an access token. With this access token we can access some parts of the user's profile. All of this is done by ASP.NET Identity.

There are two steps to enable social logins:

- Enable SSL for secure communication
- Register our app with Facebook
