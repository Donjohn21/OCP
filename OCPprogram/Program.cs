using System;
using System.Collections.Generic;

//john aquino 2022-0417
public interface ICourseSubscription
{
    void Subscribe(Student student);
    List<Student> GetSubscribedStudents();
}

public abstract class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; }

    protected ICourseSubscription subscription;

    protected Course(ICourseSubscription subscription)
    {
        this.subscription = subscription ?? throw new ArgumentNullException(nameof(subscription));
    }

    public void Subscribe(Student student)
    {
        subscription.Subscribe(student);
    }

    public List<Student> GetSubscribedStudents()
    {
        return subscription.GetSubscribedStudents();
    }
}

public class OnlineCourse : Course
{
    public OnlineCourse() : base(new OnlineCourseSubscription())
    {
        Title = "Curso de Programación en Línea"; 
    }
}

public class OfflineCourse : Course
{
    public OfflineCourse() : base(new OfflineCourseSubscription())
    {
        Title = "Curso de Cálculo Integral Presencial"; 
    }
}

public class HybridCourse : Course
{
    public HybridCourse(OnlineCourse onlineCourse, OfflineCourse offlineCourse)
        : base(new HybridCourseSubscription(onlineCourse, offlineCourse))
    {
        Title = "Curso de Inglés Híbrido";
    }
}

// Implementación de las suscripciones
public class OnlineCourseSubscription : ICourseSubscription
{
    private readonly List<Student> students = new List<Student>();

    public void Subscribe(Student student)
    {
        if (!students.Contains(student))
        {
            students.Add(student);
            Console.WriteLine($"{student.Name} ha sido suscrito al curso en línea.");
        }
    }

    public List<Student> GetSubscribedStudents()
    {
        return students;
    }
}

public class OfflineCourseSubscription : ICourseSubscription
{
    private readonly List<Student> students = new List<Student>();

    public void Subscribe(Student student)
    {
        if (!students.Contains(student))
        {
            students.Add(student);
            Console.WriteLine($"{student.Name} ha sido suscrito al curso presencial.");
        }
    }

    public List<Student> GetSubscribedStudents()
    {
        return students;
    }
}

public class HybridCourseSubscription : ICourseSubscription
{
    private readonly OnlineCourse onlineCourse;
    private readonly OfflineCourse offlineCourse;

    public HybridCourseSubscription(OnlineCourse online, OfflineCourse offline)
    {
        onlineCourse = online ?? throw new ArgumentNullException(nameof(online));
        offlineCourse = offline ?? throw new ArgumentNullException(nameof(offline));
    }

    public void Subscribe(Student student)
    {
        onlineCourse.Subscribe(student);
        offlineCourse.Subscribe(student);
        Console.WriteLine($"{student.Name} ha sido suscrito al curso híbrido.");
    }

    public List<Student> GetSubscribedStudents()
    {
        var allStudents = new HashSet<Student>(onlineCourse.GetSubscribedStudents());
        allStudents.UnionWith(offlineCourse.GetSubscribedStudents());
        return new List<Student>(allStudents);
    }
}

public class Student
{
    public string Name { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        Student student = new Student { Name = "Juan" };

        OnlineCourse onlineCourse = new OnlineCourse { CourseId = 1 };
        OfflineCourse offlineCourse = new OfflineCourse { CourseId = 2 };
        HybridCourse hybridCourse = new HybridCourse(onlineCourse, offlineCourse) { CourseId = 3 };

        onlineCourse.Subscribe(student);
        offlineCourse.Subscribe(student);
        hybridCourse.Subscribe(student);
    }
}
