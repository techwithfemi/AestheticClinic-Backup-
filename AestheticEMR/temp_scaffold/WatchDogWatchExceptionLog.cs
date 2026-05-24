using System;
using System.Collections.Generic;

namespace AestheticEMR.Server;

public partial class WatchDogWatchExceptionLog
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public string? StackTrace { get; set; }

    public string? TypeOf { get; set; }

    public string? Source { get; set; }

    public string? Path { get; set; }

    public string? Method { get; set; }

    public string? QueryString { get; set; }

    public string? RequestBody { get; set; }

    public string EncounteredAt { get; set; } = null!;
}
