using AuditTrailMVC.Models;
using AuditTrailMVC.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace AuditTrailMVC.Filters
{
    public class AuditFilterAttribute: ActionFilterAttribute
    {
        private readonly IAuditRepository _auditRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditFilterAttribute(IAuditRepository auditRepository, IHttpContextAccessor httpContextAccessor)
        {
            _auditRepository = auditRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var audit = new Audit(); //Getting Action Name 
            var controllerName = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ControllerName;
            var actionName = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.ActionName;
            var actionDescriptorRouteValues = ((ControllerBase)filterContext.Controller).ControllerContext.ActionDescriptor.RouteValues;
            if(actionDescriptorRouteValues.ContainsKey("area"))
            {
                var area = actionDescriptorRouteValues["area"];
                if(area != null)
                {
                    audit.Area = area;
                }
            }
            var request = filterContext.HttpContext.Request;
            //if (!string.IsNullOrEmpty(Convert.ToString(filterContext.HttpContext.Session.GetInt32(AllSessionKeys))) ;
            if(filterContext.HttpContext.Session !=null) audit.SessionId = filterContext.HttpContext.Session.Id;
            
            if (_httpContextAccessor.HttpContext != null)
            {
                audit.IpAddress = Convert.ToString(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress);
            }
            audit.PageAccessed = Convert.ToString(filterContext.HttpContext.Request.Path);
            audit.LoginStatus = "Active";
            audit.ControllerName = controllerName; // ControllerName 
            audit.ActionName = actionName;
            audit.UserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            RequestHeaders header = request.GetTypedHeaders();
            Uri uriReferer = header.Referer;

            if (uriReferer != null)
            {
                audit.UrlReferrer = header.Referer.AbsoluteUri;
            }

            await _auditRepository.AddAuditTrail(audit);

        }
    }
}
