# BlazorPrerenderHelper

This library provides a handful of helper functions for eliminating double data retrieval problem of WASM prerendered application.

## Quick start

Add the following line before `</body>` in your `_Host.cshtml`
```
<script src="_content/BlazorPrerenderHelper/scripts.js"></script>
@inject IPrerenderScriptGenerator PrerenderScriptGenerator
@Html.Raw(PrerenderScriptGenerator.Generate())
```

Add the following line in **both** of your client and server `Program.cs` or `Startup.cs`

for client:
```csharp
builder.Services.AddPrerenderHelperForClient();
```

for server:
```csharp
builder.Services.AddPrerenderHelperForServer();
```

Now, you may use the following example to eliminate double data retrieval problem.

`IPersonRepository.cs` (Shared)
```csharp
public interface IPersonRepository
{
    Task<Person> GetPerson(int id);
}
```

`PersonServerRepository.cs` (Server)
```csharp
public class PersonServerRepository : IPersonRepository
{
    public async Task<Person> GetPerson(int id)
    {
        return _dbContext.People.SingleOrDefault(person => person.Id == id);
    }
}
```

`PersonController.cs` (Server)
```csharp
[HttpGet]
[Route("api/person/{id}")]
public async Task<IActionResult> GetPerson(int id)
{
    var person = await _personRepo.GetPerson(id);
    return Ok(person);
}
```

`PersonClientRepository.cs` (Client)
```csharp
public class PersonClientRepository : IPersonRepository
{
    private readonly ISSRService _ssr;

    public async Task<Person> GetPerson(int id)
    {
        var ssrHint = await SSR.GetHint("person");  // this is instant return, because hint is retrieved from JS, which is generated during prerendering
        if (ssrHint.IsFound)
            return ssrHint.Result;
            
        // hint not found, call API to retrieve data as normal
        return await httpClient.GetFromJsonAsync("api/person/1");
    }
}
```

`Person.razor` (Client)
```csharp
@page "/"
@inject IPersonRepository PersonRepo

<div>Hello, @person.Name</div>

@*// The following code will generate JS that can be retrieved by `GetHint` method. In client context, the component is just a dummy component *@
<SSRHint Key="person" Value="_person" />

@code {

    private readonly Person? _person;
    
    protected override async Task OnInitializedAsync()
    {
        _person = PersonRepo.GetPerson(1);
    }

}
```
