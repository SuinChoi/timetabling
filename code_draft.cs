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
    public List<Staff> Staff { get; }

    public Department(string name)
    {
        Name = name;
        Units = new List<Unit>();
        Staff = new List<Staff>();
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
        StartTime = start;
        EndTime = end;
        Duration = EndTime - StartTime;
    }
}

public class Unit
{
    public int UnitId { get; }
    public string UnitName { get; }
    public int UnitCredit { get; }
    public int MaxStudents { get; }
    public Unit PrerequisiteUnit { get; }
    public string Requirement { get; }
    public Staff AssignedStaff { get; set; }
    public List<Student> EnrolledStudents { get; }
    public List<Timeslot> AvailableTimeslots { get; }

    public Unit(int unitId, string unitName, int unitCredit, int maxStudents, Unit prerequisiteUnit, string requirement)
    {
        UnitId = unitId;
        UnitName = unitName;
        UnitCredit = unitCredit;
        MaxStudents = maxStudents;
        PrerequisiteUnit = prerequisiteUnit;
        Requirement = requirement;
        AssignedStaff = null;
        EnrolledStudents = new List<Student>();
        AvailableTimeslots = new List<Timeslot>();
    }

    public void AssignStaff(Staff staff)
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

