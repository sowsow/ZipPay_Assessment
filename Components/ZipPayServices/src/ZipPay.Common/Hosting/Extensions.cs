using System;
using Microsoft.Extensions.DependencyInjection;

namespace ZipPay.Common.Hosting
{
    public static class Extensions
    {
        public static IServiceCollection AddEssentials(this IServiceCollection services)
        {
            return services
                .AddSingleton<Func<DateTime>>(() => DateTime.Now);
        }
    }
}
