# Introduction

This project contains a testable abstraction of the http client. I liked what Microsoft did with [HttpClient](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0) and [message handlers](https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpmessagehandler?view=net-5.0). Initially I used these classes to achieve my goal of manipulating the auth header but later I decided to create my own abstraction for unit testability and control.

## What still needs to be done

1. Security projects need to be added and updated
1. Tests
1. Example projects that can be run showing
    1. Normal Xamarin App connecting to Server
    1. Different log in types (username/password, google SSO, certifcate)
