# Chrome Developer Tool Tips

As we are working with bundles and analyzing the total number of requests and the total response size in the network tab of our chrome developer tools, we can find unreliable results. If this happens on your machine, the reason is that your browser has cached some of the requests, so you may switch to the release mode and notice that the bundle size is actually larger than before.

In chrome developer tools, in the network tab, we have a checkbox labeled **Disable Cache**. This is useful as you are developing and debugging your applications.
