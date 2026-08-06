// Exercise 1
// string region = null;
// Console.WriteLine(region.ToUpper());// this crashes system creates error of nullReference exception


// Declare the variable as nullable with '?'
// This tells the compiler: "I know this might be null. I accept responsibility.
 string? region = null;


// Null-conditional operator '?.' — skip the call if null
// If region is null, ToUpper() never executes. No crash.
string? upperRegion = region?.ToUpper();
Console.WriteLine($"Region (conditional): {upperRegion}");

// Null-coalescing operator '??' — provide a fallback value
string displayRegion = region ?? "Unassigned";
Console.WriteLine($"Region (coalesced): {displayRegion}");

region ??= "Addis Ababa";
Console.WriteLine($"Region (assigned): {region}");
// declaring varibale TMS core 
string studentName = "Abeba";
string studentId = "STU-001";

int enrollmentCount = 3;

decimal grantAmount = 1999.99m;

DateTime enrolledAt = DateTime.UtcNow;

string? campusRegion = null;

Console.WriteLine($"Student: {studentName} ({studentId})");
Console.WriteLine($"Courses: {enrollmentCount}");
Console.WriteLine($"Grant: {grantAmount:F2}");
Console.WriteLine($"Enrolled: {enrolledAt:yyyy-MM-dd}");
Console.WriteLine($"Campus: {campusRegion ?? "Not assigned"}");
//Exercise 2
// Legacy implementation — the bug that caused the audit failure
// double grantPerStudent = 1999.99;
// double totalAllocation = grantPerStudent * 100_000;
// Console.WriteLine($"Total allocated (double): {totalAllocation}");

// Fixed implementation — exact financial math

decimal grantPerStudent = 1999.99m;
decimal totalAllocation = grantPerStudent * 100_000m;
Console.WriteLine($"Total allocated (decimal): {totalAllocation}");
Console.WriteLine($"Total allocated (formatted): {totalAllocation:F2}");
// exercise 3
var enrollment =
    new EnrollmentRecord(
        "STU-001",
        "CS-401",
        DateTime.UtcNow);

Console.WriteLine(enrollment);
//  enrollment.CourseCode = "HACKED"; 
// ERROR: init-only property
// Non-destructive copy — creates a NEW record with one field chan
var corrected =
    enrollment with
    {
        CourseCode = "CS-402"
    };

Console.WriteLine(corrected);

var duplicate =
    new EnrollmentRecord(
        "STU-001",
        "CS-401",
        enrollment.EnrolledAt);

Console.WriteLine($"Same data? {enrollment == duplicate}");

var course = new Course { Code = "CS-401", Title = "Advanced C#", Capacity = 30 };
Console.WriteLine($"Course: {course.Title} (Capacity: {course.Capacity})");

// Invalid capacity — should throw
try
{
    course.Capacity = -5;
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

// Invalid title — should throw
try
{
    course.Title = "";
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught: {ex.Message}");
}

// var s = new Student { Id = "S1", Name = "Abeba", Age = 20, GPA = 3.8m };
// Console.WriteLine($"Student: {s.Name}, GPA: {s.GPA}");

// These should throw — try each one if you'd like to test validation:
// new Student { Id = "S2", Name = "", Age = 20, GPA = 3.0m };
// new Student { Id = "S3", Name = "Test", Age = 12, GPA = 3.0m };
// new Student { Id = "S4", Name = "Test", Age = 20, GPA = 5.0m };

void PrintGradeReport(IEnumerable<IGradable> assessments)
{
    Console.WriteLine("--- Grade Report---");
    foreach (var item in assessments)
    {
        Console.WriteLine($"{item.Title}: {item.CalculateGrade():F2}%");
    }
}

// Test it — one array holds two completely different types
IGradable[] cohortAssessments = [
    new Quiz { Title = "C# Basics", CorrectAnswers = 18, TotalQuestions = 20 },
    new LabAssignment { Title = "Registration API", FunctionalityScore = 90m, CodeQualityScore = 85m }
];

PrintGradeReport(cohortAssessments);

// --- Exercise 4: Enrollment Service Tests ---
Console.WriteLine("\n--- Exercise 4: Enrollment Service ---");

var service = new EnrollmentService();

// Test 1: Valid registration
var validStudent = new Student { Id = "S1", Name = "Abeba", Age = 20, GPA = 3.8m };
var validCourse = new Course { Code = "CS-401", Title = "Advanced C#", Capacity = 30 };
var registrationResult = service.ProcessRegistration(validStudent, validCourse);
Console.WriteLine($"Enrolled: {registrationResult.StudentId} in {registrationResult.CourseCode}");

// Test 2: Null student should throw ArgumentNullException
try
{
    service.ProcessRegistration(null, validCourse);
}
catch (ArgumentNullException ex)
{
    Console.WriteLine($"Guard caught: {ex.ParamName}");
}

// Test 3: Full course should throw InvalidOperationException
var fullCourse = new Course { Code = "CS-402", Title = "Full Course", Capacity = 1 };
fullCourse.EnrolledCount = 1;

try
{
    service.ProcessRegistration(validStudent, fullCourse);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Business rule: {ex.Message}");
}

// --- Exercise 5: The Analytics Dashboard ---
Console.WriteLine("\n--- Exercise 5: The Analytics Dashboard ---");

// Step 1: Student Data using C# 12+ Collection Expressions
List<Student> students = [
    new Student { Id = "S1", Name = "Abeba", Age = 22, GPA = 3.8m },
    new Student { Id = "S2", Name = "Kidane", Age = 21, GPA = 2.4m },
    new Student { Id = "S3", Name = "Dawit", Age = 20, GPA = 3.1m },
    new Student { Id = "S4", Name = "Sara", Age = 23, GPA = 3.9m },
    new Student { Id = "S5", Name = "Frehiwot", Age = 19, GPA = 2.0m },
    new Student { Id = "S6", Name = "Yonas", Age = 24, GPA = 3.5m },
    new Student { Id = "S7", Name = "Meron", Age = 22, GPA = 1.8m },
    new Student { Id = "S8", Name = "Tesfaye", Age = 21, GPA = 2.9m }
];

// Step 2: Build the Honors Leaderboard (Filter -> Sort -> Project -> Materialize)
List<string> leaderboard = students
    .Where(s => s.GPA >= 3.5m)
    .OrderByDescending(s => s.GPA)
    .Select(s => s.Name)
    .ToList();

Console.WriteLine($"Found {leaderboard.Count} Honors Students:");
foreach (var name in leaderboard)
{
    Console.WriteLine($"- {name}");
}

// Step 3: Class Average
decimal averageGpa = students.Count > 0 ? students.Average(s => s.GPA) : 0m;
Console.WriteLine($"\nClass Average GPA: {averageGpa:F2}");

// Step 4: Group by Academic Standing
var standingGroups = students.GroupBy(s => s.GPA switch
{
    >= 3.5m => "Honors",
    >= 2.5m => "GoodStanding",
    >= 2.0m => "Probation",
    _ => "Academic Warning"
});

Console.WriteLine("\n--- Academic Standing Report ---");
foreach (var group in standingGroups)
{
    Console.WriteLine($"\n{group.Key} ({group.Count()}):");
    foreach (var s in group)
    {
        Console.WriteLine($"  {s.Name} GPA: {s.GPA}");
    }
}

// Step 5: Collection Expressions with Spread Operator (..)
string[] backendCourses = ["C#", "ASP.NET Core"];
string[] frontendCourses = ["TypeScript", "Angular"];
string[] allCourses = [.. backendCourses, .. frontendCourses, "Capstone"];

Console.WriteLine($"\nFull curriculum: {string.Join(", ", allCourses)}");
// exercise 6
 using System.Diagnostics;

// Simulate 5 database calls, each taking 300ms
// THE WRONG WAY: Blocking with Thread.Sleep
var sw = Stopwatch.StartNew();
for (int i = 0; i < 5; i++)
{
    Thread.Sleep(300); // Thread is HELD for 300ms cannot serve anyone else
}
Console.WriteLine($"Blocking sequential: {sw.ElapsedMilliseconds}ms");

// ASYNC BUT STILL SEQUENTIAL: Thread released, but calls are one-at-a-time
sw.Restart();
for (int i = 0; i < 5; i++)
{
    await Task.Delay(300); // Thread released while waiting but still sequential
}
Console.WriteLine($"Async sequential: {sw.ElapsedMilliseconds}ms");

// THE RIGHT WAY: Async parallel all 5 start simultaneously
sw.Restart();
var tasks = Enumerable.Range(0, 5).Select(_ => Task.Delay(300));
await Task.WhenAll(tasks);
Console.WriteLine($"Async parallel: {sw.ElapsedMilliseconds}ms");

async Task<Student> FetchStudentAsync(string id)
{
    Console.WriteLine($" Fetching {id}...");
    await Task.Delay(300); // Simulate database latency
    return new Student
    {
        Id = id,
        Name = $"Student-{id}",
        Age = 20,
        GPA = id switch
        {
            "S1" => 3.8m,
            "S2" => 2.4m,
            "S3" => 3.5m,
            "S4" => 1.9m,
            "S5" => 3.2m,
            _ => 2.5m
        }
    };
}

//  async Task<Student> FetchStudentAsync(string id)
// {
//     Console.WriteLine($" Fetching {id}...");
//     await Task.Delay(300); // Simulate database latency
//     return new Student
//     {
//         Id = id,
//         Name = $"Student-{id}",
//         Age = 20,
//         GPA = id switch
//         {
//             "S1" => 3.8m,
//             "S2" => 2.4m,
//             "S3" => 3.5m,
//             "S4" => 1.9m,
//             "S5" => 3.2m,
//             _ => 2.5m
//         }
//     };
// }

async Task<Course> FetchCourseAsync(string code)
{
    Console.WriteLine($" Fetching course {code}...");
    await Task.Delay(200); // Simulate database latency
    return new Course
    {
        Code = code,
        Title = $"Course-{code}",
        Capacity = code switch
        {
            "CRS-101" => 2,
            "CRS-201" => 30,
            "CRS-301" => 15,
            _ => 25
        }
    };
}

sw.Restart();
// Start all fetches simultaneously students AND courses
string[] studentIds = ["S1", "S2", "S3", "S4", "S5"];
string[] courseCodes = ["CRS-101", "CRS-201", "CRS-301"];

var studentTasks = studentIds.Select(id => FetchStudentAsync(id));
var courseTasks = courseCodes.Select(code => FetchCourseAsync(code));

// Both arrays load concurrently
Student[] students = await Task.WhenAll(studentTasks);
Course[] courses = await Task.WhenAll(courseTasks);

Console.WriteLine($"\nLoaded {students.Length} students and {courses.Length} courses in {sw.ElapsedMilliseconds}ms");

foreach (var s in students)
{
    Console.WriteLine($" {s.Name} GPA: {s.GPA}");
}

var enrollCourse = new Course { Code = "CRS-101", Title = "C# Mastery", Capacity = 2 };
var enrollService = new EnrollmentService();
var enrollments = new List<EnrollmentRecord>();
var failures = new List<string>();

sw.Restart();
foreach (var student in students)
{
    try
    {
        var record = enrollService.ProcessRegistration(student, enrollCourse);
        enrollCourse.EnrolledCount++;
        enrollments.Add(record);
        Console.WriteLine($" Enrolled: {student.Name}");
    }
    catch (InvalidOperationException ex)
    {
        failures.Add($"{student.Name}: {ex.Message}");
        Console.WriteLine($" Rejected: {student.Name} {ex.Message}");
    }
}


try
{
    var overflowCourse = new Course { Code = "CRS-999", Title = "Overflow Test", Capacity = 1, EnrolledCount = 1 };
    enrollService.ProcessRegistration(
        new Student { Id = "S99", Name = "Test", Age = 20, GPA = 3.0m },
        overflowCourse
    );
}
catch (CapacityReachedException ex)
{
    Console.WriteLine($"\nDomain exception caught:");
    Console.WriteLine($" Course: {ex.CourseCode}");
    Console.WriteLine($" Message: {ex.Message}");
}

// Stop the timer
sw.Stop();

// Calculate class average GPA from loaded students
decimal classAverage = students.Length > 0
    ? students.Average(s => s.GPA)
    : 0m;

// Print the final report
Console.WriteLine("\n========== ENROLLMENT SUMMARY ==========");
Console.WriteLine($"Total students loaded:  {students.Length}");
Console.WriteLine($"Successful enrollments: {enrollments.Count}");
Console.WriteLine($"Failed enrollments:     {failures.Count}");
Console.WriteLine($"Class average GPA:      {classAverage:F2}");
Console.WriteLine($"Total elapsed time:     {sw.ElapsedMilliseconds}ms");

if (failures.Count > 0)
{
    Console.WriteLine("\n--- Failure Details ---");
    foreach (var failure in failures)
    {
        Console.WriteLine($" {failure}");
    }
}
Console.WriteLine("========================================");

// --- Optional Extension: The Modular Audit Path ---
Console.WriteLine("\n--- Modular Audit Path ---");

var auditService = new EnrollmentService();

// Attach lambda listener
auditService.Listener = s => 
    Console.WriteLine($"SMS SENT: Welcome to the TMS, {s.Name}!");

// Trigger enrollment finalization
var sampleStudent = new Student { Id = "STU-001", Name = "Abeba", Age = 21, GPA = 3.8m };
auditService.FinalizeEnrollment(sampleStudent);