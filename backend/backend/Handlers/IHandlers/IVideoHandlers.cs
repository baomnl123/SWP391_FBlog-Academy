﻿using backend.DTO;
using backend.Models;

namespace backend.Handlers.IHandlers
{
    public interface IVideoHandlers
    {
        public VideoDTO GetVideoByID(int videoId);
        public VideoDTO GetVideoByURL(string videoURL);
        public ICollection<VideoDTO> GetVideosByPost(int postId);
        public bool CreateVideo(int postId, string[] videos);
        public bool UpdateVideo(int currentVideoId, string newVideoURL);
        public bool EnableVideo(int videoId);
        public bool DisableVideo(int videoId);
    }
}