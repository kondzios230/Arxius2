namespace Arxius.Services.PCL.Entities
{
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Url { get; set; }
        public string Index { get; set; }
        public string StudiesKind { get; set; }
        public string Semester { get; set; }
        public string SemesterString { get { return "Semestr: " + Semester; } }
        public string FullName { get { return Name + " " + Surname; } }

    }
}
