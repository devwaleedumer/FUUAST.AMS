using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SHARED.Validator
{

    public interface ICustomValidatorFactory
    {
        IValidator GetValidatorFor(Type type);
    }

    public class CustomValidatorFactory : ICustomValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CustomValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator GetValidatorFor(Type type)
        {
            var genericValidatorType = typeof(IValidator<>);
            var specificValidatorType = genericValidatorType.MakeGenericType(type);
            using (var scope = _serviceProvider.CreateScope())
            {
                var validatorInstance = (IValidator)scope.ServiceProvider.GetService(specificValidatorType);
                return validatorInstance;
            }
        }
    }
}
