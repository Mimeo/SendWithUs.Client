SendWithUs.Client
=================

A .NET client implementation over the SendWithUs REST API.

Currently, the client only covers a small portion of the API surface--namely, sending email and batch operations.

## Installation via NuGet

SendWithUs.Client is [available from nuget.org](https://www.nuget.org/packages/SendWithUs.Client/). If you are using 
Visual Studio, install the package using the Package Manager Console as follows: 

```
PM> Install-Package SendWithUs.Client
```

## Usage

The following  description of the  API should be enough to get you started. See the comments in the code (available as
Intellisense documentation in Visual Studio) for more information.

### Sending Email

To send email, you must instantiate a `SendRequest` object to pass to `SendWithUsClient.SendAsync`. A `SendRequest` must 
have a `TemplateId` and a `RecipientAddress`. All other properties are optional.

#### Minimal Example

```csharp
using SendWithUs.Client;

var request = new SendRequest("template123", "foo@example.com");
var client = new SendWithUsClient("my-api-key");
var response = await client.SendAsync(request);
```

#### More Realistic Example

The data passed to SendRequest can be any CLR object, as long as it serializes as a JSON object.

```csharp
using SendWithUs.Client;

var sheriff = new Person { ... };
var wristslap = new Punishment { ... };
var data = new Dictionary<string, object> { { "who", sheriff }, { "what", wristslap } };
var request = new SendRequest
{
    TemplateId = "disciplinary-form",
    SenderName = "Boss Hogg",
    SenderAddress = "j.d.hogg@hazzard.example.com",
    RecipientAddress = "rosco@hazzard.example.com",
    Data = data
};
var client = new SendWithUsClient("my-api-key");
var response = await client.SendAsync(request);
```

### Batching

To make a batch request, pass a collection of request objects to `SendWithUsClient.BatchAsync`. (Note that the only type
of request object currently supported is `SendRequest`.)

#### Example

```csharp
using SendWithUs.Client;

var request1 = new SendRequest("template123", "foo@example.com");
var request2 = new SendRequest("template567", "bar@example.com");
var client = new SendWithUsClient("my-api-key");
var response = await client.BatchAsync(new List<IRequest> { request1, request2 });
```
