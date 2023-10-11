using System;
using System.Collections.Generic;

// University class
public class University
{
    public string Name { get; }
    public List<Department> Departments { get; }
    public List<Venue> Venues { get; }

    public University(string name)
    {
        Name = name;
        Departments = new List<Department>();
        Venues = new List<Venue>();
    }

    public void AddDepartment(Department department)
    {
        Departments.Add(department);
    }

    public void AddVenue(Venue venue)
    {
        Venues.Add(venue);
    }
}

public class Venue
{
    public int VenueId { get; }
    public string Location { get; }
    public string VenueName { get; }
    public int Capacity { get; }
    public List<Timeslot> Timeslots { get; }

    public Venue(int venueId, string location, string venueName, int capacity)
    {
        VenueId = venueId;
        Location = location;
        VenueName = venueName;
        Capacity = capacity;
        Timeslots = new List<Timeslot>();
    }

    public void BookTimeslot(Timeslot timeslot)
    {
        if (!ConflictWith(timeslot))
        {
            Timeslots.Add(timeslot);
        }
    }

    public void UnbookTimeslot(Timeslot timeslot)
    {
        Timeslots.Remove(timeslot);
    }

    public bool ConflictWith(Timeslot timeslot)
    {
        foreach (var existingTimeslot in Timeslots)
        {
            if (existingTimeslot.ConflictsWith(timeslot))
            {
                // Need to be updated recurssion
                return true;
            }
        }
        return false;
    }

    public bool Backtrack(Timeslot timeslot)
    {
        foreach (var existingTimeslot in Timeslots)
        {
            // If no conflict can be resolved , return False Otherwise, return True

        }
        return false;
    }
}

public class Department
{
    public string Name { get; }
    public List<Unit> Units { get; }
// public List<Staff> Staff { get; }

    public Department(string name)
    {
        Name = name;
        Units = new List<Unit>();
        //Staff = new List<Staff>();
    }
    
    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
    }

}


public class Timeslot
{
    public int TimeSlotId { get; }
    public string DayOfWeek { get; }
    public DateTime StartTime { get; }
    public DateTime EndTime { get; }
    public TimeSpan Duration { get; }

    public Timeslot(int timeSlotId, string dayOfWeek, DateTime startTime, DateTime endTime)
    {
        TimeSlotId = timeSlotId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        Duration = EndTime - StartTime;
    }

    public bool ConflictsWith(Timeslot otherTimeslot)
    {
        if (DayOfWeek != otherTimeslot.DayOfWeek)
        {
            return false; // Different days, no conflict
        }

        if (StartTime >= otherTimeslot.EndTime || EndTime <= otherTimeslot.StartTime)
        {
            return false; // No overlap, no conflict
        }

        return true; // Overlaps, conflict
    }

    public void SetTime(DateTime start, DateTime end)
    {
       // StartTime = start;
        //EndTime = end;
       // Duration = EndTime - StartTime;
    }
}

public class Unit
{
    public string UnitId { get; }
    public string UnitName { get; }
    public int UnitCredit { get; }
    public int MaxStudents { get; }
    public Unit PrerequisiteUnit { get; }
    public string Requirement { get; }
    public string AssignedStaff { get; set; }
   // public List<Student> EnrolledStudents { get; }
    public List<Timeslot> AvailableTimeslots { get; }
    


    public Unit(string unitId, string unitName, int unitCredit, int maxStudents, Unit prerequisiteUnit, string requirement)
    {
        UnitId = unitId;
        UnitName = unitName;
        UnitCredit = unitCredit;
        MaxStudents = maxStudents;
        PrerequisiteUnit = prerequisiteUnit;
        Requirement = requirement;
        AssignedStaff = null;
       // EnrolledStudents = new List<Student>();
        AvailableTimeslots = new List<Timeslot>();
    }
    static List<Unit> ReadUnitsFromCSV(string filePath)
    {
        List<Unit> units = new List<Unit>();
        var lines = File.ReadLines(filePath);

        foreach(var line in lines.Skip(1)) // Skip the header line
    {
            var fields = line.Split(',');
            if (fields.Length >= 2) // CSV 파일에서 "Unit Code"와 "Unit Title" 필드만 사용
            {
                string unitId = fields[0]; // "Unit Code"를 "unitId"로 매핑
                string unitName = fields[1]; // "Unit Title"을 "unitName"으로 매핑

                // 나머지 필드는 필요에 따라 파싱할 수 있습니다.
                int unitCredit = 0;
                int maxStudents = 0;

                Unit unit = new Unit(unitId, unitName, unitCredit, maxStudents, null, null);
                units.Add(unit);
            }
        }

        return units;
    }

    public void AssignStaff(string staff)
    {
        AssignedStaff = staff;
    }

    public void UnassignStaff()
    {
        AssignedStaff = null;
    }

    public void AddTimeslot(Timeslot timeslot)
    {
        if (!ConflictWith(timeslot))
        {
            AvailableTimeslots.Add(timeslot);
        }
    }

    public void RemoveTimeslot(Timeslot timeslot)
    {
        AvailableTimeslots.Remove(timeslot);
    }

    public bool ConflictWith(Timeslot timeslot)
    {
        foreach (var existingTimeslot in AvailableTimeslots)
        {
            if (existingTimeslot.ConflictsWith(timeslot))
            {
                return true;
            }
        }
        return false;
    }
}

class Program
{
    static void Main()
    {
        University university = new University("Sample University");

        // Create a department
        Department computerScience = new Department("Computer Science");

        // Create a venue
        Venue venue1 = new Venue(1, "Campus A", "Lecture Hall 101", 150);

        // Create some timeslots
        Timeslot timeslot1 = new Timeslot(1, "Monday", new DateTime(2023, 10, 16, 9, 0, 0), new DateTime(2023, 10, 16, 10, 30, 0));
        Timeslot timeslot2 = new Timeslot(2, "Monday", new DateTime(2023, 10, 16, 10, 45, 0), new DateTime(2023, 10, 16, 12, 15, 0));

        // Create a unit
        Unit computerScience101 = new Unit("unit1001", "Introduction to Computer Science", 3, 30, null, "None");

        // Assign staff to the unit
        //Staff professorSmith = new Staff("Prof. Smith");
        computerScience101.AssignStaff("professorSmith");

        // Add timeslots to the unit
        computerScience101.AddTimeslot(timeslot1);
        computerScience101.AddTimeslot(timeslot2);

        // Add the department to the university
        university.AddDepartment(computerScience);

        // Output some information
        Console.WriteLine("University Name: " + university.Name);
        Console.WriteLine("Department Name: " + computerScience.Name);
        Console.WriteLine("Venue Name: " + venue1.VenueName);
        Console.WriteLine("Unit Name: " + computerScience101.UnitName);


        Console.WriteLine("Timeslots for the Unit:");
        foreach (var timeslot in computerScience101.AvailableTimeslots)
        {
            Console.WriteLine("TimeSlot ID: " + timeslot.TimeSlotId);
            Console.WriteLine("Day of Week: " + timeslot.DayOfWeek);
            Console.WriteLine("Start Time: " + timeslot.StartTime);
            Console.WriteLine("End Time: " + timeslot.EndTime);
            Console.WriteLine();
        }
    }
}