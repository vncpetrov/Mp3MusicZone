namespace Mp3MusicZone.Web.Areas.Uploader.Controllers
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System;

	using static Common.Constants.WebConstants;

	[Area("Uploader")]
	[Authorize(Roles = AdministratorRole)]
	[Authorize(Roles = UploaderRole)]
	public class BaseUploaderController : Controller
	{
	}
}
