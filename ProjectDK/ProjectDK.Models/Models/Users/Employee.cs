namespace ProjectDK.Models.Models.Users
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        public string EmployeeName { get; set; }

        public string NationalIDNumber { get; set; }

        public string LoginID { get; set; }

        public string JobTitle { get; set; }

        public DateTime BirthDate { get; set; }

        public char MaritalStatus { get; set; }

        public char Gender { get; set; }

        public DateTime HireDate { get; set; }

        public short VacationHours { get; set; }

        public short SickLeaveHours { get; set; }

        public Guid rowguid { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
