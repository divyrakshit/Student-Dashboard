using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentDashboardAPI.Models;
using StudentDashboardAPI.Services;

namespace StudentDashboardAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IStudentService _svc;
    public DashboardController(IStudentService svc) => _svc = svc;

    // ─────────────────────────────────────────────────────────────────────────
    // SEQUENTIAL  –  Profile → Marks → Attendance  (one after another)
    // Expected time: ~6 seconds  (3 × 2s in sequence)
    // ─────────────────────────────────────────────────────────────────────────
    /// <summary>Load dashboard SEQUENTIALLY (Profile → Marks → Attendance). Expected ~6s.</summary>
    [HttpGet("sequential/{studentId}")]
    public async Task<IActionResult> GetSequential(int studentId)
    {
        var sw = Stopwatch.StartNew();

        var profile    = await _svc.GetProfileAsync(studentId);     // wait 2s
        var marks      = await _svc.GetMarksAsync(studentId);       // wait 2s more
        var attendance = await _svc.GetAttendanceAsync(studentId);  // wait 2s more

        sw.Stop();

        return Ok(new DashboardResponse
        {
            Profile          = profile,
            Marks            = marks,
            Attendance       = attendance,
            TimeTakenSeconds = Math.Round(sw.Elapsed.TotalSeconds, 3),
            ExecutionMode    = "Sequential – each task waited for the previous one to finish"
        });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PARALLEL  –  All three loaded simultaneously via Task.WhenAll()
    // Expected time: ~2 seconds  (all run at the same time)
    // ─────────────────────────────────────────────────────────────────────────
    /// <summary>Load dashboard in PARALLEL using Task.WhenAll(). Expected ~2s.</summary>
    [HttpGet("parallel/{studentId}")]
    public async Task<IActionResult> GetParallel(int studentId)
    {
        var sw = Stopwatch.StartNew();

        // Start all three tasks immediately – none waits for another
        var profileTask    = _svc.GetProfileAsync(studentId);
        var marksTask      = _svc.GetMarksAsync(studentId);
        var attendanceTask = _svc.GetAttendanceAsync(studentId);

        // Await all together – total time ≈ slowest single task (2s)
        await Task.WhenAll(profileTask, marksTask, attendanceTask);

        sw.Stop();

        return Ok(new DashboardResponse
        {
            Profile          = profileTask.Result,
            Marks            = marksTask.Result,
            Attendance       = attendanceTask.Result,
            TimeTakenSeconds = Math.Round(sw.Elapsed.TotalSeconds, 3),
            ExecutionMode    = "Parallel – all tasks ran simultaneously with Task.WhenAll()"
        });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // INDIVIDUAL ENDPOINTS  (for Swagger / Postman step-by-step testing)
    // ─────────────────────────────────────────────────────────────────────────
    /// <summary>Fetch Student Profile only (2s delay).</summary>
    [HttpGet("profile/{studentId}")]
    public async Task<IActionResult> GetProfile(int studentId)
    {
        var sw   = Stopwatch.StartNew();
        var data = await _svc.GetProfileAsync(studentId);
        sw.Stop();
        return Ok(new { data, timeTakenSeconds = Math.Round(sw.Elapsed.TotalSeconds, 3) });
    }

    /// <summary>Fetch Student Marks only (2s delay).</summary>
    [HttpGet("marks/{studentId}")]
    public async Task<IActionResult> GetMarks(int studentId)
    {
        var sw   = Stopwatch.StartNew();
        var data = await _svc.GetMarksAsync(studentId);
        sw.Stop();
        return Ok(new { data, timeTakenSeconds = Math.Round(sw.Elapsed.TotalSeconds, 3) });
    }

    /// <summary>Fetch Student Attendance only (2s delay).</summary>
    [HttpGet("attendance/{studentId}")]
    public async Task<IActionResult> GetAttendance(int studentId)
    {
        var sw   = Stopwatch.StartNew();
        var data = await _svc.GetAttendanceAsync(studentId);
        sw.Stop();
        return Ok(new { data, timeTakenSeconds = Math.Round(sw.Elapsed.TotalSeconds, 3) });
    }

    // ─────────────────────────────────────────────────────────────────────────
    // COMPARE  –  Runs both modes and returns timing side by side
    // ─────────────────────────────────────────────────────────────────────────
    /// <summary>Run Sequential then Parallel and compare execution times.</summary>
    [HttpGet("compare/{studentId}")]
    public async Task<IActionResult> Compare(int studentId)
    {
        // Sequential
        var sw1 = Stopwatch.StartNew();
        await _svc.GetProfileAsync(studentId);
        await _svc.GetMarksAsync(studentId);
        await _svc.GetAttendanceAsync(studentId);
        sw1.Stop();
        double seqTime = Math.Round(sw1.Elapsed.TotalSeconds, 3);

        // Parallel
        var sw2 = Stopwatch.StartNew();
        await Task.WhenAll(
            _svc.GetProfileAsync(studentId),
            _svc.GetMarksAsync(studentId),
            _svc.GetAttendanceAsync(studentId)
        );
        sw2.Stop();
        double parTime = Math.Round(sw2.Elapsed.TotalSeconds, 3);

        return Ok(new
        {
            SequentialSeconds = seqTime,
            ParallelSeconds   = parTime,
            TimeSavedSeconds  = Math.Round(seqTime - parTime, 3),
            SpeedupFactor     = $"{Math.Round(seqTime / parTime, 2)}x faster"
        });
    }
}
