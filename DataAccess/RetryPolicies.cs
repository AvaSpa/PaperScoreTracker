using Microsoft.Data.Sqlite;
using Polly;
using Polly.Retry;

namespace DataAccess;

internal static class RetryPolicies
{
    public static RetryPolicy DataBaseInitializePolicy => Policy.Handle<SqliteException>().Retry();
}
