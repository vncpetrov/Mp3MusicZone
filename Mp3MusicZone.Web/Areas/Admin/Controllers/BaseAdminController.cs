namespace Mp3MusicZone.Web.Areas.Admin.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System;

	using static Common.Constants.WebConstants;

	[Area("Admin")]
	[Authorize(Roles = AdministratorRole)]
	public abstract class BaseAdminController : Controller
	{
	}
}
