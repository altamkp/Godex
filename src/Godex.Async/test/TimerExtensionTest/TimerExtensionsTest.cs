using Godot;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Timer = Godot.Timer;

namespace Godex.Async.Tests;

public partial class TimerExtensionsTest : Node {
    public override void _Ready() {
        _ = TimerCompleted_Succeeds();
        _ = TimerCanceled_Succeeds();
    }

    private async Task TimerCompleted_Succeeds() {
        var timer = new Timer();
        var cts = new CancellationTokenSource();
        AddChild(timer);

        var start = GetTime();
        timer.Start(2);
        cts.CancelAfter(TimeSpan.FromSeconds(3));
        await timer.TimeoutAsync(cts.Token);

        var time = GetTime() - start;
        Assert.True(time > 1 && time < 3);
        Assert.False(cts.IsCancellationRequested);
        GD.Print("Task completed.");
    }

    private async Task TimerCanceled_Succeeds() {
        var timer = new Timer();
        var cts = new CancellationTokenSource();
        AddChild(timer);

        var start = GetTime();
        timer.Start(3);
        cts.CancelAfter(TimeSpan.FromSeconds(2));
        await timer.TimeoutAsync(cts.Token);

        var time = GetTime() - start;
        Assert.True(time > 1 && time < 3);
        Assert.True(cts.IsCancellationRequested);
        GD.Print("Task canceled.");
    }

    private static double GetTime() => (double)Time.GetTicksMsec() / 1000;
}
