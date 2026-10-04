namespace Godex.Async.Tests;

using GdUnit4;
using Godot;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Timer = Godot.Timer;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public class TimerExtensionsTest {
    // A long enough wait that the assertion is not measuring Godot's timer
    // quantisation: Timer decrements by whole process deltas, so very short waits
    // are quantised coarsely and were observed firing at 0.023s for a 0.05s wait.
    private const double DURATION = 1.0;

    // The lower bound only rules out "fired immediately", the upper bound rules
    // out "never fired", so neither is tight enough to be flaky.
    private const double MIN_ELAPSED = DURATION * 0.5;
    private const double MAX_ELAPSED = 15.0;

    private Timer CreateTimer(double waitTime) {
        Timer timer = AddNode(new Timer(), true);
        timer.OneShot = true;
        timer.WaitTime = waitTime;
        // WaitTime alone does not run the timer, the timeout only fires once started.
        timer.Start();
        return timer;
    }

    private static double Now() => Time.GetTicksMsec() / 1000.0;

    [TestCase]
    public async Task TimeoutAsyncCompletesWhenTheTimerExpires() {
        Timer timer = CreateTimer(DURATION);

        double start = Now();
        await timer.TimeoutAsync();
        double elapsed = Now() - start;

        AssertFloat(elapsed).IsBetween(MIN_ELAPSED, MAX_ELAPSED);
    }

    [TestCase]
    public async Task TimeoutAsyncWithATokenCompletesWhenTheTimerExpires() {
        Timer timer = CreateTimer(DURATION);
        CancellationTokenSource cts = new CancellationTokenSource();

        double start = Now();
        await timer.TimeoutAsync(cts.Token);
        double elapsed = Now() - start;

        AssertFloat(elapsed).IsBetween(MIN_ELAPSED, MAX_ELAPSED);
        AssertBool(cts.IsCancellationRequested).IsFalse();
        cts.Dispose();
    }

    [TestCase]
    public async Task TimeoutAsyncCompletesEarlyWhenCancelled() {
        Timer timer = CreateTimer(30.0);
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds((int)(DURATION * 500)));

        double start = Now();
        await timer.TimeoutAsync(cts.Token);
        double elapsed = Now() - start;

        AssertBool(cts.IsCancellationRequested).IsTrue();
        // The 30s timer never fires, so returning early is the only way to finish.
        AssertFloat(elapsed).IsLess(MAX_ELAPSED);
        cts.Dispose();
    }

    [TestCase]
    public void AnAlreadyCancelledTokenCompletesImmediately() {
        Timer timer = CreateTimer(30.0);
        CancellationTokenSource cts = new CancellationTokenSource();
        cts.Cancel();

        CancellableSignalAwaiter awaiter = timer.TimeoutAsync(cts.Token);

        AssertBool(awaiter.IsCompleted).IsTrue();
        cts.Dispose();
    }

    [TestCase]
    public void ACancelledAwaiterReportsCompletionWithoutTheSignal() {
        Timer timer = CreateTimer(30.0);
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellableSignalAwaiter awaiter = timer.TimeoutAsync(cts.Token);

        AssertBool(awaiter.IsCompleted).IsFalse();

        cts.Cancel();

        AssertBool(awaiter.IsCompleted).IsTrue();
        cts.Dispose();
    }
}
