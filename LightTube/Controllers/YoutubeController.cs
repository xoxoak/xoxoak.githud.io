﻿using System.Text.Json;
using InnerTube;
using LightTube.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace LightTube.Controllers;

public class YoutubeController : Controller
{
	private readonly InnerTube.InnerTube _youtube;
	private readonly HttpClient _client;

	public YoutubeController(InnerTube.InnerTube youtube, HttpClient client)
	{
		_youtube = youtube;
		_client = client;
	}

	[Route("/embed/{v}")]
	public async Task<IActionResult> Embed(string v, bool contentCheckOk, bool compatibility = false)
	{
		InnerTubePlayer player;
		Exception? e;
		try
		{
			player = await _youtube.GetPlayerAsync(v, contentCheckOk, false, HttpContext.GetLanguage(),
				HttpContext.GetRegion());
			e = null;
		}
		catch (Exception ex)
		{
			player = null;
			e = ex;
		}
		InnerTubeNextResponse video =
			await _youtube.GetVideoAsync(v, language: HttpContext.GetLanguage(), region: HttpContext.GetRegion());
		if (player is null || e is not null)
			return View(new EmbedContext(e ?? new Exception("player is null"), video));
		return View(new EmbedContext(player, video, compatibility));
	}

	[Route("/watch")]
	public async Task<IActionResult> Watch(string v, string list, bool contentCheckOk, bool compatibility = false)
	{
		InnerTubePlayer player;
		Exception? e;
		try
		{
			player = await _youtube.GetPlayerAsync(v, contentCheckOk, false, HttpContext.GetLanguage(),
				HttpContext.GetRegion());
			e = null;
		}
		catch (Exception ex)
		{
			player = null;
			e = ex;
		}
		InnerTubeNextResponse video =
			await _youtube.GetVideoAsync(v, list, language: HttpContext.GetLanguage(), region: HttpContext.GetRegion());
		int dislikes;
		try
		{
			HttpResponseMessage rydResponse =
				await _client.GetAsync("https://returnyoutubedislikeapi.com/votes?videoId=" + v);
			Dictionary<string, JsonElement> rydJson =
				JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(await rydResponse.Content.ReadAsStringAsync())!;
			dislikes = rydJson["dislikes"].GetInt32();
		}
		catch
		{
			dislikes = -1;
		}
		if (player is null || e is not null)
			return View(new WatchContext(e ?? new Exception("player is null"), video, dislikes, HttpContext));
		return View(new WatchContext(player, video, compatibility, dislikes, HttpContext));
	}
}