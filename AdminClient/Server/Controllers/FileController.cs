using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using TransferModel;

namespace AdminClient.Server.Controllers;

public class FileController : ControllerBase
{
	private readonly IWebHostEnvironment env;

	public FileController(IWebHostEnvironment env)
	{
		this.env = env;
	}

	[HttpPost]
	public async Task<List<string>> UploadFile(List<IFormFile> files)
	{
		var uploadFileNames = new List<string>();

		foreach (var file in files)
		{
			var path = Path.Combine(env.ContentRootPath, "..", "..", "uploads", file.FileName);

			await using var fs = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(fs);

            uploadFileNames.Add(file.FileName);
		}

		return uploadFileNames;
	}

    public string GetFileExtensionFromMimeType(string mimeType)
		=> mimeType switch
		{
		    "image/png" => ".png",
		    _ => null,
		};
}
