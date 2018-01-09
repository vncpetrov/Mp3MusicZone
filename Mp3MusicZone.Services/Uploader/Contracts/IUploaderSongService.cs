namespace Mp3MusicZone.Services.Uploader.Contracts
{
	using System;
	using System.Threading.Tasks;

	public interface IUploaderSongService
    {
		Task<bool> ExistsAsync(string name);

		Task<bool> UploadAsync(
			string userId,
			string name, 
			string singer,
			int categoryId,
			byte[] file);
	}
}
