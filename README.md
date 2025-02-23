# backend
Backend for teamdev project

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