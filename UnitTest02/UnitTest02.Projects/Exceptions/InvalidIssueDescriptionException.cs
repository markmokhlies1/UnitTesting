namespace UnitTest02.Projects.Exceptions
{
    public class InvalidIssueDescriptionException : Exception
    {
        public InvalidIssueDescriptionException() : base("issue description cannot be null or whitespace")
        {
        }
    }
}
