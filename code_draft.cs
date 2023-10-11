using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    public static List<Venue> ReadVenuesFromCSV(string filePath)
    {
        List<Venue> venues = new List<Venue>();

        using (var reader = new StreamReader(filePath)) // Open the file and ensure it's properly closed
        {
            // Skip the first line (column headers)
            bool firstLine = true;
            int lineNumber = 0; // Initialize a line number counter

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                lineNumber++; // Increment the line number for each line read

                if (firstLine)
                {
                    firstLine = false;
                    continue; // Skip the first line
                }

                var fields = line.Split(','); // Change the delimiter to a comma
                if (fields.Length >= 4)
                {
                    int venueId = int.Parse(fields[0]);
                    string venueName = fields[1];
                    string location = fields[2];
                    int capacity = int.Parse(fields[3]);

                    //Console.WriteLine("Read lines: " + lineNumber); // Use lineNumber to count lines
                    Venue venue = new Venue(venueId, location, venueName, capacity);
                    venues.Add(venue);
                }
            }
        }

        return venues;
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
    public string Lecturer { get; set; }
    // public List<Student> EnrolledStudents { get; }
    public List<Timeslot> AvailableTimeslots { get; }



    public Unit(string unitId, string unitName, string lecturer, int unitCredit, int maxStudents, Unit prerequisiteUnit, string requirement)
    {
        UnitId = unitId;
        UnitName = unitName;
        UnitCredit = unitCredit;
        MaxStudents = maxStudents;
        PrerequisiteUnit = prerequisiteUnit;
        Requirement = requirement;
        Lecturer = lecturer;
        // EnrolledStudents = new List<Student>();
        AvailableTimeslots = new List<Timeslot>();
    }
    public static List<Unit> ReadUnitsFromCSV(string filePath)
    {
        List<Unit> units = new List<Unit>();
        var lines = File.ReadLines(filePath);

        // Skip the first line (column headers)
        bool firstLine = true;

        foreach (var line in lines)
        {
            if (firstLine)
            {
                firstLine = false;
                continue; // Skip the first line
            }

            var fields = line.Split(',');
            if (fields.Length >= 3)
            {
                string unitId = fields[0];
                string unitName = fields[1];
                string lecturer = fields[2];
                // Parse other fields as needed.
                int unitCredit = 0;
                int maxStudents = 0;

                Unit unit = new Unit(unitId, unitName, lecturer, unitCredit, maxStudents, null, null);
                units.Add(unit);
            }
        }


        return units;
    }

    public void AssignLecturer(string lecturer)
    {
        Lecturer = lecturer;
    }

    public void UnassignLecturer()
    {
        Lecturer = null;
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
        University university = new University("Curtin University");

        // Create a department
        Department computerScience = new Department("Computer Science");

        // Create a venue
        List<Unit> units = Unit.ReadUnitsFromCSV("unitsample.csv");
        List<Venue> venues = Venue.ReadVenuesFromCSV("venueSample.csv");

        // Add the units to the department
        foreach (var unit in units)
        {
            computerScience.AddUnit(unit);
        }

        // Add the venues to the university
        foreach (var venue in venues)
        {
            university.AddVenue(venue);
        }
        // Create some timeslots
        Timeslot timeslot1 = new Timeslot(1, "Monday", new DateTime(2023, 10, 16, 9, 0, 0), new DateTime(2023, 10, 16, 10, 30, 0));
        Timeslot timeslot2 = new Timeslot(2, "Monday", new DateTime(2023, 10, 16, 10, 45, 0), new DateTime(2023, 10, 16, 12, 15, 0));

        // Create a unit
        //Unit computerScience101 = new Unit("unit1001", "Introduction to Computer Science", 3, 30, null, "None");

        // Assign staff to the unit
        //Staff professorSmith = new Staff("Prof. Smith");
        //computerScience101.AssignStaff("professorSmith");

        // Add timeslots to the unit
        //computerScience101.AddTimeslot(timeslot1);
        //computerScience101.AddTimeslot(timeslot2);

        // Add the department to the university
        university.AddDepartment(computerScience);

        // Output some information
        Console.WriteLine("University Name: " + university.Name);
        Console.WriteLine("Department Name: " + computerScience.Name);

        Console.WriteLine("--------------------------------------------");
        foreach (var venue in venues)
        {
            Console.WriteLine("Venue ID: " + venue.VenueId);
            Console.WriteLine("Venue Name: " + venue.VenueName);
            Console.WriteLine("");
        }
       Console.WriteLine("--------------------------------------------");
        foreach (var unit in units)
        {
            Console.WriteLine("Unit Id: " + unit.UnitId);
            Console.WriteLine("Unit Name: " + unit.UnitName);
            Console.WriteLine("Unit Lecturer: " + unit.Lecturer);
            Console.WriteLine("");
        }
    }
}