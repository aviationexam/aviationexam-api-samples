namespace Aviationexam.Api.TpsSample.Models;

public enum ExamStatus : byte
{
    Planned = 0,
    Generated = 1,
    Paused = 2,
    Running = 3,
    Passed = 4,
    Failed = 5,
    Cancelled = 6,
    Approved = 7,
    Hidden = 100,
}

public enum ExamSetSubjectStatus : byte
{
    Open = 0,
    Planned = 1,
    Failed = 2,
    Passed = 3,
    Approved = 4,
}

public enum ExamSetStatus : byte
{
    InProgress = 0,
    Passed = 1,
    Failed = 2,
}
