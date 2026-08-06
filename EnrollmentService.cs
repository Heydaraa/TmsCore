public class EnrollmentService
{
    // TODO 1 & 2: Action delegate to hold subscriber callbacks
    public Action<Student>? Listener { get; set; }

    public EnrollmentRecord ProcessRegistration(Student? student, Course? course)
    {
        // TODO 1: Guard clauses - fail fast for nulls or invalid capacity
        if (student is null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        if (course is null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        if (course.Capacity <= 0 || course.EnrolledCount >= course.Capacity)
        {
            throw new CapacityReachedException(course.Code);
        }

        // TODO 2: Switch expression to classify academic standing
        string standing = student.GPA switch
        {
            >= 3.5m => "Honors",
            >= 2.5m => "GoodStanding",
            _ => "AcademicWarning"
        };

        Console.WriteLine($"{student.Name} is in {standing}.");

        // TODO 3: Return a new EnrollmentRecord
        return new EnrollmentRecord(student.Id, course.Code, DateTime.UtcNow);
    }

    // TODO 3: Check if Listener is not null and invoke it
    public void FinalizeEnrollment(Student s)
    {
        Console.WriteLine("Persisting to database...");
        
        // Call the delegate if someone subscribed to it
        Listener?.Invoke(s);
    }
}