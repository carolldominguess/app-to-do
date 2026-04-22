using System.Data;
using App.ToDo.Domain.Enums;
using Dapper;

namespace App.ToDo.Infra.Dapper;
public class ToDoStatusTypeHandler : SqlMapper.TypeHandler<ToDoStatus>
{
    public override ToDoStatus Parse(object value) =>
        Enum.Parse<ToDoStatus>(value.ToString()!, ignoreCase: true);

    public override void SetValue(IDbDataParameter parameter, ToDoStatus value) =>
        parameter.Value = value.ToString();
}