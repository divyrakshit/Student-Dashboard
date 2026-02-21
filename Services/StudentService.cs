using StudentDashboardAPI.Models;

namespace StudentDashboardAPI.Services;

public interface IStudentService
{
    Task<StudentProfile>    GetProfileAsync(int studentId);
    Task<StudentMarks>      GetMarksAsync(int studentId);
    Task<StudentAttendance> GetAttendanceAsync(int studentId);
}

public class StudentService : IStudentService
{
    // Each method simulates a 2-second delay to represent real I/O (DB / API call)
    private const int DelayMs = 2000;

    public async Task<StudentProfile> GetProfileAsync(int studentId)
    {
        await Task.Delay(DelayMs);          // ← simulated 2s delay
        return new StudentProfile
        {
            StudentId  = studentId,
            Name       = "Rahul Sharma",
            Email      = "rahul.sharma@college.edu",
            Course     = "B.Tech Computer Science",
            Year       = 3,
            Department = "Computer Science & Engineering"
        };
    }

    public async Task<StudentMarks> GetMarksAsync(int studentId)
    {
        await Task.Delay(DelayMs);          // ← simulated 2s delay
        var subjects = new List<SubjectMark>
        {
            new() { Subject = "Data Structures",      MarksObtained = 88, MaxMarks = 100 },
            new() { Subject = "Operating Systems",    MarksObtained = 75, MaxMarks = 100 },
            new() { Subject = "Database Management",  MarksObtained = 91, MaxMarks = 100 },
            new() { Subject = "Computer Networks",    MarksObtained = 82, MaxMarks = 100 },
            new() { Subject = "Software Engineering", MarksObtained = 79, MaxMarks = 100 },
        };
        double pct = subjects.Average(s => (double)s.MarksObtained / s.MaxMarks * 100);
        return new StudentMarks
        {
            StudentId       = studentId,
            Subjects        = subjects,
            TotalPercentage = Math.Round(pct, 2),
            Grade           = pct >= 90 ? "A+" : pct >= 80 ? "A" : pct >= 70 ? "B" : "C"
        };
    }

    public async Task<StudentAttendance> GetAttendanceAsync(int studentId)
    {
        await Task.Delay(DelayMs);          // ← simulated 2s delay
        var subjectWise = new List<SubjectAttendance>
        {
            new() { Subject = "Data Structures",      Attended = 42, Total = 45 },
            new() { Subject = "Operating Systems",    Attended = 38, Total = 45 },
            new() { Subject = "Database Management",  Attended = 44, Total = 45 },
            new() { Subject = "Computer Networks",    Attended = 35, Total = 45 },
            new() { Subject = "Software Engineering", Attended = 40, Total = 45 },
        };
        int total    = subjectWise.Sum(s => s.Total);
        int attended = subjectWise.Sum(s => s.Attended);
        double attPct = Math.Round((double)attended / total * 100, 2);
        return new StudentAttendance
        {
            StudentId           = studentId,
            TotalClasses        = total,
            AttendedClasses     = attended,
            AttendancePercentage = attPct,
            Status              = attPct >= 75 ? "Eligible" : "Detained",
            SubjectWise         = subjectWise
        };
    }
}
