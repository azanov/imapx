using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ImapX.WebSample.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class HandleErrorCodeAttribute : FilterAttribute, IExceptionFilter
    {

        private Type _exceptionType = typeof(Exception);

        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.IsChildAction ||
                (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)) return;

            var innerException = filterContext.Exception;

            if ((new HttpException(null, innerException).GetHttpCode() != 500) ||
                !ExceptionType.IsInstanceOfType(innerException)) return;

            var result = new HttpStatusCodeResult(StatusCode);

            filterContext.Result = result;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)StatusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

        public Type ExceptionType
        {
            get
            {
                return _exceptionType;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                if (!typeof(Exception).IsAssignableFrom(value))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "The type '{0}' does not inherit from Exception.", new object[] { value.FullName }));
                }
                _exceptionType = value;
            }
        }

        [DefaultValue(HttpStatusCode.InternalServerError)]
        public HttpStatusCode StatusCode { get; set; }


    }

}
