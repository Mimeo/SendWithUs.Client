SendWithUs.Client
=================

A .NET client implementation over the SendWithUs REST API.

Currently, the client covers the following parts of the API:

  * [Operations on Templates](https://www.sendwithus.com/docs/api#templates)
  * [Sending Email](https://www.sendwithus.com/docs/api#sending-emails)
  * [Rendering Templates](https://www.sendwithus.com/docs/api#rendering-templates)
  * [Batching Requests](https://www.sendwithus.com/docs/api#batch-api-requests)

## Installation via NuGet

SendWithUs.Client is [available from nuget.org](https://www.nuget.org/packages/SendWithUs.Client/). If you are using 
Visual Studio, install the package using the Package Manager Console as follows: 

```
PM> Install-Package SendWithUs.Client
```

## Design Notes

The goal of this library is to provide a thin, strongly-typed abstraction over the raw REST API. 
Central to this approach is the `SendWithUsClient` object, which exposes a set of methods that uniformly take a request
object and return a response object.

A malformed request will result in a `ValidationException` being thrown.

Errors from the REST API are not turned in to exceptions by the library. Rather, each response object has several properties
whose values indicate the outcome of the request: `StatusCode`, `IsSuccessStatusCode`, `ErrorMessage`.

## Usage

The following description of the API should be enough to get you started. See the comments in the code (available as
Intellisense documentation in Visual Studio) for more information.

### Sending Email

To send email, you must instantiate a `SendRequest` object, which must have values for `TemplateId` and `RecipientAddress`. 
All other properties are optional.

#### Minimal Example

```csharp
using SendWithUs.Client;

var request = new SendRequest { TemplateId = "template123", RecipientAddress = "foo@example.com" };
var client = new SendWithUsClient("my-api-key");
var response = await client.ExecuteAsync(request);
```

#### A More Realistic Example

The value of `SendRequest.Data` can be any CLR object, as long as it serializes as a JSON object.

```csharp
using SendWithUs.Client;

var sheriff = new Person { Name = "Rosco P. Coltrane" };
var wristslap = new Punishment { Severity = 1 };
var data = new DisciplinaryData { Who = sheriff, What = wristslap };
var request = new SendRequest
{
    TemplateId = "disciplinary-form",
    SenderName = "Boss Hogg",
    SenderAddress = "j.d.hogg@hazzard.example.com",
    RecipientAddress = "rosco@hazzard.example.com",
    Data = data
};
var client = new SendWithUsClient("my-api-key");
var response = await client.ExecuteAsync(request);
```

#### Typed Request Data

*Please note that strongly-typed request data has been deprecated and will be removed in the next version of this 
library.*

The type of the `Data` property on `SendRequest` is plain old object. If you want a strongly-typed `Data` property, use
`SendRequest<TData>` in lieu of `SendRequest`. This is merely a developer convenience. `SendRequest<TData>` shadows the 
`Data` property of `SendRequest` and casts it to the specified type.

### Rendering Templates

To render a template, you must instantiate a `RenderRequest` object, which must have a value for `TemplateId`. All other
properties are optional, although it's probably not very useful to omit the `Data` property.

The value of `RenderRequest.Data` can be any CLR object, as long as it serializes as a JSON object.

#### Example

```csharp
using SendWithUs.Client;

var generalLee = new Car { Make = "Dodge", Model = "Charger", Year = "1969" };
var request = new RenderRequest { TemplateId = "template987", Data = generalLee };
var client = new SendWithUsClient("my-api-key");
var response = await client.ExecuteAsync(request);
```

### Batching

To make a batch request, pass a collection of request objects to the appropriate overload of `SendWithUsClient.ExecuteAsync`.
The result will be an object that contains an ordered collection of individual response objects (one per request) on its `Items` 
property. The first response in `Items` will correspond to the first request, and so on.

Because your collection of requests can be heterogeneous, the collection of responses can also be heterogeneous. Therefore,
the specific type of each response is not known until run-time, and we can only represent the responses in `Items` as 
instances of the basic `IResponse` interface. 

#### Example

```csharp
using SendWithUs.Client;

var request1 = new SendRequest { TemplateId = "template123", RecipientAddress = "foo@example.com" };
var request2 = new SendRequest { TemplateId = "template567", RecipientAddress = "bar@example.com" };
var client = new SendWithUsClient("my-api-key");
var response = await client.ExecuteAsync(request1, request2);

foreach (var item in response.Items)
{
    if (item.IsSuccessStatusCode)
    {
        ...
    }

    ...
}
```
