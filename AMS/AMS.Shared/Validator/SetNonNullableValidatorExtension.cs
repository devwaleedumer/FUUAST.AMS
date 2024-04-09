﻿using FluentValidation.Validators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.SHARED.Validator
{
    public static class SetNonNullableValidatorExtension
    {
        public static IRuleBuilderOptions<T, TProperty?> SetNonNullableValidator<T, TProperty>(this IRuleBuilder<T, TProperty?> ruleBuilder, IValidator<TProperty> validator, params string[] ruleSets)
        {
            var adapter = new NullableChildValidatorAdaptor<T, TProperty>(validator, validator.GetType())
            {
                RuleSets = ruleSets
            };

            return ruleBuilder.SetAsyncValidator(adapter);
        }

        private class NullableChildValidatorAdaptor<T, TProperty> : ChildValidatorAdaptor<T, TProperty>, IPropertyValidator<T, TProperty?>, IAsyncPropertyValidator<T, TProperty?>
        {
            public NullableChildValidatorAdaptor(IValidator<TProperty> validator, Type validatorType)
                : base(validator, validatorType)
            {
            }
#pragma warning disable RCS1132
            public override bool IsValid(ValidationContext<T> context, TProperty? value)
            {
                return base.IsValid(context, value!);
            }

            public override Task<bool> IsValidAsync(ValidationContext<T> context, TProperty? value, CancellationToken cancellation)
            {
                return base.IsValidAsync(context, value!, cancellation);
            }
#pragma warning restore RCS1132
        }
    }
}
