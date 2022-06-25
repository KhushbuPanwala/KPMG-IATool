using InternalAuditSystem.DomailModel.DataRepository;
using InternalAuditSystem.DomailModel.Enums;
using InternalAuditSystem.DomailModel.Models.AuditableEntityModels;
using InternalAuditSystem.Utility.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace InternalAuditSystem.Core.ActionFilters
{
    public class EntityRestrictionFilter : ActionFilterAttribute
    {
        #region Private variable(s)
        private readonly IDataRepository _dataRepository;
        #endregion

        #region Constructor
        public EntityRestrictionFilter(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            var entityId = string.Empty;

            #region Get the entity Id
            if (filterContext.ActionArguments.ContainsKey(StringConstant.ParamEntityId1))
            {
                entityId = filterContext.ActionArguments[StringConstant.ParamEntityId1].ToString();
            }
            else if(filterContext.ActionArguments.ContainsKey(StringConstant.ParamEntityId2))
            {
                entityId = filterContext.ActionArguments[StringConstant.ParamEntityId2].ToString();
            }
            else if(filterContext.HttpContext.Request.Form != null && filterContext.HttpContext.Request.Form.ContainsKey(StringConstant.ParamEntityId1))
            {
                entityId = filterContext.HttpContext.Request.Form[StringConstant.ParamEntityId1][0].ToString();
            }
            #endregion

            var currentUser = Guid.Parse(filterContext.HttpContext.Request.Cookies[StringConstant.CurrentUserIdKey]);

            var currentEntity = _dataRepository.FirstOrDefault<AuditableEntity>(x => x.Id.ToString() == entityId);

            #region Restrict the action based on condition
            // if current member is not a member of the actioned entity
            if (currentEntity != null && currentUser != null 
                && (!_dataRepository.Any<DomailModel.Models.EntityUserMapping>(x => x.EntityId == currentEntity.Id && x.UserId == currentUser) &&
                currentEntity.CreatedBy != currentUser))
            {
                filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
            }
            // check if current entity is deleted
            else if (currentEntity != null && currentEntity.IsDeleted)
            {
                filterContext.Result = new StatusCodeResult((int)HttpStatusCode.MethodNotAllowed);
            }
            // check if current entity is on close state then restriction every action other than get
            else if(currentEntity != null &&  currentEntity.Status == AuditableEntityStatus.Closed && filterContext.HttpContext.Request.Method != StringConstant.HttpGetMethodKey)
            {
                filterContext.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
            // if current entity not found  
            else if(currentEntity == null)
            {
                filterContext.Result = new StatusCodeResult((int)HttpStatusCode.NotFound);
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
            #endregion
        }
    }
}
