# backend
Backend for teamdev project

## Running and stopping mongo for dev environment

```
\backend>start_mongo.bat
\backend>stop_mongo.bat
```

To completely drop container with running mongo:

```
\backend>stop_mongo.bat -d
```

## Deploying app

```
\backend>deploy.bat
...
[+] Running 2/3
 - Network backend_backend     Created                                                                                                                                              5.3s 
 ✔ Container backend-mongo-1   Healthy                                                                                                                                              4.4s 
 ✔ Container backend-server-1  Healthy

```

## Dropping deployed app

```
\backend>drop.bat
[+] Running 4/4
 ✔ Container backend-server-1   Removed                                                                                                                                             0.4s 
 ✔ Container backend-mongo-1    Removed                                                                                                                                             0.5s 
 ✔ Image backend-server:latest  Removed                                                                                                                                             0.0s 
 ✔ Network backend_backend      Removed                                                                                                                                             0.2s 
Total reclaimed space: 0B

```

## Creating mongo models

First, create model class:

```csharp
using Database;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models;

[Model("hello")] // Model annotation containing mongo collection name. MUST BE ADDED TO EACH MODEL!!!
public class HelloModel
{
    // Optional Id field, add it only if u need it
    [BsonId] public ObjectId Id { get; set; }
    
    // Payload fields
    public string Message { get; set; }
}

```

Now you can request repository instance from di container:

```csharp
[ApiController]
[Route("/")]
public class HelloController : ControllerBase
{
    private readonly IRepository<ObjectId, HelloModel> _hellos;

    public HelloController(IRepository<ObjectId, HelloModel> hellos)
    {
        _hellos = hellos;
    }
    ...
}

```
