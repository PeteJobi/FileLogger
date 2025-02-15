using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FileLogger
{
    public static class Extensions
    {
        public static void AddFileLogger(this IServiceCollection serviceCollection, string folderName, string? folderDirectory = null, string? dateTimeFormat = null)
        {
            serviceCollection.AddSingleton<IFileLogger>(new FileLogger(folderName, folderDirectory, dateTimeFormat));
        }
    }
}
