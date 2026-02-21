namespace StudentDashboardAPI.Models;

public class StudentProfile
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Course { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Department { get; set; } = string.Empty;
}

public class SubjectMark
{
    public string Subject { get; set; } = string.Empty;
    public int MarksObtained { get; set; }
    public int MaxMarks { get; set; }
}

public class StudentMarks
{
    public int StudentId { get; set; }
    public List<SubjectMark> Subjects { get; set; } = new();
    public double TotalPercentage { get; set; }
    public string Grade { get; set; } = string.Empty;
}

public class SubjectAttendance
{
    public string Subject { get; set; } = string.Empty;
    public int Attended { get; set; }
    public int Total { get; set; }
}

public class StudentAttendance
{
    public int StudentId { get; set; }
    public int TotalClasses { get; set; }
    public int AttendedClasses { get; set; }
    public double AttendancePercentage { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<SubjectAttendance> SubjectWise { get; set; } = new();
}

public class DashboardResponse
{
    public StudentProfile Profile { get; set; } = null!;
    public StudentMarks Marks { get; set; } = null!;
    public StudentAttendance Attendance { get; set; } = null!;
    public double TimeTakenSeconds { get; set; }
    public string ExecutionMode { get; set; } = string.Empty;
}
