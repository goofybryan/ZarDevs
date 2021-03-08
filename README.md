# ZarDevs, where code and whimsy meet

## Introduction

I have been working in the .Net environment for years, and I as everyone know, you generate a style of how you code. So when it comes to new projects, I always tend to have the same patterns. However, I was working with a friend of mine, developing the backbone and putting place what needs to be done and his comment stuck with me. He said that what did just made it easy to follow.

So here I am thinking, so why not share and maybe someone will think, hey that's not a bad idea! These are projects are just a place where I am playing with concepts and having some fun coding.

## What is there?

So I decided to take one of last projects and completely revamp the backbone libraries I wrote. I decided that I was going to try things differently and all things that annoyed me about it and build something that is useful and flexible with no time constraints.

So here is a list of items I am busy with.

### Dependency Injection (Release Ready)

One of my problems I had was that I developed this backbone using Ninject but as soon as I ran the code on a Xamarin Forms iOS phone, it crashed due to an exception about the JIT compiler. Now I found a workaround but the issue is that I wrote a lot of code, and to undo this, would take a lot of work.

So I decided that I didn't want to be tied down again. So I developed a standard way for IOC binding and calling, now I can write my libraries and just choose my implementing IOC technology.

I have some grandiose idea of even maybe adding method binding but then I decided that if there is a need, add it.

[Link to the repo](./Dependency/README.md)

### Http Api Security Abstractions (In Progress)

With one of my projects, I wrote a way to handle API calls that included refreshing tokens and logging in if required without having to compromise simplicity. It was this architecture that my friend said made it easy to implement what he needed to do. There was no complex requirements he needed to do.

[Link to repo](./Http/README.md)

### Model and PropertyChanged Abstractions (To be started)

During my time I was doing some R&D development and what troubled me was the way that models and the changes that occur to them are handled. This came from how I normally saw them implemented. There is normally a base class with a dictionary that maps the property and changes and some complex code that manages this. What if you could take away that need on a model class and just let something else worry about that.

[Link to repo](./Models/README.md)

### Runtime Utilities

This isn't really something I dreamed of doing, this is just a generic project that houses the shared runtime logic I need.

[Link to repo](./Runtime/README.MD)
