using System;
using System.Collections.Generic;

namespace Aviationexam.LmsApiSample
{
    public class ContinuationResponse<T>
    {
        public IEnumerable<T> Items { get; set; }
        
        public string ContinuationToken { get; set; }
    }

    public class GetLmsStudentOutput
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string LmsNote { get; set; }
    }

    public class GetLmsStudentExamOutput
    {
        public int UserId { get; set; }

        public string SubjectName { get; set; }

        public long ExamId { get; set; }

        public string ExamName { get; set; }

        public DateTime? DueBy { get; set; }

        public DateTime CompletedOn { get; set; }

        public decimal Score { get; set; }

        public int ElapsedTimeSeconds { get; set; }
    }
}
