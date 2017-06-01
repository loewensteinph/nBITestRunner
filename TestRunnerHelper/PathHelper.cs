using System;
using System.Text;

namespace TestrunnerHelper
{
    public class PathHelper
    {
        public static string MakeRelative(string absPath, string relTo)
        {
            var absDirs = absPath.Split('\\');
            var relDirs = relTo.Split('\\');
            var len = absDirs.Length < relDirs.Length ? absDirs.Length : relDirs.Length;
            var lastCommonRoot = -1;
            int index;
            for (index = 0; index < len; index++)
                if (absDirs[index] == relDirs[index])
                    lastCommonRoot = index;
                else break;
            if (lastCommonRoot == -1)
                throw new ArgumentException("Paths do not have a common base");
            var relativePath = new StringBuilder();
            for (index = lastCommonRoot + 1; index < absDirs.Length; index++)
                if (absDirs[index].Length > 0) relativePath.Append("..\\");
            for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++)
                relativePath.Append(relDirs[index] + "\\");
            relativePath.Append(relDirs[relDirs.Length - 1]);
            var res = relativePath.ToString();
            return relativePath.ToString();
        }
    }
}