namespace Aviationexam.Api.TpsSample.Models;

public class ExamSetSubjectOutput
{
    public required int IdSubject { get; set; }

    public required string SubjectName { get; set; }

    public required ExamSetSubjectStatus Status { get; set; }

    public required IReadOnlyCollection<ExamOutput> Exams { get; set; } = null!;
}
