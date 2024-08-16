// THIS FILE IS PART OF Xunet PROJECT
// THE Xunet PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet

namespace Xunet.Dapper.DbContext;

using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;

/// <summary>
/// IDbContext
/// </summary>
public interface IDbContext
{
    /// <summary>
    /// 创建只读DbConnection
    /// </summary>
    /// <returns></returns>
    public IDbConnection CreateReadOnlyDbConnection();

    /// <summary>
    /// 创建读写DbConnection
    /// </summary>
    /// <returns></returns>
    public IDbConnection CreateReadWriteDbConnection();
}

/// <summary>
/// 数据库访问上下文基类
/// </summary>
/// <typeparam name="DbContext"></typeparam>
public class BaseDbContext<DbContext> where DbContext : IDbContext
{
    /// <summary>
    /// 当前DbContext
    /// </summary>
    static IDbContext CurrentDbContext => Activator.CreateInstance(typeof(DbContext)) as IDbContext;

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static IEnumerable<T> Query<T>(string sql, object param = null)
    {
        using var connection = CurrentDbContext.CreateReadOnlyDbConnection();

        return connection.Query<T>(sql, param, null, true, null, null);
    }

    /// <summary>
    /// 单条查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static T QueryFirstOrDefault<T>(string sql, object param = null)
    {
        using var connection = CurrentDbContext.CreateReadOnlyDbConnection();

        return connection.QueryFirstOrDefault<T>(sql, param, null, null, null);
    }

    /// <summary>
    /// 增删改
    /// </summary>
    /// <param name="sql"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public static int Execute(string sql, object param = null)
    {
        using var connection = CurrentDbContext.CreateReadWriteDbConnection();

        return connection.Execute(sql, param, null, null, null);
    }

    /// <summary>
    /// 列表查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression">表达式</param>
    /// <returns></returns>
    public static IEnumerable<T> Select<T>(Expression<Func<T, bool>> expression = null)
    {
        return Query<T>(SqlBuilder.SelectSql(expression));
    }

    /// <summary>
    /// 单条查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression">表达式</param>
    /// <returns></returns>
    public static T SelectFirstOrDefault<T>(Expression<Func<T, bool>> expression = null)
    {
        return QueryFirstOrDefault<T>(SqlBuilder.SelectSql(expression));
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Insert<T>(T entity)
    {
        return Execute(SqlBuilder.InsertSql<T>(), entity);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="expression">表达式</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Update<T>(T entity, Expression<Func<T, bool>> expression)
    {
        return Execute(SqlBuilder.UpdateSql(expression), entity);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="expression">表达式</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static int Delete<T>(Expression<Func<T, bool>> expression)
    {
        return Execute(SqlBuilder.DeleteSql(expression));
    }

    /// <summary>
    /// 增删改（事务）
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public static int ExecuteTran(string sql)
    {
        if (string.IsNullOrEmpty(sql)) return 0;

        using var connection = CurrentDbContext.CreateReadWriteDbConnection();

        using var transaction = connection.BeginTransaction();

        var result = 0;

        try
        {
            result = connection.Execute(sql, null, transaction, null, null);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw;
        }

        return result;
    }

    /// <summary>
    /// 增删改（事务）
    /// </summary>
    /// <param name="action">主方法</param>
    /// <param name="failedAction">失败回调</param>
    /// <returns></returns>
    public static bool ExecuteTran(Action<IDbConnection, IDbTransaction> action, Action<Exception> failedAction = null)
    {
        using var connection = CurrentDbContext.CreateReadWriteDbConnection();

        using var transaction = connection.BeginTransaction();

        try
        {
            action(connection, transaction);

            transaction.Commit();

            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();

            failedAction?.Invoke(ex);

            return false;
        }
    }
}

/// <summary>
/// Sql构建器
/// Author：徐来
/// Date：2024.7.24
/// </summary>
public class SqlBuilder
{
    public static string SelectSql<T>(Expression<Func<T, bool>> expression = null)
    {
        var type = typeof(T);

        var tableName = type.GetCustomAttribute<DescriptionAttribute>()?.Description;

        if (string.IsNullOrEmpty(tableName)) throw new InvalidOperationException($"{type.Name}缺少Description特性");

        var columns = string.Join(",", type.GetProperties().Select(x => x.Name));

        var sql = $"SELECT {columns} FROM {tableName};";

        if (expression != null)
        {
            sql = $"SELECT {columns} FROM {tableName} WHERE {Where(expression)};";
        }

        return sql;
    }

    public static string InsertSql<T>()
    {
        var type = typeof(T);

        var tableName = type.GetCustomAttribute<DescriptionAttribute>()?.Description;

        if (string.IsNullOrEmpty(tableName)) throw new InvalidOperationException($"{type.Name}缺少Description特性");

        var columns = string.Join(",", type.GetProperties().Select(x => x.Name));

        var values = string.Join(",", type.GetProperties().Select(x => $"@{x.Name}"));

        var sql = $"INSERT INTO {tableName}({columns}) VALUES({values});";

        return sql;
    }

    public static string UpdateSql<T>(Expression<Func<T, bool>> expression)
    {
        var type = typeof(T);

        var tableName = type.GetCustomAttribute<DescriptionAttribute>()?.Description;

        if (string.IsNullOrEmpty(tableName)) throw new InvalidOperationException($"{type.Name}缺少Description特性");

        var columns = string.Join(",", type.GetProperties().Select(x => $"{x.Name} = @{x.Name}"));

        var sql = $"UPDATE {tableName} SET {columns} WHERE {Where(expression)};";

        return sql;
    }

    public static string DeleteSql<T>(Expression<Func<T, bool>> expression)
    {
        var type = typeof(T);

        var tableName = type.GetCustomAttribute<DescriptionAttribute>()?.Description;

        if (string.IsNullOrEmpty(tableName)) throw new InvalidOperationException($"{type.Name}缺少Description特性");

        var sql = $"DELETE FROM {tableName} WHERE {Where(expression)};";

        return sql;
    }

    static string Where<T>(Expression<Func<T, bool>> expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        return GetOperand(expression.Body);
    }

    static string GetOperand(Expression operand)
    {
        return operand switch
        {
            BinaryExpression binaryExpression => BinaryExpressionProvider(binaryExpression),
            ConditionalExpression conditionalExpression => ConditionalExpressionProvider(conditionalExpression),
            LambdaExpression lambdaExpression => LambdaExpressionProvider(lambdaExpression),
            MemberExpression memberExpression => MemberExpressionProvider(memberExpression),
            NewArrayExpression newArrayExpression => NewArrayExpressionProvider(newArrayExpression),
            ConstantExpression constantExpression => ConstantExpressionProvider(constantExpression),
            MethodCallExpression methodCallExpression => MethodCallExpressionProvider(methodCallExpression),
            UnaryExpression unaryExpression => UnaryExpressionProvider(unaryExpression),
            _ => throw new NotSupportedException($"Unsupported operand type {operand.Type}."),
        };
    }

    // 表示具有二进制运算符的表达式
    static string BinaryExpressionProvider(BinaryExpression binaryExpression)
    {
        var oper = binaryExpression.NodeType switch
        {
            ExpressionType.And or ExpressionType.AndAlso => "AND",
            ExpressionType.Equal => "=",
            ExpressionType.GreaterThan => ">",
            ExpressionType.GreaterThanOrEqual => ">=",
            ExpressionType.LessThan => "<",
            ExpressionType.LessThanOrEqual => "<=",
            ExpressionType.NotEqual => "<>",
            ExpressionType.Or or ExpressionType.OrElse => "OR",
            ExpressionType.Add or ExpressionType.AddChecked => "+",
            ExpressionType.Subtract or ExpressionType.SubtractChecked => "-",
            ExpressionType.Divide => "/",
            ExpressionType.Multiply or ExpressionType.MultiplyChecked => "*",
            _ => throw new NotSupportedException("Unsupported binary operator."),
        };

        // 递归处理嵌套的二元表达式
        var left = GetOperand(binaryExpression.Left);
        var right = GetOperand(binaryExpression.Right);

        return $"({left} {oper} {right})";
    }

    // 表示访问字段或属性
    static string MemberExpressionProvider(MemberExpression memberExpression)
    {
        if (memberExpression.Member is FieldInfo fieldInfo)
        {
            return fieldInfo.Name;
        }

        if (memberExpression.Member is PropertyInfo propertyInfo)
        {
            return propertyInfo.Name;
        }

        return memberExpression.Member.Name;
    }

    // 表示创建一个新数组，并可能初始化该新数组的元素
    static string NewArrayExpressionProvider(NewArrayExpression newArrayExpression)
    {
        var list = new List<string>();
        foreach (Expression expression in newArrayExpression.Expressions)
        {
            list.Add(GetOperand(expression));
        }
        return string.Join(",", list);
    }

    // 表示具有常数值的表达式
    static string ConstantExpressionProvider(ConstantExpression constantExpression)
    {
        return Type.GetTypeCode(constantExpression.Type) switch
        {
            TypeCode.String or TypeCode.Empty or TypeCode.Object or TypeCode.DBNull or TypeCode.Char or TypeCode.DateTime => $"'{constantExpression.Value}'",
            TypeCode.Boolean => $"{constantExpression.Value}".ToLower(),
            TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single or TypeCode.Double or TypeCode.Decimal => $"{constantExpression.Value}",
            _ => throw new NotSupportedException($"Unsupported type {constantExpression.Type.Name}."),
        };
    }

    // 表示对静态方法或实例方法的调用
    static string MethodCallExpressionProvider(MethodCallExpression methodCallExpression)
    {
        return methodCallExpression.Method.Name switch
        {
            "IsNullOrEmpty" => $"{GetOperand(methodCallExpression.Arguments[0])} IS NULL OR {GetOperand(methodCallExpression.Arguments[0])} = ''",
            "IsNotNullOrEmpty" => $"{GetOperand(methodCallExpression.Arguments[0])} IS NOT NULL AND {GetOperand(methodCallExpression.Arguments[0])} <> ''",
            _ => throw new NotSupportedException($"Unsupported method {methodCallExpression.Method.Name}."),
        };
    }

    // 表示具有一元运算符的表达式
    static string UnaryExpressionProvider(UnaryExpression unaryExpression)
    {
        // 取非解析
        if (unaryExpression.NodeType == ExpressionType.Not)
        {
            return $"NOT ({GetOperand(unaryExpression.Operand)})";
        }

        return GetOperand(unaryExpression.Operand);
    }

    // 表示具有条件运算符的表达式
    static string ConditionalExpressionProvider(ConditionalExpression conditionalExpression)
    {
        throw new NotImplementedException();
    }

    // 描述一个lambda表达式
    static string LambdaExpressionProvider(LambdaExpression lambdaExpression)
    {
        throw new NotImplementedException();
    }
}
