using System;

namespace Aviationexam.LmsApiSample.Models;

public class GetLmsStudentExamOutput
{
    public required int UserId { get; set; }

    public required string SubjectName { get; set; }

    public required long ExamId { get; set; }

    public required string ExamName { get; set; }

    public DateTime? DueBy { get; set; }

    public required DateTime CompletedOn { get; set; }

    public required decimal Score { get; set; }

    public required int ElapsedTimeSeconds { get; set; }
}
