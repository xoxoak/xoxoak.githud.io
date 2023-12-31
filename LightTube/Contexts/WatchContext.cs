﻿using InnerTube;
using LightTube.Database.Models;

namespace LightTube.Contexts;

public class WatchContext : BaseContext
{
	public PlayerContext Player;
	public InnerTubeNextResponse Video;
	public InnerTubePlaylistInfo? Playlist;
	public InnerTubeContinuationResponse? Comments;
	
	public int Dislikes;

	public WatchContext(HttpContext context, InnerTubePlayer innerTubePlayer, InnerTubeNextResponse innerTubeNextResponse,
		InnerTubeContinuationResponse? comments,
		bool compatibility, int dislikes) : base(context)
	{
		Player = new PlayerContext(context, innerTubePlayer, "embed", compatibility, context.Request.Query["q"]);
		Video = innerTubeNextResponse;
		Playlist = Video.Playlist;
		Comments = comments;
		Dislikes = dislikes;
		GuideHidden = true;

		AddMeta("description", Video.Description);
		AddMeta("author", Video.Channel.Title);
		AddMeta("og:title", Video.Title);
		AddMeta("og:description", Video.Description);
		AddMeta("og:url", $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
		AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:player", $"https://{context.Request.Host}/embed/${Video.Id}");
		AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/${Video.Id}/18");

		AddStylesheet("/lib/videojs/video-js.min.css");
		AddStylesheet("/lib/videojs-endscreen/videojs-endscreen.css");
		AddStylesheet("/lib/videojs-vtt-thumbnails/videojs-vtt-thumbnails.min.css");
		AddStylesheet("/lib/videojs-hls-quality-selector/videojs-hls-quality-selector.css");
		AddStylesheet("/lib/silvermine-videojs-quality-selector/silvermine-videojs-quality-selector.css");
		AddStylesheet("/css/vjs-skin.css");

		AddScript("/lib/videojs/video.min.js");
		AddScript("/lib/videojs-hotkeys/videojs.hotkeys.min.js");
		AddScript("/lib/videojs-endscreen/videojs-endscreen.js");
		AddScript("/lib/videojs-vtt-thumbnails/videojs-vtt-thumbnails.min.js");
		AddScript("/lib/videojs-contrib-quality-levels/videojs-contrib-quality-levels.min.js");
		AddScript("/lib/videojs-hls-quality-selector/videojs-hls-quality-selector.min.js");
		AddScript("/lib/silvermine-videojs-quality-selector/silvermine-videojs-quality-selector.min.js");
		AddScript("/js/player.js");
	}

	public WatchContext(HttpContext context, Exception e, InnerTubeNextResponse innerTubeNextResponse,
		InnerTubeContinuationResponse? comments, int dislikes) : base(context)
	{
		Player = new PlayerContext(context, e);
		Video = innerTubeNextResponse;
		Playlist = Video.Playlist;
		Comments = comments;
		Dislikes = dislikes;
		GuideHidden = true;

		AddMeta("description", Video.Description);
		AddMeta("author", Video.Channel.Title);
		AddMeta("og:title", Video.Title);
		AddMeta("og:description", Video.Description);
		AddMeta("og:url", $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
		AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:player", $"https://{context.Request.Host}/embed/${Video.Id}");
		AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/${Video.Id}/18");
	}

	public WatchContext(HttpContext context, InnerTubePlayer innerTubePlayer, InnerTubeNextResponse innerTubeNextResponse, DatabasePlaylist? playlist,
		InnerTubeContinuationResponse? comments,
		bool compatibility, int dislikes) : base(context)
	{
		Player = new PlayerContext(context, innerTubePlayer, "embed", compatibility, context.Request.Query["q"]);
		Video = innerTubeNextResponse;
		Playlist = playlist?.GetInnerTubePlaylistInfo(innerTubePlayer.Details.Id);
		if (playlist != null && playlist.Visibility == PlaylistVisibility.PRIVATE)
			if (playlist.Author != User?.UserID) 
				Playlist = null;
		Comments = comments;
		Dislikes = dislikes;
		GuideHidden = true;

		AddMeta("description", Video.Description);
		AddMeta("author", Video.Channel.Title);
		AddMeta("og:title", Video.Title);
		AddMeta("og:description", Video.Description);
		AddMeta("og:url", $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
		AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:player", $"https://{context.Request.Host}/embed/${Video.Id}");
		AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/${Video.Id}/18");

		AddStylesheet("/lib/videojs/video-js.min.css");
		AddStylesheet("/lib/videojs-endscreen/videojs-endscreen.css");
		AddStylesheet("/lib/videojs-vtt-thumbnails/videojs-vtt-thumbnails.min.css");
		AddStylesheet("/lib/videojs-hls-quality-selector/videojs-hls-quality-selector.css");
		AddStylesheet("/lib/silvermine-videojs-quality-selector/silvermine-videojs-quality-selector.css");
		AddStylesheet("/css/vjs-skin.css");

		AddScript("/lib/videojs/video.min.js");
		AddScript("/lib/videojs-hotkeys/videojs.hotkeys.min.js");
		AddScript("/lib/videojs-endscreen/videojs-endscreen.js");
		AddScript("/lib/videojs-vtt-thumbnails/videojs-vtt-thumbnails.min.js");
		AddScript("/lib/videojs-contrib-quality-levels/videojs-contrib-quality-levels.min.js");
		AddScript("/lib/videojs-hls-quality-selector/videojs-hls-quality-selector.min.js");
		AddScript("/lib/silvermine-videojs-quality-selector/silvermine-videojs-quality-selector.min.js");
		AddScript("/js/player.js");
	}

	public WatchContext(HttpContext context, Exception e, InnerTubeNextResponse innerTubeNextResponse, DatabasePlaylist? playlist,
		InnerTubeContinuationResponse? comments, int dislikes) : base(context)
	{
		Player = new PlayerContext(context, e);
		Video = innerTubeNextResponse;
		Playlist = playlist?.GetInnerTubePlaylistInfo(innerTubeNextResponse.Id);
		if (playlist != null && playlist.Visibility == PlaylistVisibility.PRIVATE)
			if (playlist.Author != User?.UserID) 
				Playlist = null;
		Comments = comments;
		Dislikes = dislikes;
		GuideHidden = true;

		AddMeta("description", Video.Description);
		AddMeta("author", Video.Channel.Title);
		AddMeta("og:title", Video.Title);
		AddMeta("og:description", Video.Description);
		AddMeta("og:url", $"{context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}");
		AddMeta("og:image", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:card", $"{context.Request.Scheme}://{context.Request.Host}/proxy/thumbnail/{Video.Id}/-1");
		AddMeta("twitter:player", $"https://{context.Request.Host}/embed/${Video.Id}");
		AddMeta("twitter:player:stream", $"https://{context.Request.Host}/proxy/media/${Video.Id}/18");
	}
}