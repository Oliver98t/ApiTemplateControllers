using ApiTemplateControllers.Models;

namespace ApiTemplateControllers.BaseServices;

public class BaseService<TModel> : CRUD<TModel> where TModel : class, IBaseModel
{
	public BaseService(ApiContext context) : base(context){}
}