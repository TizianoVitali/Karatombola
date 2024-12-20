﻿using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace Karatombola.Pages;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("Brice-Black.ttf", "BriceBlack");
				fonts.AddFont("WaltographUI.ttf", "Disney");
				fonts.AddFont("Doyle-Regular.ttf", "DoyleRegular");
			});

		builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddTransient<PlayPage>();
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
