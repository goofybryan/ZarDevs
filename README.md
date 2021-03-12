# ZarDevs

## Introduction

I have been working in the .Net environment for years, and as everyone knows, you generate a style of how you code. So when it comes to new projects, I always tend to have the same thought patterns. So I decided to take some of the ideas I have been toying with and make them available, without time constraints.

## What is there?

So I decided to take one of last projects and completely revamp the backbone libraries I wrote. I decided that I was going to try things differently and all things that annoyed me about it and build something that is useful and flexible and re-usable.

So here is a list of items I am busy with. I also need to split the project into smaller projects instead of having one monolith one.

### Dependency Injection (Ready, needs lots of testing)

One of my problems I had was that I developed this backbone using Ninject but as soon as I ran the code on a Xamarin Forms iOS phone, it crashed due to iOS not allowing certain types dynamic code, something along that lines. Now I found a workaround but the issue is that I wrote a lot of code, and to undo this, would take a lot of work.

So I decided that I didn't want to be tied down again. So I developed a standard way for IOC binding and calling, now I can write my libraries and just choose my implementing IOC technology.

[Link to the repo](./Dependency)

### Http Api Security Abstractions (In Progress)

With one of my projects, I wrote a way to handle API calls that included refreshing tokens and logging in if required without having to compromise simplicity. It was this architecture that my friend said made it easy to implement what he needed to do. There was no complex requirements he needed to do, just implement and run.

[Link to repo](./Http)

### Model and PropertyChanged Abstractions (To be started)

During my time I was doing some R&D development and what troubled me was the way that models and the changes that occur to them are handled. This came from how I normally saw them implemented. There is normally a base class with a dictionary that maps the property and changes and some complex code that manages this. What if you could take away that need on a model class and just let something else worry about that and let a model just say it's changed something.

[Link to repo](./Models)

### Runtime Utilities

This isn't really something I dreamed of doing, this is just a generic project that houses the shared runtime logic I need to make some of the magic happen.

[Link to repo](./Runtime)

## Links

1. [Home](./README.md)
1. [Dependency Injection](./Dependency/README.md)
1. [Http Abstractions](./Http/README.md)
1. [Model Abstractions](./Models/README.md)
1. [Runtime](./Runtime/README.md)
