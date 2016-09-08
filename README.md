SendWithUs.Client
=================

A .NET client implementation over the SendWithUs REST API.

Currently, the client only covers a small portion of the API surface--namely, sending email, rendering templates, and batch operations.

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
have values for `TemplateId` and `RecipientAddress`. All other properties are optional.

#### Minimal Example

```csharp
using SendWithUs.Client;

var request = new SendRequest("template123", "foo@example.com");
var client = new SendWithUsClient("my-api-key");
var response = await client.SendAsync(request);
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
var response = await client.SendAsync(request);
```

#### Typed Request Data

The type of the `Data` property on `SendRequest` is plain old object. If you want a strongly-typed `Data` property, use
`SendRequest<TData>` in lieu of `SendRequest`. This is merely a developer convenience. `SendRequest<TData>` shadows the 
`Data` property of `SendRequest` and casts it to the specified type.

### Rendering Templates

To render a template, you must instantiate a `RenderRequest` object to pass to `SendWithUsClient.RenderAsync`. A 
`RenderRequest` must have a value for `TemplateId`. All other properties are optional, although it's probably not 
very useful to omit the `Data` property.

The value of `RenderRequest.Data` can be any CLR object, as long as it serializes as a JSON object.

#### Example

```csharp
using SendWithUs.Client;

var generalLee = new Car { Make = "Dodge", Model = "Charger", Year = "1969" };
var request = new RenderRequest("template987", generalLee);
var client = new SendWithUsClient("my-api-key");
var response = await client.RenderAsync(request);
```

### Batching

To make a batch request, pass a collection of request objects to `SendWithUsClient.BatchAsync`. (Note that the only type
of requests currently supported are `SendRequest` and `RenderRequest`.)

#### Example

```csharp
using SendWithUs.Client;

var request1 = new SendRequest("template123", "foo@example.com");
var request2 = new SendRequest("template567", "bar@example.com");
var client = new SendWithUsClient("my-api-key");
var response = await client.BatchAsync(new List<IRequest> { request1, request2 });

foreach (var item in response.Items)
{
    if (item.StatusCode == HttpStatusCode.OK)
    {
        ...
    }

    ...
}
```
