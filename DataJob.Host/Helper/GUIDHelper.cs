using System;

namespace DataJob.Server.Helper
{
    public static class GUIDHelper
    {
        public static string NewGuid => Guid.NewGuid().ToString("N");
    }
}
