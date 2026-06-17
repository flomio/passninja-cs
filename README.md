# passninja-cs

Use [passninja-cs](https://passninja.com/docs) as a .NET module.

[![Status](https://img.shields.io/badge/status-active-success.svg)](https://github.com/flomio/passninja-cs)
[![Issues](https://img.shields.io/github/issues/flomio/passninja-cs.svg)](https://github.com/flomio/passninja-cs/issues)
[![NuGet](https://img.shields.io/nuget/v/passninja.svg)](https://www.nuget.org/packages/passninja)

# Contents

- [Contents](#contents)
- [Installation](#installation)
- [Usage](#usage)
  - [`PassNinjaClient`](#passninjaclient)
  - [`PassNinjaClient Methods`](#passninjaclientmethods)
  - [Script Tag](#script-tag)
  - [Examples](#examples)
- [TypeScript support](#typescript-support)

# Installation

Install via the .NET CLI:

```sh
dotnet add package passninja
```

Or via the Package Manager Console:

```sh
Install-Package passninja
```

# Usage

## `PassNinjaClient`

This function returns a newly created PassNinjaClient object. Make sure to pass your user credentials to make any authenticated requests. 

```C#
using passninja;

private const string AccountId = "90f0629f-2960-4c20-ac8d-d6e6e389ac73";
private const string ApiKey = "PEMsFSPv2H3O9e8iPW4oO96mqpSChzek7omQGjdY";

PassNinjaClient passNinjaClient = new PassNinjaClient(AccountId, ApiKey);
```

We've placed our demo user API credentials in this example. Replace it with your
[actual API credentials](https://passninja.com/auth/profile) to test this code
through your PassNinja account and don't hesitate to contact
[PassNinja](https://passninja.com) with our built in chat system if you'd like
to subscribe and create your own custom pass type(s).

For more information on how to use `passninja-cs` once it loads, please refer to
the [PassNinja JS API reference](https://passninja.com/docs/js)

## `PassNinjaClientMethods`

This library currently supports methods for creating, getting, updating, and deleting passes via the PassNinja api. The methods are outlined below. Note that each method returns a promise. 

### Create

```C#
  var createResponse = passNinjaClient.CreatePass("demo.coupon", new PassData()
  {
    discount = "50%",
    memberName = "Test User1"
  });

  Console.WriteLine("Create Pass response");
  Console.WriteLine(createResponse.url);
  Console.WriteLine(createResponse.passTemplate);
  Console.WriteLine(createResponse.serialNumber);
```

### Get

```C#
var getResponse = passNinjaClient.GetPass("demo.coupon", "ed669b28-9b87-485b-8df6-8b04158d53c1");

Console.WriteLine(getResponse.url);             
Console.WriteLine(getResponse.passTemplate);
Console.WriteLine(getResponse.serialNumber);
```

### Update

```C#
var putResponse = passNinjaClient.PutPass("demo.coupon", "ed669b28-9b87-485b-8df6-8b04158d53c1", new PassData()
{
    discount = "50%",
    memberName = "Test User2"
});

```

### Delete

```C#
var deleteResponse = passNinjaClient.DeletePass("demo.coupon", "e56b4857-5bf6-4829-a6c3-951aee5d3a15");  

Console.WriteLine("Delete Pass response");

```