var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BlazorAppWidgets>("blazorappwidgets");

builder.Build().Run();
