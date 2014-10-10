namespace ADO.Models
{
    [System.ComponentModel.DefaultValue(NonQuery)]
    public enum enumCommandExecutionType
    {
        NonQuery,
        Scalar,
        DataTable,
        DataSet
    }
}
