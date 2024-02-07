using System.Net.Sockets;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireWithSeq_ApiService>("apiservice");

builder
    .AddResource(new ContainerResource("seq"))
    .WithAnnotation(new EndpointAnnotation(ProtocolType.Tcp, uriScheme: "http", name: "seq", port: 5341, containerPort: 80))
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithAnnotation(new ContainerImageAnnotation { Image = "datalust/seq", Tag = "latest" });

builder.AddProject<Projects.AspireWithSeq_Web>("webfrontend")
    .WithReference(apiService);

builder.Build().Run();
