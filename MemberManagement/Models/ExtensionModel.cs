using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace MemberManagement.Models
{
    public static class ExtensionModel
    {
        public static string GetErrorModelState(this ModelStateDictionary modelState)
        {
            var modelValue = modelState.Values
                                .Select(v => v.Errors)
                                . Where(value => value.Count() > 0)
                                .FirstOrDefault();
            if (modelValue == null)
            {
                return null;
            }
            return modelValue[0].ErrorMessage;
        }

        public static Exception GetErrorException(this Exception excption) {
            return excption.InnerException != null ? excption.InnerException.GetErrorException(): excption;
        }
    }
}